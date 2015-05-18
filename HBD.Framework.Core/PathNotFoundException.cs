using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HBD.Framework.Core
{
    public class PathNotFoundException : DirectoryNotFoundException
    {
        public PathNotFoundException() { }
        public PathNotFoundException(string path) : base(path) { }
    }
}
