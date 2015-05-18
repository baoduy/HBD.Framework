using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace HBD.Framework.Data
{
    public abstract class FileDataConverterBase : DataConverterBase
    {
        public string FileName { get; private set; }

        protected string EnsureFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.FileName;

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot empty");
            return fileName;
        }

        public FileDataConverterBase() { }
        public FileDataConverterBase(string fileName) { this.FileName = fileName; }
        public FileDataConverterBase(FileInfo file) : this(file != null ? file.FullName : null) { }

        public abstract void WriteFile(DataTable data);
    }
}
