#region using

using System.Data;

#endregion

namespace HBD.Framework.Data.Base
{
    public abstract class DataFileAdapterBase : IDataFileAdapter
    {
        protected DataFileAdapterBase(string documentFile)
        {
            DocumentFile = documentFile;
        }

        public void Dispose() => Dispose(true);

        protected abstract void Dispose(bool isDisposing);

        public string DocumentFile { get; }

#if !NETSTANDARD1_6
        public abstract DataSet ReadData(bool firstRowIsColumnName = true);

        public abstract void WriteData(DataSet data, bool ignoreHeader = true);
#endif
        public abstract void Save();
    }
}