#region using

using HBD.Framework.Security.Services;

#endregion

namespace HBD.Framework.Security
{
    public static class CryptionManager
    {
        //Default Password
        private const string DefaultPassword = "287DE7A3-630B-42D4-8C02-A6F89C694CD7";

        public static ICryptionService Default { get; } = new CryptionService(DefaultPassword);
    }
}