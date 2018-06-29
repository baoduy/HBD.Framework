#if !NETSTANDARD2_0
#region using

using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web;

#endregion

namespace HBD.Framework.Core
{
    public static partial class UserPrincipalHelper
    {
        public static WindowsIdentity User
            =>
                EnvironmentExtension.IsHosted
                    ? HttpContext.Current.User.Identity as WindowsIdentity
                    : WindowsIdentity.GetCurrent();

        public static string UserName => User.Name;
        public static string UserNameWithoutDomain => GetUserNameWithoutDomain(User.Name);



        /// <summary>
        ///     Find User in Machine and Domain
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static UserPrincipal FindUser(string displayName)
            => FindUserInDomain(displayName) ?? FindUserInLocalMachine(displayName);

        public static UserPrincipal FindUserInLocalMachine(string displayName)
        {
            // set up Local context
            using (var ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName))
            {
                return UserPrincipal.FindByIdentity(ctx, displayName);
            }
        }

        public static UserPrincipal FindUserInDomain(string displayName)
        {
            // set up Local context
            using (var ctx = new PrincipalContext(ContextType.Domain))
            {
                return UserPrincipal.FindByIdentity(ctx, displayName);
            }
        }

        public static bool IsUserExisted(string displayName)
            => FindUser(displayName) != null;
    }
}
#endif