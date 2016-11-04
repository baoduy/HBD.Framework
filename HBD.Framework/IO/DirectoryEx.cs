using System.IO;
using System.Runtime.CompilerServices;

namespace HBD.Framework.IO
{
    public static class DirectoryEx
    {
        public static void DeleteFiles(params string[] files)
        {
            foreach (var f in files)
            {
                if (!File.Exists(f)) continue;
                File.Delete(f);
            }
        }

        /// <summary>
        /// Delete folders and all subfolders and file.
        /// </summary>
        /// <param name="folders"></param>
        public static void DeleteDirectories(params string[] folders)
        {
            foreach (var f in folders)
            {
                if (!Directory.Exists(f)) continue;
                Directory.Delete(f, true);
            }
        }

        /// <summary>
        ///     Delete All Files inside Folder
        /// </summary>
        /// <param name="folder">Folder location</param>
        /// <param name="searchPattern"></param>
        public static void DeleteFiles(string folder, string searchPattern = null)
        {
            if (!Directory.Exists(folder)) return;
            DeleteFiles(Directory.GetFiles(folder, searchPattern ?? "*.*"));
        }

        /// <summary>
        ///     Delete Sub Folders inside Folder
        /// </summary>
        /// <param name="folder">Folder location</param>
        public static void DeleteSubDirectories(string folder)
        {
            if (!Directory.Exists(folder)) return;
            DeleteDirectories(Directory.GetDirectories(folder));
        }

        /// <summary>
        ///     Delete All Files and Sub Folders inside Folder
        /// </summary>
        /// <param name="rootDiractory">Folder location</param>
        public static void CleanupDirectory(string rootDiractory)
        {
            DeleteFiles(rootDiractory);
            DeleteSubDirectories(rootDiractory);
        }

        /// <summary>
        ///     Delete All Files inside Folder
        /// </summary>
        public static void DeleteFiles(this DirectoryInfo @this)
        {
            foreach (var f in @this.GetFiles())
            {
                if (!f.Exists) continue;
                f.Delete();
            }
        }

        public static void MoveAllFilesAndFoldersTo(this DirectoryInfo @this, string destinationDirectory)
        {
            Directory.CreateDirectory(destinationDirectory);

            foreach (var d in @this.GetDirectories())
                d.MoveTo(Path.Combine(destinationDirectory, d.Name));
            foreach (var f in @this.GetFiles())
                f.MoveTo(Path.Combine(destinationDirectory, f.Name));
            @this.Delete();
        }
    }
}