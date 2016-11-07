using Microsoft.Win32.SafeHandles;

namespace HBD.Framework.Security.Auths
{
    internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeTokenHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle() => Impersonator.CloseHandle(handle);
    }
}