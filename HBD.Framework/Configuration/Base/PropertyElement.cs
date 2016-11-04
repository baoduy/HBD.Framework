using System.Configuration;

namespace HBD.Framework.Configuration.Base
{
    public class PropertyElement : ConfigurationElementBase
    {
        private const string _value = "value";

        [ConfigurationProperty(_value, IsKey = false)]
        public string Value => this[_value] as string;
    }
}