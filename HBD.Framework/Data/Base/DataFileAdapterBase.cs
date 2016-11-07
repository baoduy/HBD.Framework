using System.Data;

namespace HBD.Framework.Data.Base
{
    public abstract class DataFileAdapterBase : IDataFileAdapter
    {
        public abstract void Dispose();

        public string DocumentFile { get; }

        public abstract void Save();

        protected DataFileAdapterBase(string documentFile)
        {
            DocumentFile = documentFile;
        }

        public abstract DataSet ReadData(bool firstRowIsColumnName = true);

        public abstract void WriteData(DataSet data, bool ignoreHeader = true);
    }
}