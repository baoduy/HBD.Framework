using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Configuration;
using System.Configuration;

namespace HBD.Framework.Plugin.Configuration
{
    [ConfigurationCollection(typeof(PluginElementCollection),
      AddItemName = "add",
      ClearItemsName = "clear",
      RemoveItemName = "remove")]
    public class PluginElementGroup : ConfigurationElementBase
    {
        const string _icon = "icon";
        const string _plugins = "Plugins";

        [ConfigurationProperty(_icon, IsRequired = false)]
        public string Icon
        { get { return this[_icon] as string; } }

        [ConfigurationProperty(_plugins, IsRequired = false, IsDefaultCollection = true)]
        public PluginElementCollection Plugins
        { get { return this[_plugins] as PluginElementCollection; } }
    }
}
