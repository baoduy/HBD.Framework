using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using HBD.Framework.Core;

namespace HBD.Framework.Data.XML
{
    public class XMLAdapter : FileDataConverterBase
    {
        public XMLAdapter(string fileName) : base(fileName) { }

        public XMLAdapter(FileInfo file) : base(file) { }

        /// <summary>
        /// Conver xml file to DataTable
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns></returns>
        public override DataTable ToDataTable(string fileName = null)
        {
            fileName = this.EnsureFileName(fileName);
            Guard.PathExisted(fileName);

            var data = new DataTable();
            data.ReadXml(fileName);
            return data;
        }

        public void WriteFile(DataTable data, string xmlFileName = null)
        {
            xmlFileName = this.EnsureFileName(xmlFileName);
            data.WriteXml(xmlFileName, XmlWriteMode.WriteSchema, true);
        }

        public override void WriteFile(DataTable data)
        {
            this.WriteFile(data, string.Empty);
        }
    }
}
