using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Plugin.Configuration;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.Libraries.Unity;
using Microsoft.Practices.Unity;
using System.IO;
using System.Reflection;

namespace HBD.Framework.Plugin
{
    public class PluginContainer : HBD.Framework.Plugin.IPluginContainer
    {
        const string HBDViewBase = "HBD.WinForms.Controls.Core.IHBDViewBase";
        const string HBDFeature = "HBD.Framework.Core.IHBDFeature";

        const string _wrongPlugin = "The plugin {0} is not inherited {1}";
        const string _pluginNotFound = "The plugin {0} is not found";

        PluginSection _pluginSection = null;
        public PluginSection Plugins
        {
            get
            {
                if (_pluginSection == null)
                    _pluginSection = HBD.Framework.Configuration.ConfigurationManager.GetSection<PluginSection>();
                return _pluginSection;
            }
        }

        public void InitialPlugin()
        {
            //WinForm Plugins
            LoadPlugins<IHBDViewBase>(Plugins.WinFormPlugin, HBDViewBase);

            //Feature plugins
            LoadPlugins<IHBDFeature>(Plugins.FeaturePlugin, HBDFeature);
        }

        private void LoadPlugins<IParentType>(PluginGroupCollection configurations, string notInheritMessage)
        {
            foreach (var g in configurations)
            {
                foreach (var i in g.Plugins)
                {
                    var type = LoadTypeFromAssemblyFile(i.Name, i.AssemblyFile);
                    if (type == null)
                        throw new ArgumentException(string.Format(_pluginNotFound, i.Name));
                    if (!typeof(IParentType).IsAssignableFrom(type))
                        throw new ArgumentException(string.Format(_wrongPlugin, i.Name, notInheritMessage));

                    if (UnityManager.Container.IsRegistered(type, i.Name))
                        continue;
                    
                    //UI controls will be apply ContainerControlledLifetimeManager
                    UnityManager.Container.RegisterType(typeof(IParentType), type, i.Name);
                }
            }
        }

        private Type LoadTypeFromAssemblyFile(string name, string fileName)
        {
            Guard.PathExisted(fileName);

            var fullPath = PathExtension.GetFullPath(fileName);
            var assemble = Assembly.LoadFile(fullPath);
            return assemble.GetType(name);
        }

        public IHBDViewBase GetWinFormPlugin(string pluginName)
        {
            return UnityManager.Container.RegisterResolve<IHBDViewBase>(pluginName);
        }

        public IHBDFeature GetFeaturePlugin(string pluginName)
        {
            return UnityManager.Container.RegisterResolve<IHBDFeature>(pluginName);
        }
    }
}
