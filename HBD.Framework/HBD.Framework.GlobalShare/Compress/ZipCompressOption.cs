
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System.Collections.Generic;

namespace HBD.Framework.Compress
{
    public class ZipCompressOption : ZipOption
    {
        protected internal IList<string> Inputs { get; } = new List<string>();
        protected internal string OutputFile { get; private set; }
        protected internal ZipLevel Level { get; private set; } = Framework.Compress.ZipLevel.BestCompress;
        internal ZipFileOption FileOption { get; private set; } = ZipFileOption.Default;

        protected internal ZipCompressOption(params string[] filesOrDirectories)
        {
            Inputs.AddRange(filesOrDirectories);
        }

        public ZipCompressOption Include([NotNull]string fileOrDirectory)
        {
            Guard.ArgumentIsNotNull(fileOrDirectory, nameof(fileOrDirectory));

            if (!Inputs.Contains(fileOrDirectory))
                Inputs.Add(fileOrDirectory);

            return this;
        }

        public ZipCompressOption ZipLevel(ZipLevel level)
        {
            this.Level = level;
            return this;
        }

        public ZipCompressOption OverwriteIfExisted()
        {
            this.FileOption = ZipFileOption.OverwriteIfExisted;
            return this;
        }

        public ZipCompressOption AppendIfExisted()
        {
            this.FileOption = ZipFileOption.AppendIfExisted;
            return this;
        }

        /// <summary>
        /// Compress and store the zip file as the same folder with the first file or folder.
        /// </summary>
        /// <returns></returns>
        public virtual string Build()
            => GetOrCreateAdapter().Compress(this);

        /// <summary>
        /// Compress to a location.
        /// </summary>
        /// <param name="outputFile"></param>
        public virtual void BuildTo([NotNull]string outputFile)
        {
            Guard.ArgumentIsNotNull(outputFile, nameof(outputFile));
            OutputFile = outputFile;
            GetOrCreateAdapter().Compress(this);
        }
    }

    public enum ZipLevel
    {
        Storage = 0,
        Normal = 5,
        BestCompress = 9,
    }

    internal enum ZipFileOption
    {
        /// <summary>
        /// Default it will zip to the new file.
        /// The InvalidOperationException will be thrown if the file is existed.
        /// </summary>
        Default = 0,

        /// <summary>
        /// If the destination file is existed then overwrite that file.
        /// </summary>
        OverwriteIfExisted = 1,

        /// <summary>
        /// Append if the file is existed.
        /// </summary>
        AppendIfExisted = 2,
    }
}
