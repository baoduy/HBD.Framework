#region using

using HBD.Framework.Data.GetSetters;
using System;

#endregion

namespace HBD.Framework.Data.Base
{
    /// <summary>
    ///     The interface of FileData will convert data file to DataWrapper that can convert to DataTable.
    /// </summary>
    public interface IDataFileAdapter : IDisposable
    {
        string DocumentFile { get; }

        /// <summary>
        ///     Read Data from file.
        /// </summary>
        /// <param name="firstRowIsHeader"></param>
        /// <returns></returns>
        IGetSetterCollection Read(bool firstRowIsHeader = true);

        void Write(IGetSetterCollection data, bool ignoreHeader = true);
    }
}