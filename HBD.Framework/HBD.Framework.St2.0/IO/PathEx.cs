#region using

using HBD.Framework.Extensions;
using System.IO;

#endregion

namespace HBD.Framework.IO
{
    public partial class PathEx
    {
        public static bool IsPathExisted(string path)
        {
            if (path.IsNullOrEmpty()) return false;

            return IsDirectory(path) ? Directory.Exists(path) : File.Exists(path);
        }

        public static bool IsDirectory(string path) => string.IsNullOrEmpty(Path.GetExtension(path));
    }
}