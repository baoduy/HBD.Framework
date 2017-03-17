#region

using System.Configuration;
using System.Linq;
using System.Runtime.Caching.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Configuration
{
    [TestClass]
    public class ConfigurationManagerTests
    {
        [TestInitialize]
        [TestCategory("Fw.Config")]
        public void Initialiser()
        {
            ConfigurationManager.AppSettings["TestIntValue"] = "123";
            ConfigurationManager.AppSettings["TestTrueValue"] = "True";
            ConfigurationManager.AppSettings["TestFalseValue"] = "False";
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetDefaultCultureTest()
        {
            var culture = Framework.Configuration.ConfigurationManager.GetDefaultCulture();
            Assert.IsNotNull(culture);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void OpenConfigurationTest()
        {
            var config = Framework.Configuration.ConfigurationManager.OpenConfiguration();
            Assert.IsNotNull(config);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionsTest()
        {
            var sections = Framework.Configuration.ConfigurationManager.GetSections<UriSection>();
            Assert.IsNotNull(sections);
            Assert.IsTrue(sections.Any());
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionTest()
        {
            var section = Framework.Configuration.ConfigurationManager.GetSection<UriSection>();
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionWithNameTest()
        {
            var section = Framework.Configuration.ConfigurationManager.GetSection<UriSection>("uri");
            Assert.IsNotNull(section);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupsTest()
        {
            var sectionGroup =
                Framework.Configuration.ConfigurationManager.GetSectionGroup<CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTest()
        {
            var sectionGroup =
                Framework.Configuration.ConfigurationManager.GetSectionGroup<CachingSectionGroup>();
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetSectionGroupTestWithName()
        {
            var sectionGroup = Framework.Configuration.ConfigurationManager.GetSectionGroup("system.runtime.caching");
            Assert.IsNotNull(sectionGroup);
        }

        [TestMethod]
        [TestCategory("Fw.Config")]
        public void GetAppSettingValueTest()
        {
            var value = Framework.Configuration.ConfigurationManager.GetAppSettingValue<int>("TestIntValue");
            Assert.IsTrue(value == 123);

            var obj = Framework.Configuration.ConfigurationManager.GetAppSettingValue<object>("123");
            Assert.IsNull(obj);

            var truebool = Framework.Configuration.ConfigurationManager.GetAppSettingValue<bool>("TestTrueValue");
            Assert.IsTrue(truebool);

            var falsebool = Framework.Configuration.ConfigurationManager.GetAppSettingValue<bool>("TestFalseValue");
            Assert.IsFalse(falsebool);
        }
    }
}