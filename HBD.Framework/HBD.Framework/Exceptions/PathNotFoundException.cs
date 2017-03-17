#region

using System.IO;

#endregion

namespace HBD.Framework.Core.Exceptions
{
    public class PathNotFoundException : DirectoryNotFoundException
    {
        public PathNotFoundException()
        {
        }

        public PathNotFoundException(string path) : base(path)
        {
        }
    }
}