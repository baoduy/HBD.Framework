#region using

using Microsoft.Win32.SafeHandles;

#endregion

namespace HBD.Framework.Security.Auths
{
    internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeTokenHandle() : base(true)
        {
        }

        protected override bool ReleaseHandle() => Impersonator.CloseHandle(handle);
    }
}