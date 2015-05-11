using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Configuration;
using System.Configuration;

namespace HBD.Framework.Plugin.Configuration
{
    [ConfigurationCollection(typeof(PluginGroupCollection),
      AddItemName = "add",
      ClearItemsName = "clear",
      RemoveItemName = "remove")]
    public class PluginSection : ConfigurationSectionBase
    {
        const string _WinFormPlugin = "WinFormPlugin";
        const string _FeaturePlugin = "FeaturePlugin";

        public override string SectionName
        {
            get { return "HBD.Framework.PluginSection"; }
        }

        [ConfigurationProperty(_WinFormPlugin, IsRequired = false)]
        public PluginGroupCollection WinFormPlugin
        {
            get { return this[_WinFormPlugin] as PluginGroupCollection; }
        }

        [ConfigurationProperty(_FeaturePlugin, IsRequired = false)]
        public PluginGroupCollection FeaturePlugin
        {
            get { return this[_FeaturePlugin] as PluginGroupCollection; }
        }
    }
}
