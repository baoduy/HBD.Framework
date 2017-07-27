#region using

using HBD.Framework.Data.GetSetters;

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

        public abstract IGetSetterCollection Read(bool firstRowIsHeader = true);

        public abstract void Write(IGetSetterCollection data, bool ignoreHeader = false);

        public abstract void Save();
    }
}