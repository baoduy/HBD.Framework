#region using

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

#endregion

namespace HBD.Framework.Configuration
{
    /// <summary>
    ///     ConfigurationManager helps to working with Section in the config file easier than ever.
    /// </summary>
    public static class ConfigurationManager
    {
        public static CultureInfo GetDefaultCulture()
        {
            var cultureLanguageTag = CultureInfo.CurrentCulture.IetfLanguageTag;
            return CultureInfo.GetCultureInfoByIetfLanguageTag(cultureLanguageTag);
        }

        public static System.Configuration.Configuration OpenConfiguration()
            => HostingEnvironment.IsHosted
                ? WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
                : System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static IEnumerable<TSection> GetSections<TSection>() where TSection : ConfigurationSection
        {
            var config = OpenConfiguration();
            var sections = config?.Sections.Cast<ConfigurationSection>()
                .Where(section => section is TSection)
                .Cast<TSection>();
            return sections;
        }

        public static TSection GetSection<TSection>(string name = null) where TSection : ConfigurationSection
            => name.IsNullOrEmpty()
                ? GetSections<TSection>().FirstOrDefault()
                : GetSections<TSection>()
                    .FirstOrDefault(
                        s => string.Equals(s.SectionInformation.Name, name, StringComparison.CurrentCultureIgnoreCase));

        public static TSectionGroup GetSectionGroup<TSectionGroup>()
            where TSectionGroup : ConfigurationSectionGroup, new()
        {
            var config = OpenConfiguration();
            return config?.SectionGroups.Cast<ConfigurationSectionGroup>()
                .Where(s => s is TSectionGroup)
                .OfType<TSectionGroup>().FirstOrDefault();
        }

        public static ConfigurationSectionGroup GetSectionGroup(string groupName)
        {
            var config = OpenConfiguration();
            return config?.GetSectionGroup(groupName);
        }

        public static T GetAppSettingValue<T>(string name)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[name];

            if (string.IsNullOrEmpty(value))
                return default(T);

            var type = typeof(T);

            var obj = type == typeof(bool)
                ? bool.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase)
                : Convert.ChangeType(value, type);

            return (T) obj;
        }

        public static string GetConnectionString(string nameOrConnectionString)
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings[nameOrConnectionString];
            return conn?.ConnectionString ?? nameOrConnectionString;
            //return ConsolidateConnectionString(conn?.ConnectionString ?? nameOrConnectionString);
        }
    }
}