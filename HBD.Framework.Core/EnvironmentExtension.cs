using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Core
{
    public static class EnvironmentExtension
    {
        public static bool IsHosted { get { return System.Web.Hosting.HostingEnvironment.IsHosted; } }
    }
}
