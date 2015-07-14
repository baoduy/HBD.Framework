using System.Configuration;
using HBD.Framework.Configuration;

namespace HBD.Libraries.Unity.ExtensionConfiguration
{
    public class AliasMappingElement : ConfigurationElementBase
    {
        private const string _Interface = "interface";
        private const string _mapTo = "mapTo";

        [ConfigurationProperty(_Interface, IsRequired = true)]
        public string Interface 
        { get { return this[_Interface] as string; } }

        [ConfigurationProperty(_mapTo, IsRequired = true)]
        public string MapTo
        { get { return this[_mapTo] as string; } }
    }
}
