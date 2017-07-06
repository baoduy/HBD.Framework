#region using

using System;
using System.Data;

#endregion

namespace HBD.Framework.Data.Base
{
    /// <summary>
    ///     The interface of FileData will convert data file to DataWrapper that can convert to DataTable.
    /// </summary>
    public interface IDataFileAdapter : IDisposable
    {
        string DocumentFile { get; }

#if !NETSTANDARD1_6
        /// <summary>
        ///     Read Data from file.
        /// </summary>
        /// <param name="firstRowIsColumnName"></param>
        /// <returns></returns>
        DataSet ReadData(bool firstRowIsColumnName = true);

        void WriteData(DataSet data, bool ignoreHeader = true);
#endif
    }
}