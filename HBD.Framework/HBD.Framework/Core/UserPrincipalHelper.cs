#region

using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web;

#endregion

namespace HBD.Framework.Core
{
    public static class UserPrincipalHelper
    {
        public static WindowsIdentity User
         => EnvironmentExtension.IsHosted ? HttpContext.Current.User.Identity as WindowsIdentity : WindowsIdentity.GetCurrent();

        public static string UserName => User.Name;
        public static string UserNameWithoutDomain => GetUserNameWithoutDomain(User.Name);

        public static string GetUserNameWithoutDomain(string userName)
        {
            if (userName.IsNullOrEmpty()) return userName;
            var index = userName.LastIndexOf("\\", StringComparison.Ordinal);
            if (index > 0 && index < userName.Length) return userName.Substring(index + 1);

            index = userName.IndexOf("@", StringComparison.Ordinal);
            if (index > 0 && index < userName.Length)
                return userName.Substring(0, index);

            return userName;
        }

        /// <summary>
        /// Find User in Machine and Domain
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static UserPrincipal FindUser(string displayName)
            => FindUserInDomain(displayName) ?? FindUserInLocalMachine(displayName);

        public static UserPrincipal FindUserInLocalMachine(string displayName)
        {
            // set up Local context
            using (var ctx = new PrincipalContext(ContextType.Machine, Environment.MachineName))
                return UserPrincipal.FindByIdentity(ctx, displayName);
        }

        public static UserPrincipal FindUserInDomain(string displayName)
        {
            // set up Local context
            using (var ctx = new PrincipalContext(ContextType.Domain))
                return UserPrincipal.FindByIdentity(ctx, displayName);
        }

        public static bool IsUserExisted(string displayName)
            => FindUser(displayName) != null;
    }
}