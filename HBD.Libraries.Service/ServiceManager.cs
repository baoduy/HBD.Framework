using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Libraries.Unity;
using System.ServiceModel.Configuration;
using System.ServiceModel;
using HBD.Framework.Log;
using Microsoft.Practices.Unity;

namespace HBD.Libraries.Service
{
    public static class ServiceManager
    {
        public static TInterface GetServiceInstance<TInterface>()
        {
            //The mapping instance of TInterface of _container is loading from Configuration file.
            //1. If the instance of TInterface was registered on _container then get instance from _container.
            //2. If not registered the try to get from Service.

            try
            {
                if (UnityManager.Container.IsRegistered<TInterface>())
                    return UnityManager.Container.Resolve<TInterface>();

                //In the configuration the Service ne must be identtical with Interface Name.
                var serviceName = typeof(TInterface).FullName;

                //Get Web configuration
                var configFile = HBD.Framework.Configuration.ConfigurationManager.OpenConfiguration();
                var serviceSection = ServiceModelSectionGroup.GetSectionGroup(configFile);

                if (serviceSection != null)
                {
                    var channelEndpointElement = serviceSection.Client.Endpoints.Cast<ChannelEndpointElement>().SingleOrDefault(c => c.Contract == serviceName);
                    if (channelEndpointElement != null)
                    {
                        var endpointAddress = new EndpointAddress(channelEndpointElement.Address.AbsoluteUri);
                        return new ConfigurationChannelFactory<TInterface>(channelEndpointElement.Name, configFile, endpointAddress).CreateChannel();
                    }
                }
            }
            catch (Exception ex)
            { LogManager.Write(ex); }

            return default(TInterface);
        }
    }
}
