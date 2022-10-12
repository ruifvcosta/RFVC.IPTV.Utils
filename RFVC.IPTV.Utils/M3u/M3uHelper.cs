using RFVC.IPTV.Utils.Net6.Helpers;

namespace RFVC.IPTV.M3u
{
    public static class M3uHelper
    {

        #region Public

        public enum FileItemType
        {
            Tv = 0,
            Video = 1,
            Music = 2,
            Undefined = 3
        }

        /// <summary>
        /// Converts a M3u file content into a List of M3uFileItem to be processed by an APP
        /// </summary>
        /// <param name="fileContent"> M3u file content</param>
        /// <param name="maxItems">Limit the number of returned items ( optional) </param>
        /// <returns>List of M3uFileItems</returns>
        public static IList<M3uFileItem> GetM3UFileItems(string fileContent, int? maxItems = null)
        {
            if (string.IsNullOrEmpty(fileContent))
                throw new ArgumentNullException(nameof(fileContent));

            var result = new List<M3uFileItem>();
            using (StringReader reader = new StringReader(fileContent))
            {
                string? line;
                M3uFileItem? item = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (maxItems != null)
                        if (result.Count >= maxItems.Value)
                            return result;


                    if (!line.StartsWith("#EXTM3U"))
                    {
                        if (line.StartsWith("#EXTINF"))
                        {
                            if (item != null)
                            {
                                result.Add(item);
                            }

                            item = ParseLineIntoInfoFileItem(line);
                        }
                        else
                        {
                            if (item != null)
                            {
                                if (Uri.IsWellFormedUriString(line, UriKind.Absolute))
                                    item.Location = line;

                                item.Original += "\n" + line;
                            }
                        }
                    }
                }

                if (item != null)
                {
                    result.Add(item);
                }
                return result;
            }
        }

