using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzUserInfo
    {
        internal AzUserInfo( string scopeName, string userName)
        {
            ScopeName = scopeName;
            UserName = userName;
        }

        public string UserName { get; }
        public string ScopeName { get; }
        public string[] Groups { get; internal set; }
        public string[] Roles { get; internal set; }
        public string[] Operations { get; internal set; }
        public IReadOnlyList<AzUserInfo> UserScopeInfos { get; internal set; }

        public bool IsEmpty() => !Groups.Any() && !Roles.Any() && !Operations.Any();
    }
}
