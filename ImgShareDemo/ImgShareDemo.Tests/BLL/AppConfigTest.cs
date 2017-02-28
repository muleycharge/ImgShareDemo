namespace ImgShareDemo.Tests.BLL
{
    using ImgShareDemo.BLL.Static;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AppConfigTest
    {
        [TestMethod]
        public void TestMethodTestCorrectConfiguration()
        {
            string[] missingConfiguration;
            Assert.IsTrue(AppConfig.Validate(out missingConfiguration));
        }
    }
}
