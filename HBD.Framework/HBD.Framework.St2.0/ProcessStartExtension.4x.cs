#region using

using System.Diagnostics;

#endregion

namespace HBD.Framework
{
    #if !NETSTANDARD2_0
    public static class ProcessStartExtension
    {
        public static string Run(this ProcessStartInfo @this)
        {
            @this.RedirectStandardOutput = true;

            using (var process = Process.Start(@this))
            {
                if (process == null) return string.Empty;

                var str = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();

                return str;
            }
        }
    }
    #endif
}