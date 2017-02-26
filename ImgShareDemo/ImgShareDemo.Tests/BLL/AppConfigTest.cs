using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImgShareDemo.BLL.Static;
using System.Configuration;
using System.Collections.Specialized;

namespace ImgShareDemo.Tests.BLL
{
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
