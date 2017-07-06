#region using

using System;
using System.IO;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.IO
{
    public static class FileEx
    {
        /// <summary>
        ///     Move file to destination
        /// </summary>
        /// <param name="file">FilePath</param>
        /// <param name="destination">Destination</param>
        /// <param name="newFileName">New File Name. The same with source name if empty.</param>
        public static void MoveTo(string file, string destination, string newFileName = null)
        {
            Guard.ArgumentIsNotNull(file, nameof(file));
            Guard.ArgumentIsNotNull(destination, nameof(destination));

            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (PathEx.IsDirectory(destination)) throw new ArgumentException(destination);

            if (newFileName.IsNullOrEmpty()) newFileName = Path.GetFileName(file);

            //Create folder if not existed.
            Directory.CreateDirectory(destination);
            //Move Files
            // ReSharper disable once AssignNullToNotNullAttribute
            File.Move(file, Path.Combine(destination, newFileName));
        }

        /// <summary>
        ///     Move all files to destination folder.
        ///     Destination File Name wil be the same with Source File.
        /// </summary>
        /// <param name="destination">Destination</param>
        /// <param name="files">files</param>
        public static void MoveTo(string destination, params string[] files)
        {
            foreach (var f in files)
                try
                {
                    MoveTo(f, destination);
                }
                catch
                {
                    // ignored
                }
        }

        /// <summary>
        ///     Move all files to destination folder.
        ///     Destination File Name will be [Source File Name]_[Current DateTime].file
        /// </summary>
        /// <param name="destination">Destination</param>
        /// <param name="files">File</param>
        public static void ArchiveFiles(string destination, params string[] files)
        {
            foreach (var f in files)
                try
                {
                    var desFileName =
                        $"{Path.GetFileName(f)}_{DateTime.Now:yyyy.MMM.dd hhmmss}.file";
                    MoveTo(desFileName, destination);
                }
                catch
                {
                    // ignored
                }
        }

        public static bool FileIsExists(this string @this)
            => File.Exists(@this);
    }
}