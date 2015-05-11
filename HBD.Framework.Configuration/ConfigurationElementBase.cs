using System.Configuration;

namespace HBD.Framework.Configuration
{
    public abstract class ConfigurationElementBase : ConfigurationElement
    {
        const string _name = "name";

        [ConfigurationProperty(_name, IsRequired = true, IsKey = true)]
        public string Name
        { get { return this[_name] as string; } }
    }
}
