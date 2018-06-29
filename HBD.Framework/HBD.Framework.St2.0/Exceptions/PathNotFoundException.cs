#region using

using System.IO;

#endregion

namespace HBD.Framework.Exceptions
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