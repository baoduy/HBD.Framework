using System;
using System.Configuration;
using System.Globalization;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace HBD.Framework.Configuration
{
    public static class ConfigurationManager
    {
        public static CultureInfo GetDefaultCulture()
        {
            var cultureLanguageTag = CultureInfo.CurrentCulture.IetfLanguageTag;
            return CultureInfo.GetCultureInfoByIetfLanguageTag(cultureLanguageTag);
        }

        public static System.Configuration.Configuration OpenConfiguration()
        {
            if (HostingEnvironment.IsHosted)
                return WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            else
                return System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static TSection GetSection<TSection>() where TSection : ConfigurationSectionBase, new()
        {
            var section = System.Configuration.ConfigurationManager.GetSection(new TSection().SectionName);
            return section as TSection ?? default(TSection);
        }

        public static TSectionGroup GetSectionGroup<TSectionGroup>() where TSectionGroup : ConfigurationSectionGroup, new()
        {
            var config = OpenConfiguration();
            if (config != null)
            {
                if (typeof(TSectionGroup) == typeof(ServiceModelSectionGroup))
                    return ServiceModelSectionGroup.GetSectionGroup(config) as TSectionGroup;

                var section = config.GetSectionGroup(new TSectionGroup().SectionGroupName);
                if (section != null)
                    return (TSectionGroup)section;
            }
            return default(TSectionGroup);
        }

        public static ConfigurationSectionGroup GetSectionGroup(string groupName)
        {
            var config = OpenConfiguration();
            if (config != null)
                return config.GetSectionGroup(groupName);
            return null;
        }

        public static T GetAppSettingValue<T>(string name)
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[name];

            if (string.IsNullOrEmpty(value))
                return default(T);

            var type = typeof(T);

            object obj = null;
            if (type == typeof(bool))
                obj = bool.TrueString.Equals(value, StringComparison.CurrentCultureIgnoreCase);
            else obj = Convert.ChangeType(value, type);

            return (T)obj;
        }
    }
}
