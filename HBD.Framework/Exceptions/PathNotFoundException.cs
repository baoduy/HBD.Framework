using System.IO;

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