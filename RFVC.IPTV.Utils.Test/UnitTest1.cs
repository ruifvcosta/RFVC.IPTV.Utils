namespace RFVC.IPTV.Utils.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestM3uListParser()
        {

            string filecontent = GetDummyFileContent("portugal_teste.m3u8");
            var lista = M3u.M3uHelper.GetM3UFileItems(filecontent);
            Assert.IsNotNull(lista);
            Assert.AreEqual(99, lista.Count);            
        }

        private string GetDummyFileContent(string name)
        {
           return System.IO.File.ReadAllText(name);
        }
    }
}