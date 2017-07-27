
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System.IO;

namespace HBD.Framework.Compress
{
    public static class Zip
    {
        public static ZipCompressOption Compress(params string[] files)
            => new ZipCompressOption(files);

        public static ZipExtractOption Extract(string zipFile)
           => new ZipExtractOption(zipFile);

        public static TOption WithAdapter<TOption>(this TOption @this, [NotNull]IZipAdapter adapter)
            where TOption : ZipOption
        {
            Guard.ArgumentIsNotNull(adapter, nameof(adapter));
            @this.ZipAdapter = adapter;

            return @this;
        }

        public static TOption WithPassword<TOption>(this TOption @this, [NotNull]string password)
             where TOption : ZipOption
        {
            Guard.ArgumentIsNotNull(password, nameof(password));
            @this.Password = password;

            return @this;
        }
    }
}
