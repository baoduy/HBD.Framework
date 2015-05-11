using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Core
{
    public static class IdentityExtension
    {
        public static string Name
        {
            get
            {
                if (EnvironmentExtension.IsHosted)
                    return System.Web.HttpContext.Current.User.Identity.Name;
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        } }
    }
}
