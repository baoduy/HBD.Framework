using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace HBD.Framework.Compress
{
    internal static class InternalExtentions
    {
        public static void AddDirectory(this ZipFile zip, string directory, int folderOffset)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            var files = Directory.GetFiles(directory);
            var folders = Directory.GetDirectories(directory);

            //If directory is empty then just add the Entry in
            if (files.NotAny() && folders.NotAny())
            {
                var name = directory.Substring(folderOffset);
                zip.AddDirectory(name);
                return;
            }

            foreach (var f in files)
            {
                var name = folderOffset <= 0 ? Path.GetFileName(f) : f.Substring(folderOffset);
                zip.Add(f, name);
            }

            foreach (var d in folders)
                zip.AddDirectory(d, folderOffset);
        }
    }
}
