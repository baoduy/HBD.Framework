#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

#endregion

namespace HBD.Framework.Core
{
    public static partial class UserPrincipalHelper
    {
        public static IEnumerable<string> Roles(this IPrincipal user) => ClaimsOfType(user, ClaimTypes.Role);

        public static IEnumerable<string> ClaimsOfType(this IPrincipal user, string claimType)
        {
            if (!(user.Identity is ClaimsIdentity)) return new string[0];
            return ((ClaimsIdentity)user.Identity).Claims
                .Where(c => c.Type.Equals(claimType))
                .Select(c => c.Value);
        }

        public static string ClaimOfType(this IPrincipal user, string claimType) => ClaimsOfType(user, claimType).FirstOrDefault();

        public static string Name(this IPrincipal user) => user.Identity.Name;

        public static string DisplayName(this IPrincipal user)
        {
            var surname = ClaimOfType(user, ClaimTypes.Surname);
            var givenName = ClaimOfType(user, ClaimTypes.GivenName);

            if (string.IsNullOrWhiteSpace(surname) && string.IsNullOrWhiteSpace(givenName))
                return GetUserNameWithoutDomain(Name(user));

            if (string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(givenName))
                return $"{givenName ?? string.Empty}{surname ?? string.Empty}";

            return $"{givenName} {surname}";
        }

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
    }
}