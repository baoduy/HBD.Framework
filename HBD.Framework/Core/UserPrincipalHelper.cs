using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web;

namespace HBD.Framework.Core
{
    public static class UserPrincipalHelper
    {
        public static string UserName => EnvironmentExtension.IsHosted ? HttpContext.Current.User.Identity.Name : WindowsIdentity.GetCurrent()?.Name;

        public static UserPrincipal GetUserByName(string displayName)
        {
            UserPrincipal user;

            // set up Local context
            using (var ctx = new PrincipalContext(ContextType.Machine))
                user = UserPrincipal.FindByIdentity(ctx, displayName);

            if (user != null) return user;

            // set up Domain context
            using (var ctx = new PrincipalContext(ContextType.Domain))
                user = UserPrincipal.FindByIdentity(ctx, displayName);

            return user;
        }
    }
}