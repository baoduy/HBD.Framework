using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace HBD.Framework
{
    public static class ConfigurationExtentions
    {
        /// <summary>
        ///     Merge AppSettings
        /// </summary>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        public static void Merge(this NameValueCollection @this, KeyValueConfigurationCollection collection)
        {
            foreach (var a in collection.OfType<KeyValueConfigurationElement>())
                @this[a.Key] = a.Value;
        }

        /// <summary>
        ///     Merge ConnectionStrings.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        public static void Merge(this ConnectionStringSettingsCollection @this,
            ConnectionStringsSection collection)
        {
            var memberInfo =
                typeof(ConfigurationElementCollection).GetField("bReadOnly",
                    BindingFlags.Instance | BindingFlags.NonPublic);
            if (memberInfo != null)
                memberInfo.SetValue(@this, false);

            //ConnectionStrings.
            foreach (var a in collection.ConnectionStrings.OfType<ConnectionStringSettings>())
                @this.Add(new ConnectionStringSettings(a.Name, a.ConnectionString, a.ProviderName));

            if (memberInfo != null)
                memberInfo.SetValue(@this, true);
        }
    }
}