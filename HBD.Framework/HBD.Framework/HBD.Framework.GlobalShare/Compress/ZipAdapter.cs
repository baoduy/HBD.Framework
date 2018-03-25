

using HBD.Framework.IO;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HBD.Framework.Compress
{
    internal class ZipAdapter : IZipAdapter
    {
        private const string Ext = ".zip";

        private static void Validation(ZipCompressOption option)
        {
            if (!option.Inputs.Any())
                throw new ArgumentException("The input files cannot be empty.");
        }

        private string GetOutputFileName(ZipCompressOption option)
        {
            var output = string.Empty;

            if (option.OutputFile.IsNotNullOrEmpty())
                return Path.GetFullPath(option.OutputFile);

            var f = Path.GetFullPath(option.Inputs.First());

            if (PathEx.IsDirectory(f))
                return $"{Path.GetDirectoryName(f)}\\{new DirectoryInfo(f).Name}{Ext}";
            return Path.ChangeExtension(f, Ext);
        }

        public string Compress(ZipCompressOption option)
        {
            Validation(option);

            var outputFile = GetOutputFileName(option);

            if (File.Exists(outputFile) && option.FileOption == ZipFileOption.Default)
                throw new InvalidOperationException($"File {outputFile} is existed.");

            if (option.FileOption == ZipFileOption.OverwriteIfExisted)
                File.Delete(outputFile);

            using (var zip = option.FileOption == ZipFileOption.AppendIfExisted
                ? new ZipFile(outputFile) : ZipFile.Create(outputFile))
            {
                // Must call BeginUpdate to start, and CommitUpdate at the end.
                zip.BeginUpdate();

                if (option.Password.IsNotNullOrEmpty())
                    zip.Password = option.Password;

                foreach (var ip in option.Inputs)
                {
                    var full = Path.GetFullPath(ip);

                    if (PathEx.IsDirectory(full))
                    {
                        var folderOffset = Path.GetDirectoryName(full).Length + 1;
                        zip.AddDirectory(full, folderOffset);
                    }
                    // The "Add()" method will add or overwrite as necessary.
                    // When the optional entryName parameter is omitted, the entry will be named
                    else zip.Add(full, Path.GetFileName(full));
                }

                // Both CommitUpdate and Close must be called.
                zip.CommitUpdate();
                zip.Close();
            }

            return outputFile;
        }

        private string GetOutputFolder(ZipExtractOption option)
            => Path.GetFullPath(option.OutputFolder ?? Path.GetDirectoryName(option.ZipFile));

        public IEnumerable<string> Extract(ZipExtractOption option)
        {
            if (!File.Exists(option.ZipFile))
                throw new FileNotFoundException(option.ZipFile);

            var outputFolder = GetOutputFolder(option);
            Directory.CreateDirectory(outputFolder);

            using (var s = new ZipInputStream(File.OpenRead(option.ZipFile)))
            {
                if (option.Password.IsNotNullOrEmpty())
                    s.Password = option.Password;

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (theEntry.IsDirectory)
                    {
                        var dir = Path.Combine(outputFolder, theEntry.Name);
                        Directory.CreateDirectory(dir);
                        yield return dir;
                        continue;
                    }

                    var fileName = Path.Combine(outputFolder, theEntry.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                    if (File.Exists(fileName))
                    {
                        if (!option.IsOverwriteIfExisted)
                            throw new InvalidOperationException($"File {fileName} is already exisited.");
                        else File.Delete(fileName);
                    }

                    using (var streamWriter = File.Create(fileName))
                    {
                        var data = new byte[2048];
                        while (true)
                        {
                            var size = s.Read(data, 0, data.Length);
                            if (size <= 0) break;

                            streamWriter.Write(data, 0, size);
                        }
                    }
                    yield return fileName;
                }
            }
        }
    }
}
