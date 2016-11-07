using System.Web.Hosting;

namespace HBD.Framework.Core
{
    public static class EnvironmentExtension
    {
        public static bool IsHosted => HostingEnvironment.IsHosted;
    }
}