#if !NETSTANDARD2_0
#region using

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

#endregion

namespace HBD.Framework.Configuration
{
    /// <summary>
    ///     ConfigurationManager helps to working with Section in the config file easier than ever.
    /// </summary>
    public static partial class ConfigurationManager
    {
        public static CultureInfo GetDefaultCulture()
        {
            var cultureLanguageTag = CultureInfo.CurrentCulture.IetfLanguageTag;
            return CultureInfo.GetCultureInfoByIetfLanguageTag(cultureLanguageTag);
        }

        public static System.Configuration.Configuration OpenConfiguration()
        {
            return HostingEnvironment.IsHosted
                ? WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)
                : System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static IEnumerable<TSection> GetSections<TSection>() where TSection : ConfigurationSection
        {
            var config = OpenConfiguration();
            var sections = config?.Sections.Cast<ConfigurationSection>()
                .Where(section => section is TSection)
                .Cast<TSection>();
            return sections;
        }

        public static TSection GetSection<TSection>(string name = null) where TSection : ConfigurationSection
        {
            if (name.IsNotNullOrEmpty())
                return OpenConfiguration().GetSection(name) as TSection;

            return GetSections<TSection>().FirstOrDefault();
        }

        public static SmtpSection GetSmtpSection() => GetSection<SmtpSection>("system.net/mailSettings/smtp");

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

        /// <summary>
        ///     Get AppSetting value from either AppSettings or Environment.EnvironmentVariables and convert to T type.
        ///     Supporting Azure AppSettings from Environment.EnvironmentVariables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetAppSettingValue<T>(string key)
        {
            var value = System.Configuration.ConfigurationManager.AppSettings[key]
                        ?? Environment.GetEnvironmentVariable(key)
                        ?? Environment.GetEnvironmentVariable($"APPSETTING_{key}");
            return value.ConvertTo<T>();
        }

        /// <summary>
        ///     Get AppSetting value from either AppSettings or Environment.EnvironmentVariables and convert to T type.
        ///     Supporting Azure AppSettings from Environment.EnvironmentVariables
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the string value of key</returns>
        public static string GetAppSettingValue(string key)
        {
            return GetAppSettingValue<string>(key);
        }

        /// <summary>
        ///     Get ConnectionString value from either ConnectionStrings or Environment.EnvironmentVariables.
        ///     Supporting Azure AppSettings from Environment.EnvironmentVariables
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        /// <returns></returns>
        public static string GetConnectionString(string nameOrConnectionString)
        {
            var conn = System.Configuration.ConfigurationManager.ConnectionStrings[nameOrConnectionString]
                           ?.ConnectionString
                       ?? Environment.GetEnvironmentVariable($"SQLCONNSTR_{nameOrConnectionString}")
                       ?? Environment.GetEnvironmentVariable($"SQLAZURECONNSTR_{nameOrConnectionString}")
                       ?? Environment.GetEnvironmentVariable($"MYSQLCONNSTR_{nameOrConnectionString}")
                       ?? Environment.GetEnvironmentVariable($"CUSTOMCONNSTR_{nameOrConnectionString}");

            return conn.IsNullOrEmpty() ? nameOrConnectionString : conn;
        }


        /// <summary>
        ///     Merge AppSettings and ConnectionStrings to System.Configuration.ConfigurationManager.
        /// </summary>
        /// <param name="files"></param>
        public static void MergeConfigFrom(params string[] files)
        {
            foreach (var file in files)
            {
                if (!File.Exists(file)) continue;

                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = file };
                var config =
                    System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configMap,
                        ConfigurationUserLevel.None);

                //AppSettings
                System.Configuration.ConfigurationManager.AppSettings.Merge(config.AppSettings.Settings);

                //Allow to write to the ConnectionSring collection.
                System.Configuration.ConfigurationManager.ConnectionStrings.Merge(config.ConnectionStrings);
            }
        }
    }
}
#endif