        /// <summary>
        /// Filters the groups of an M3u File and returns a file content with 
        /// only the items that belong to the filtered groups.
        /// Can be filtered by exact name or by part if is has a `*`
        /// ex: "sport*" or "portugal"
        /// </summary>
        /// <param name="fileContent">M3u File Content</param>
        /// <param name="groups">List containing the filters for the groups</param>
        /// <returns> Filtered M3u File text than can be saved into a file</returns>
        public static string FilterM3uFileByGroup(string fileContent, IList<string>? groups)
        {
            if (string.IsNullOrEmpty(fileContent))
                throw new ArgumentNullException(nameof(fileContent));
            if (groups == null)
                throw new ArgumentNullException(nameof(groups));

            if (groups.Count == 0)
                return fileContent;

            var wildGroups = groups.Where((f) => f.Contains("*")).ToList();


            using (StringWriter writer = new StringWriter())
            {
                using (StringReader reader = new StringReader(fileContent))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#EXTM3U"))
                        {
                            writer.WriteLine(line);
                        }
                        else
                        {
                            if (line.StartsWith("#EXTINF"))
                            {
                                // IF Contains the group, copy 2 lines
                                if (ContainsGroup(line, groups, wildGroups))
                                {
                                    writer.WriteLine(line);
                                    line = reader.ReadLine();
                                    writer.WriteLine(line);
                                }
                                else // if not, pass to the next line
                                    reader.ReadLine();

                            }
                        }
                    }
                }
                return writer.ToString();
            }
        }

        /// <summary>
        /// Tries to add logo information missing based on similar items of the same group.
        /// Copys the logo information of an item to another item with that has almost the same name
        /// ex: SPORT 1 and SPORT 1 HD
        /// The accuracy depends on the original information.. it tries its best..
        /// </summary>
        /// <param name="library"></param>
        /// <param name="minBestMatch">Use value between 0.7 and 0.99</param>
        public static void CompleteLogoInformation(IList<M3uFileItem> library, double minBestMatch = 0.85)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library));

            var emptyLogoList = library.Where((f) => f.LogoLocation == "").ToList();
            foreach (var item in emptyLogoList)
            {
                M3uFileItem? bestMach = null;
                double bestvalue = 0;
                var sameGroupList = library.Where((f) => f.Group == item.Group && f.LogoLocation != "").ToList();
                foreach (var itemInGroup in sameGroupList)
                {
                    if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(itemInGroup.Name))
                    {
                        var calculatedValue = StringHelper.CalculateSimilarity(item.Name, itemInGroup.Name);
                        if (calculatedValue > minBestMatch)
                        {
                            if (calculatedValue > bestvalue)
                            {
                                int differentChar = StringHelper.FirstDiferentCaracter(item.Name, itemInGroup.Name);
                                if (differentChar > -1)
                                {
                                    if (!Char.IsNumber(itemInGroup.Name.Length > item.Name.Length ? itemInGroup.Name[differentChar] : item.Name[differentChar]))
                                    {
                                        bestMach = itemInGroup;
                                    }
                                }
                            }
                        }
                    }

                }
                if (bestMach == null)
                    bestMach = library.OrderByDescending((o) => o.Name?.Length).FirstOrDefault((f) => f.Group == item.Group && f.LogoLocation != "" && f.Name?.ToLower().StartsWith(item.Name?.ToLower()) == true);

                if (bestMach == null)
                    bestMach = library.OrderByDescending((o) => o.Name?.Length).FirstOrDefault((f) => f.Group == item.Group && f.LogoLocation != "" && item.Name?.Contains(f.Name, StringComparison.OrdinalIgnoreCase) == true);

                if (bestMach != null)
                {
                    item.LogoLocation = bestMach.LogoLocation;
                }
            }
        }



        public static int GetM3uFileType(string location)
        {
            var extension = Path.GetExtension(location);
            if (string.IsNullOrEmpty(extension))
                return (int)FileItemType.Tv;

            switch (extension)
            {
                case ".mp4":
                case ".avi":
                case ".mkv":
                case ".flv":
                case ".wmv":
                case ".mpeg":
                case ".ts":
                    return (int)FileItemType.Video;
                case ".mp3":
                case ".flac":
                case ".aac":
                case ".wav":
                    return (int)FileItemType.Music;
                case ".m3u":
                case ".m3u8":
                    return (int)FileItemType.Undefined;
                default:
                    return (int)FileItemType.Undefined;

            }

        }



        /// <summary>
        /// Downloads List and filters
        /// </summary>
        /// <param name="url"></param>
        /// <param name="groups"></param>
        /// <returns>Filtered M3u File text than can be saved into a file</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<string> DownloadAndFilterM3uList(string? url, string? groups)
        {

            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = System.TimeSpan.FromMinutes(1);
                using (var response = await httpClient.GetAsync(url))
                {

                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(apiResponse))
                        return "";

                    if (string.IsNullOrEmpty(groups))
                        return apiResponse;
                    else
                    {
                        List<string> filters = groups.ToLower().Split("|").ToList();
                        string parsedFile = M3uHelper.FilterM3uFileByGroup(apiResponse, filters);
                        if (string.IsNullOrEmpty(parsedFile) || parsedFile == "#EXTM3U\r\n")
                            return apiResponse;
                        else
                            return parsedFile;
                    }

                }
            }
        }

        #endregion

        #region Private

        private static M3uFileItem ParseLineIntoInfoFileItem(string line)
        {
            var SplitedLines = line.Split("=");
            var item = new M3uFileItem();

            for (int i = 0; i < SplitedLines.Count() - 1; i++)
            {
                if (SplitedLines[i].ToLower().EndsWith("tvg-id"))
                    item.GuideID = GetFirstPart(SplitedLines[i + 1]);

                if (SplitedLines[i].ToLower().EndsWith("tvg-name"))
                    item.GuideName = GetFirstPart(SplitedLines[i + 1]);

                if (SplitedLines[i].ToLower().EndsWith("tvg-logo"))
                    item.LogoLocation = GetFirstPart(SplitedLines[i + 1]);

                if (SplitedLines[i].ToLower().EndsWith("group-title"))
                    item.Group = GetFirstPart(SplitedLines[i + 1], "\",");

            }
            if (SplitedLines[SplitedLines.Count() - 1].Contains(","))
                item.Name = SplitedLines[SplitedLines.Count() - 1].
                             Substring(SplitedLines[SplitedLines.Count() - 1].IndexOf(",") + 1);

            //save the original content
            item.Original = line;
            return item;
        }

        private static bool ContainsGroup(string line, IList<string> groups, List<string> wildGroups)
        {

            if (line.Contains("group-title="))
            {
                int index = line.IndexOf("group-title=");
                if (index != -1)
                {
                    string linegroup = GetFirstPart(line.Substring(index + "group-title=".Length), "\",");
                    if (!string.IsNullOrEmpty(linegroup))
                    {

                        var result = groups.Contains(linegroup.ToLower());
                        if (!result)
                        {
                            {
                                foreach (var item in wildGroups)
                                {
                                    if (linegroup.ToLower().Contains(item.ToLower().Replace("*", "")))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        if (wildGroups != null)
                            return result;
                    }
                }
            }
            return false;
        }

        private static string GetFirstPart(string text, string spliter = " ")
        {
            string? resultado = text.Trim();

            if (string.IsNullOrEmpty(text))
                return "";

            if (!text.Contains(spliter))
                return text;

            try
            {
                if (text.StartsWith("\""))
                    resultado = text.Substring(1, text.LastIndexOf(spliter));
                else
                    resultado = text.Substring(0, text.LastIndexOf(spliter));

                resultado = resultado.Trim();

                if (resultado.EndsWith(","))
                    resultado = resultado.Substring(0, resultado.Length - 1);
                if (resultado.EndsWith("\""))
                    resultado = resultado.Substring(0, resultado.Length - 1);

                return resultado;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

    }
}