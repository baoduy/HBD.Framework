using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Configuration;
using System.Configuration;

namespace HBD.Framework.Plugin.Configuration
{
    public class PluginElement : ConfigurationElementBase
    {
        const string _assemblyFile = "assemblyFile";
        const string _title = "title";
        const string _icon = "icon";

        [ConfigurationProperty(_title, IsRequired = true)]
        public string Title
        { get { return this[_title] as string; } }

        [ConfigurationProperty(_icon, IsRequired = false)]
        public string Icon
        { get { return this[_icon] as string; } }

        [ConfigurationProperty(_assemblyFile, IsRequired = true)]
        public string AssemblyFile
        { get { return this[_assemblyFile] as string; } }
    }
}
