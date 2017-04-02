#region using

using System;
using System.Web.Hosting;

#endregion

namespace HBD.Framework.Core
{
    public static class EnvironmentExtension
    {
        public static bool IsHosted => HostingEnvironment.IsHosted;
        public static bool IsJoinedDomain => !Environment.UserDomainName.EqualsIgnoreCase(Environment.MachineName);
    }
}