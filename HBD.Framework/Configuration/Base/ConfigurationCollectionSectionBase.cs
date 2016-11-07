using System.Configuration;

namespace HBD.Framework.Configuration.Base
{
    public class ConfigurationCollectionSectionBase<TElement> : ConfigurationSection
        where TElement : ConfigurationElement, new()
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConfigurationElement), AddItemName = "add", ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ConfigurationCollection<TElement> Elements => this[""] as ConfigurationCollection<TElement>;
    }
}