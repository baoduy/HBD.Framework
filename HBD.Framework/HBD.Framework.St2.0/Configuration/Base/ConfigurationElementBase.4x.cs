#if !NETSTANDARD2_0
#region using

using System.Configuration;

#endregion

namespace HBD.Framework.Configuration.Base
{
    
    public abstract class ConfigurationElementBase : ConfigurationElement
    {
        // ReSharper disable once InconsistentNaming
        private const string _name = "name";

        [ConfigurationProperty(_name, IsKey = true)]
        public string Name => this[_name] as string;
    }
}
#endif