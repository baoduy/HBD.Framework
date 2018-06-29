#region using

using System.IO;

#endregion

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
        ///     Delete folders and all subfolders and file.
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
                f.Delete();
        }

        /// <summary>
        /// Copy whole directort to new location.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="destination"></param>
        public static void CopyTo(this DirectoryInfo @this, string destination, bool overwrite = true)
        {
            Directory.CreateDirectory(destination);

            foreach (var f in @this.GetFiles())
                f.CopyTo(Path.Combine(destination, f.Name), overwrite);

            foreach (var d in @this.GetDirectories())
                d.CopyTo(Path.Combine(destination, d.Name), overwrite);
        }

        public static bool DirectoryIsExists(this string @this)
            => Directory.Exists(@this);
    }
}