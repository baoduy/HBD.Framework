using System;
using System.Collections.Generic;
using System.Text;

namespace HBD.Framework.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// Remove Domain name from Username follow these format Domain\username or username@domain.name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string WithoutDomain(this string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return userName;

            var slashIndex = userName.IndexOf('\\');
            if (slashIndex >= 0) return userName.Substring(slashIndex + 1);

            var atIndex = userName.IndexOf('@');
            if (atIndex >= 0) return userName.Substring(0, atIndex);

            return userName;
        }
    }
}
