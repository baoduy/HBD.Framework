#region

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

        public abstract void Dispose();

        public string DocumentFile { get; }

        public abstract DataSet ReadData(bool firstRowIsColumnName = true);

        public abstract void WriteData(DataSet data, bool ignoreHeader = true);

        public abstract void Save();
    }
}