using HBD.Framework.Core;
using System;
using System.Web;

namespace HBD.Framework.IO
{
    public static class PathEx
    {
        public static string GetFullPath(string path)
        {
            if (!EnvironmentExtension.IsHosted) return System.IO.Path.GetFullPath(path);
            if (path.StartsWith("~")) return HttpContext.Current.Server.MapPath(path);
            path = path.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) ? $"~/bin/{path}" : $"~/{path}";
            return HttpContext.Current.Server.MapPath(path);
        }

        public static bool IsPathExisted(string path)
        {
            if (path.IsNullOrEmpty()) return false;

            return IsDirectory(path) ? System.IO.Directory.Exists(path) : System.IO.File.Exists(path);
        }

        public static bool IsDirectory(string path) => string.IsNullOrEmpty(System.IO.Path.GetExtension(path));
    }
}