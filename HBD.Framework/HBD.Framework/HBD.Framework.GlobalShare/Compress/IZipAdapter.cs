
using System.Collections.Generic;

namespace HBD.Framework.Compress
{
    public interface IZipAdapter
    {
        /// <summary>
        /// Compress files.
        /// </summary>
        /// <param name="option"></param>
        /// <returns>The location of zip file.</returns>
        string Compress(ZipCompressOption option);

        /// <summary>
        /// Extract zip file.
        /// </summary>
        /// <param name="option"></param>
        /// <returns>The extracted files.</returns>
        IEnumerable<string> Extract(ZipExtractOption option);
    }
}
