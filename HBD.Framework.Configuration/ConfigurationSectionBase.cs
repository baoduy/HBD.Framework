using System.Configuration;

namespace HBD.Framework.Configuration
{
    public abstract class ConfigurationSectionBase : ConfigurationSection
    {
        //const string _xmlns = "xmlns";

        //[ConfigurationProperty(_xmlns, IsRequired = false)]
        //protected string xmlns
        //{ get { return this[_xmlns] as string; } }

        public abstract string SectionName { get; }
    }
}
