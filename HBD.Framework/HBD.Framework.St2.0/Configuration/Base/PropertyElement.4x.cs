
#if !NETSTANDARD2_0
#region using

using System.Configuration;

#endregion

namespace HBD.Framework.Configuration.Base
{
    public class PropertyElement : ConfigurationElementBase
    {
        // ReSharper disable once InconsistentNaming
        private const string _value = "value";

        [ConfigurationProperty(_value, IsKey = false)]
        public string Value => this[_value] as string;
    }
}
#endif