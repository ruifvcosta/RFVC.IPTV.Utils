using RFVC.IPTV.M3u;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RFVC.IPTV.M3u.M3uHelper;

namespace RFVC.IPTV.Guide
{
    public static class GuideHelper
    {



        public static IList<TvGuideItem>? GetTvGuideItems(Guide guideContent)
        {
            if (guideContent == null)
                return null;

            var result = new List<TvGuideItem>();

            foreach (var prog in guideContent.programme)
            {
                var newItem = new TvGuideItem(prog.channel,
                       prog.title,
                       prog.desc,
                       DateTime.ParseExact(prog.start, "yyyyMMddHHmmss zzz", null),
                       DateTime.ParseExact(prog.stop, "yyyyMMddHHmmss zzz", null));
                if (result.Where(f => f.Channel == newItem.Channel && f.Stop == newItem.Stop).Count() == 0)
                    result.Add(newItem);
            }
            return result;
        }

        public static IList<TvGuideItem>? GetTvGuideItemsForM3uFileItems(Guide guideContent, 
                                                            IList<M3uFileItem> fileItems)
        {
            if(fileItems == null)
                throw new ArgumentNullException(nameof(fileItems));

            var result = new List<TvGuideItem>();

            foreach (var item in fileItems.Where(f => f.Type == (int)FileItemType.Tv && !string.IsNullOrEmpty(f.GuideID)))
            {
                var programs = guideContent.programme.Where(f => f.channel == item.GuideID && DateTime.ParseExact(f.stop, "yyyyMMddHHmmss zzz", null) >= DateTime.Now).OrderBy(f => f.start).ToList();
                if (programs != null)
                    if (programs.Count() > 0)
                        foreach (var prog in programs)
                        {
                            var newItem = new TvGuideItem(prog.channel,
                           prog.title,
                            prog.desc,
                            DateTime.ParseExact(prog.start, "yyyyMMddHHmmss zzz", null),
                            DateTime.ParseExact(prog.stop, "yyyyMMddHHmmss zzz", null));
                            if (result.Where(f => f.Channel == newItem.Channel && f.Stop == newItem.Stop).Count() == 0)
                            {
                                result.Add(newItem);
                            }
                        }
            }
            return result;
        }

       
    }
}
