#region

using System;
using System.IO;
using System.Web;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.IO
{
    public static class PathEx
    {
        public static string GetFullPath(string path)
        {
            if (!EnvironmentExtension.IsHosted) return Path.GetFullPath(path);
            if (path.StartsWith("~")) return HttpContext.Current.Server.MapPath(path);
            path = path.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) ? $"~/bin/{path}" : $"~/{path}";
            return HttpContext.Current.Server.MapPath(path);
        }

        public static bool IsPathExisted(string path)
        {
            if (path.IsNullOrEmpty()) return false;

            return IsDirectory(path) ? Directory.Exists(path) : File.Exists(path);
        }

        public static bool IsDirectory(string path) => string.IsNullOrEmpty(Path.GetExtension(path));
    }
}