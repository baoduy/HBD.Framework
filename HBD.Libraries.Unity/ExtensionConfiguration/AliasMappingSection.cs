using System.Configuration;
using HBD.Framework.Configuration;

namespace HBD.Libraries.Unity.ExtensionConfiguration
{
    public class AliasMappingSection : ConfigurationSectionBase
    {
        public override string SectionName
        {
            get { return "HBD.Libraries.Unity.AliasMappingSection"; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(AliasMappingCollection), AddItemName = "alias", ClearItemsName = "clear", RemoveItemName = "remove")]
        public AliasMappingCollection AliasMapping
        {
            get { return this[""] as AliasMappingCollection; }
        }
    }
}
