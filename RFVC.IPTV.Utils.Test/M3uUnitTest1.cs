using RFVC.IPTV.M3u;

namespace RFVC.IPTV.Test
{
    [TestClass]
    public class M3uUnitTest1
    {

        [TestMethod]
        public void TestM3uDownloadAndListParser()
        {
            string testUrl = "https://iptv-org.github.io/iptv/countries/pt.m3u";
            string groups = "news";

            string filecontent = M3u.M3uHelper.DownloadAndFilterM3uList(testUrl, groups).Result;
            var lista = M3uHelper.GetM3UFileItems(filecontent);
            Assert.IsNotNull(lista);
            // Assert.AreEqual(100, lista.Count);
        }

        [TestMethod]
        public void TestM3uListParser()
        {

            string filecontent = GetDummyFileContent("portugal_teste.m3u8");
            var lista = M3uHelper.GetM3UFileItems(filecontent);
            Assert.IsNotNull(lista);
            Assert.AreEqual(100, lista.Count);
        }


        [TestMethod]
        public void TestLogoMissing()
        {
            string filecontent = GetDummyFileContent("portugal_logo_teste.m3u8");
            var lista = M3uHelper.GetM3UFileItems(filecontent);
            Assert.IsNotNull(lista);
            int noLogo = lista.Count((f) => string.IsNullOrEmpty(f.LogoLocation));
            M3uHelper.CompleteLogoInformation(lista);
            int noLogo2 = lista.Count((f) => string.IsNullOrEmpty(f.LogoLocation));

            Assert.AreNotEqual(noLogo, noLogo2);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestM3uListParserEmptyArgument()
        {
            M3uHelper.GetM3UFileItems("");
        }

        [TestMethod]
        public void TestM3uListParserWithMax()
        {

            string filecontent = GetDummyFileContent("portugal_teste.m3u8");
            var lista = M3uHelper.GetM3UFileItems(filecontent, 50);
            Assert.IsNotNull(lista);
            Assert.AreEqual(50, lista.Count);
        }

        [TestMethod]
        public void TestM3uListFilter()
        {
            List<string> groups = new List<string>() { "spor*", "kid" };
            string filecontent = GetDummyFileContent("portugal_teste.m3u8");
            var newContent = M3uHelper.FilterM3uFileByGroup(filecontent, groups);
            var lista = M3uHelper.GetM3UFileItems(newContent);
            Assert.IsNotNull(lista);
            Assert.AreEqual(16, lista.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestM3uListFilterEmptyArgument()
        {
            string filecontent = GetDummyFileContent("portugal_teste.m3u8");
            M3uHelper.FilterM3uFileByGroup(filecontent, null);
        }


        private string GetDummyFileContent(string name)
        {
            return System.IO.File.ReadAllText(name);
        }
    }
}