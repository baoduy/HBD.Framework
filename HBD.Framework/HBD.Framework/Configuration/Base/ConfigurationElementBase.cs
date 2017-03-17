#region

using System.Configuration;

#endregion

namespace HBD.Framework.Configuration.Base
{
    public abstract class ConfigurationElementBase : ConfigurationElement
    {
        private const string _name = "name";

        [ConfigurationProperty(_name, IsKey = true)]
        public string Name => this[_name] as string;
    }
}