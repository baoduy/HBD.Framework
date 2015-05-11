using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;

namespace HBD.Framework.Core
{
    public static class PathExtension
    {
        public static string GetFullPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                if (!path.StartsWith("~"))
                {
                    if (path.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                        path = string.Format("~/bin/{0}", path);
                    else path = string.Format("~/{0}", path);
                }
                return System.Web.HttpContext.Current.Server.MapPath(path);
            }
            else return Path.GetFullPath(path);
        }
        public static bool IsPathExisted(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (IsDirectory(path))
                return Directory.Exists(path);
            return File.Exists(path);
        }

        public static bool IsDirectory(string path)
        {
            return string.IsNullOrEmpty(Path.GetExtension(path));
        }
    }
}
