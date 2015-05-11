using System;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
namespace HBD.Framework.Plugin
{
    public interface IPluginContainer
    {
        void InitialPlugin();
        HBD.Framework.Plugin.Configuration.PluginSection Plugins { get; }
        IHBDViewBase GetWinFormPlugin(string pluginName);
        IHBDFeature GetFeaturePlugin(string pluginName);
    }
}
