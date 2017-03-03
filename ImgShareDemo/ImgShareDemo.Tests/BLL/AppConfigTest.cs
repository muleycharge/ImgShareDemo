namespace ImgShareDemo.Tests.BLL
{
    using ImgShareDemo.BLL.Static;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    /// <summary>
    /// Not only test the AppConfig class but also make sure
    /// that this test library has access to all the needed
    /// constants to work properly
    /// </summary>
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
