
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System.Linq;

namespace HBD.Framework.Compress
{
    public class ZipExtractOption : ZipOption
    {
        internal ZipExtractOption(string zipFile) => ZipFile = zipFile;

        internal protected string OutputFolder { get; private set; }

        internal protected string ZipFile { get; }

        internal bool IsOverwriteIfExisted { get; private set; } = false;

        public ZipExtractOption OverwriteFilesIfExisted()
        {
            IsOverwriteIfExisted = true;
            return this;
        }

        /// <summary>
        /// Extect to the same folder of zip file.
        /// </summary>
        /// <returns></returns>
        public string[] ToTheSameFolder()
        {
            return GetOrCreateAdapter().Extract(this).ToArray();
        }

        /// <summary>
        /// Extract to percifict folder.
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        public string[] To([NotNull]string outputFolder)
        {
            Guard.ArgumentIsNotNull(outputFolder, nameof(outputFolder));
            OutputFolder = outputFolder;

            return GetOrCreateAdapter().Extract(this).ToArray();
        }
    }
}
