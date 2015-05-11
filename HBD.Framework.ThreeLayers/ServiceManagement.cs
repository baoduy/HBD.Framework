using HBD.Libraries.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HBD.Framework.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace HBD.Framework.ThreeLayers
{
    /// <summary>
    /// Load Biz, DAO instace from Unity, WebService or Create Directly.
    /// </summary>
    public static class ServiceManagement
    {
        public static TInterface GetInstance<TInterface>()
        {
            //The mapping instance of TInterface of _container is loading from Configuration file.
            //1. If the instance of TInterface was registered on _container then get instance from _container.
            //2. If not registered the try to get from Service.
            //3. Try to create instace directly.

            try
            {
                //1.
                if (UnityManager.Container.IsRegistered<TInterface>())
                    return UnityManager.Container.Resolve<TInterface>();

                //In the configuration the Service ne must be identtical with Interface Name.
                var type = typeof(TInterface);
                var serviceName = type.FullName;

                //2.
                var configFile = ConfigurationManager.OpenConfiguration();
                var serviceSection = ServiceModelSectionGroup.GetSectionGroup(configFile);

                if (serviceSection != null)
                {
                    var channelEndpointElement = serviceSection.Client.Endpoints.Cast<ChannelEndpointElement>().SingleOrDefault(c => c.Contract == serviceName);
                    if (channelEndpointElement != null)
                    {
                        var endpointAddress = new EndpointAddress(channelEndpointElement.Address.AbsoluteUri);
                        var serviceInstance = new ConfigurationChannelFactory<TInterface>(channelEndpointElement.Name, configFile, endpointAddress).CreateChannel();
                        return serviceInstance;
                    }
                }

                //3.
                if (typeof(TInterface).IsClass)
                    return Activator.CreateInstance<TInterface>();
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                Log.LogManager.Write(ex);
#endif
            }

            return default(TInterface);
        }
    }
}
