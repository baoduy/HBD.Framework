#region using

using System;
using System.Linq;

#endregion

namespace HBD.Framework.Core
{
    public static class EnvironmentExtension
    {
#if NETSTANDARD2_0 || NETSTANDARD1_6
        public static bool IsHosted =>System.IO.Directory.GetFiles(System.AppContext.BaseDirectory,"Web.config").Any() ;
#else
        public static bool IsHosted => System.Web.Hosting.HostingEnvironment.IsHosted;
#endif

#if !NETSTANDARD1_6
        public static bool IsJoinedDomain => !Environment.UserDomainName.EqualsIgnoreCase(Environment.MachineName);
#endif
    }
}