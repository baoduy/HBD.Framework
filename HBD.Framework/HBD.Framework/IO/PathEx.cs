using System;
using System.IO;
using System.Web;
using HBD.Framework.Core;

namespace HBD.Framework.IO
{
    public partial class PathEx
    {
        public static string GetFullPath(string path)
        {
            if (!EnvironmentExtension.IsHosted) return Path.GetFullPath(path);
            if (path.StartsWith("~")) return HttpContext.Current.Server.MapPath(path);
            path = path.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) ? $"~/bin/{path}" : $"~/{path}";
            return HttpContext.Current.Server.MapPath(path);
        }
    }
}
