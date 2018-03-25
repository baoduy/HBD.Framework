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
            if (collection == null) return;

            foreach (var a in collection.OfType<KeyValueConfigurationElement>())
            {
                if(@this[a.Key]!=null)continue;
                @this[a.Key] = a.Value;
            }
        }

        /// <summary>
        ///     Merge ConnectionStrings.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        public static void Merge(this ConnectionStringSettingsCollection @this,
            ConnectionStringsSection collection)
        {
            if (collection == null) return;

            var memberInfo =
                typeof(ConfigurationElementCollection).GetField("bReadOnly",
                    BindingFlags.Instance | BindingFlags.NonPublic);
            if (memberInfo != null)
                memberInfo.SetValue(@this, false);

            //ConnectionStrings.
            foreach (var a in collection.ConnectionStrings.OfType<ConnectionStringSettings>())
            {
                if (@this[a.Name] != null) continue;
                @this.Add(new ConnectionStringSettings(a.Name, a.ConnectionString, a.ProviderName));
            }

            if (memberInfo != null)
                memberInfo.SetValue(@this, true);
        }
    }
}