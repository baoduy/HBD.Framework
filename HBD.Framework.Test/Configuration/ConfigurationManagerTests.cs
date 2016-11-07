using System;
using System.Collections.Specialized;
using System.Linq;
using HBD.Framework.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Framework.Test.Configuration
{
    [TestClass]
    public class ConfigurationManagerTests
    {
        [TestInitialize]
        [TestCategory("Fw.Config")]
        public void Initialiser()
        {
            System.Configuration.ConfigurationManager.AppSettings["TestIntValue"] = "123";
            System.Configuration.ConfigurationManager.AppSettings["TestTrueValue"] = "True";
            System.Configuration.ConfigurationManager.AppSettings["TestFalseValue"] = "False";
        }

        [TestMethod()]
        [TestCategory("Fw.Config")]
        public void GetDefaultCultureTest()
        {
            var culture = ConfigurationManager.GetDefaultCulture();
            Assert.IsNotNull(culture);
        }

        [TestMethod()]
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
            var sections = ConfigurationManager.GetSections<System.Configuration.UriSection>();
            Assert.IsNotNull(sections);
            Assert.IsTrue(sections.Any());
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionTest()
        {
            var section = ConfigurationManager.GetSection<System.Configuration.UriSection>();
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionWithNameTest()
        {
            var section = ConfigurationManager.GetSection<System.Configuration.UriSection>("uri");
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupsTest()
        {
            var sectionGroup = ConfigurationManager.GetSectionGroup<System.Runtime.Caching.Configuration.CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTest()
        {
            var sectionGroup = ConfigurationManager.GetSectionGroup<System.Runtime.Caching.Configuration.CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod()]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTestWithName()
        {
            var sectionGroup = ConfigurationManager.GetSectionGroup("system.runtime.caching");
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod()]
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