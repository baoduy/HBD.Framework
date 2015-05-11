using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Plugin.Configuration;
using System.Reflection;
using System.IO;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.Libraries.Unity;
using HBD.Framework.Log;

namespace HBD.Framework.Plugin
{
    public static class PluginManager
    {
        private static IPluginContainer _pluginContainer = null;

        static PluginManager()
        {
            try
            {
                _pluginContainer = UnityManager.Container.RegisterResolveWithLogingInjection<IPluginContainer, PluginContainer>();
                _pluginContainer.InitialPlugin();
            }
            catch (Exception ex) 
            { LogManager.Write(ex); }
        }

        public static PluginSection Plugins
        {
            get
            {
                return _pluginContainer.Plugins;
            }
        }

        /// <summary>
        /// Get controls from WinFormPlugin. DO NOT DISPOSED THIS CONTROL.
        /// </summary>
        /// <param name="pluginName">pluginName</param>
        /// <returns>User Control</returns>
        public static IHBDViewBase GetWinFormPlugin(string pluginName)
        {
            return _pluginContainer.GetWinFormPlugin(pluginName);
        }

        public static IHBDFeature GetFeaturePlugin(string pluginName)
        {
            return _pluginContainer.GetFeaturePlugin(pluginName);
        }
    }
}
