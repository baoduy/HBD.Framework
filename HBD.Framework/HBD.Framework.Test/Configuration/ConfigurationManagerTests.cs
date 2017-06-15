#region using

using System.Configuration;
using System.Linq;
using System.Runtime.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Configuration.Tests
{
    [TestClass]
    public class ConfigurationManagerTests
    {
        [TestMethod]
        public void MergeConfigFromTest()
        {
            ConfigurationManager.MergeConfigFrom("TestData\\Web1.config", "TestData\\Web2.config",
                "TestData\\Web4.config");

            Assert.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["A"]);
            Assert.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["B"]);
            Assert.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["C"]);
            Assert.IsNotNull(System.Configuration.ConfigurationManager.AppSettings["D"]);
            Assert.IsTrue(System.Configuration.ConfigurationManager.AppSettings.Count >= 8);

            Assert.IsNotNull(System.Configuration.ConfigurationManager.ConnectionStrings["A"].ConnectionString);
            Assert.IsNotNull(System.Configuration.ConfigurationManager.ConnectionStrings["B"].ConnectionString);
            Assert.IsTrue(System.Configuration.ConfigurationManager.ConnectionStrings.Count >= 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void MergeConfigFrom_Exception_Test()
        {
            ConfigurationManager.MergeConfigFrom("TestData\\Web3.config");
        }

        [TestInitialize]
        [TestCategory("Fw.Config")]
        public void Initialiser()
        {
            System.Configuration.ConfigurationManager.AppSettings["TestIntValue"] = "123";
            System.Configuration.ConfigurationManager.AppSettings["TestTrueValue"] = "True";
            System.Configuration.ConfigurationManager.AppSettings["TestFalseValue"] = "False";
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetDefaultCultureTest()
        {
            var culture = ConfigurationManager.GetDefaultCulture();
            Assert.IsNotNull(culture);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void OpenConfigurationTest()
        {
            var config = ConfigurationManager.OpenConfiguration();
            Assert.IsNotNull(config);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionsTest()
        {
            var sections = ConfigurationManager.GetSections<UriSection>();
            Assert.IsNotNull(sections);
            Assert.IsTrue(sections.Any());
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionTest()
        {
            var section = ConfigurationManager.GetSection<UriSection>();
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionWithNameTest()
        {
            var section = ConfigurationManager.GetSection<UriSection>("uri");
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupsTest()
        {
            var sectionGroup =
                ConfigurationManager.GetSectionGroup<CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTest()
        {
            var sectionGroup =
                ConfigurationManager.GetSectionGroup<CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTestWithName()
        {
            var sectionGroup = ConfigurationManager.GetSectionGroup("system.runtime.caching");
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetAppSettingValueTest()
        {
            var value = ConfigurationManager.GetAppSettingValue<int>("TestIntValue");
            Assert.IsTrue(value == 123);

            var obj = ConfigurationManager.GetAppSettingValue<object>("123");
            Assert.IsNull(obj);

            var truebool = ConfigurationManager.GetAppSettingValue<bool>("TestTrueValue");
            Assert.IsTrue(truebool);

            var falsebool = ConfigurationManager.GetAppSettingValue<bool>("TestFalseValue");
            Assert.IsFalse(falsebool);
        }
    }
}