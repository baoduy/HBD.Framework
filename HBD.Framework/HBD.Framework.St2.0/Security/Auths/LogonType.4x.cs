#if !NETSTANDARD2_0
namespace HBD.Framework.Security.Auths
{
    public enum LogonTypes
    {
        Interactive = 2,
        NewCredentials = 9
    }

    public enum LogonProviders
    {
        Default = 0,
        Winnt50 = 3
    }
}
#endif