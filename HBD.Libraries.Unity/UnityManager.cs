using System;
using System.Security.Principal;
using HBD.Framework.Configuration;
using HBD.Framework.Log;
using HBD.Libraries.Unity.ExtensionConfiguration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Threading.Tasks;

namespace HBD.Libraries.Unity
{
    public static class UnityManager
    {
        static readonly IUnityContainer _container;
        public static IUnityContainer Container
        {
            get { return _container; }
        }

        static UnityManager()
        {
            //_registedTypes = new Dictionary<string, Type>();
            _container = new UnityContainer();
            //Apply LogManager

            try
            { _container.LoadConfiguration(); }
            catch (ArgumentNullException)
            { LogManager.Write("There is no 'unity' configuration for 'IUnityContainer' in config file."); }
            catch (Exception ex)
            { LogManager.Write(ex); }

            try
            { LoadConfiguration(); }
            catch (ArgumentNullException)
            { LogManager.Write("There is no 'HBD.Libraries.Unity.AliasMappingSection configuration' for 'IUnityContainer' in config file."); }
            catch (Exception ex)
            { LogManager.Write(ex); }

            _container.AddNewExtension<Interception>();
        }

        private static void LoadConfiguration()
        {
            var section = ConfigurationManager.GetSection<AliasMappingSection>();

            if (section != null)
            {
                using (var impersonate = WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    Parallel.ForEach(section.AliasMapping, (item) =>
                     {
                         try
                         {
                             var inface = Framework.Core.AssemblyExtension.GetType(item.Interface);
                             var objClass = Framework.Core.AssemblyExtension.GetType(item.MapTo);

                             if (inface == null || objClass == null)
                                 LogManager.Write(string.Format("Cannot load alias interface '{0}' and mapping class '{1}'.", item.Interface, item.MapTo), LogManager.LogCategories.Error);
                             else if (!inface.IsInterface || !objClass.IsClass)
                                 LogManager.Write(string.Format("The alias interface '{0}' must be a Interface Instance and mapping class '{1}' must be a Class Instance.", item.Interface, item.MapTo), LogManager.LogCategories.Error);
                             else _container.RegisterResolveWithLogingInjection(inface, objClass, null);
                         }
                         catch (Exception ex)
                         { LogManager.Write(ex); }
                     });
                }
            }
        }
    }
}
