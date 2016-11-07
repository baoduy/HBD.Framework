using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace HBD.Framework.Security.Auths
{
    public class Impersonator : IDisposable
    {
        private readonly string _domain;
        private readonly string _password;
        private readonly string _userName;
        private WindowsIdentity _identity;
        private SafeTokenHandle _token;

        public Impersonator(string domainName, string userName, string password)
        {
            _userName = userName;
            _password = password;
            _domain = domainName;
        }

        private SafeTokenHandle Token
        {
            get
            {
                if (_token != null) return _token;
                //if (!RevertToSelf()) throw new Win32Exception(Marshal.GetLastWin32Error());

                if (
                    LogonUser(_userName, _domain, _password, (int)LogonTypes.Interactive, (int)LogonProviders.Default,
                        out _token) != 0)
                    return _token;
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        private WindowsIdentity Identity
        {
            get
            {
                if (_identity == null)
                    _identity = new WindowsIdentity(Token.DangerousGetHandle());
                return _identity;
            }
        }

        public void Dispose()
        {
            _identity?.Dispose();
            _token.Dispose();
        }

        public WindowsImpersonationContext Impersonate()
        {
            if (Token == null) throw new Win32Exception(Marshal.GetLastWin32Error());
            return Identity.Impersonate();
        }

        #region Extern Methods

        [DllImport("advapi32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        internal static extern int LogonUser(string userName, string domain, string password, int logonType,
            int logonProvider, out SafeTokenHandle token);

        //[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        //[SuppressUnmanagedCodeSecurity]
        //internal static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //internal static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);

        #endregion Extern Methods
    }
}