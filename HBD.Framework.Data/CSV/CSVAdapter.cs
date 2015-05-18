using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using HBD.Framework.Core;

namespace HBD.Framework.Data.CSV
{
    public class CSVAdapter : FileDataConverterBase
    {
        const string CSVDelimiterChars = ";,\t|";

        //public string FileName { get; private set; }

        public CSVAdapter() { }

        public CSVAdapter(FileInfo file) : base(file) { }

        public CSVAdapter(string fileName) : base(fileName) { }


        private char GetDelimiter()
        {
            using (var reader = System.IO.File.OpenText(this.FileName))
            {
                var line = reader.ReadLine();

                //Try Read one more time
                if (string.IsNullOrEmpty(line))
                    line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return char.MinValue;

                foreach (char c in CSVDelimiterChars)
                {
                    if (line.IndexOf(c) > 0)
                        return c;
                }
            }

            return char.MinValue;
        }

        public override DataTable ToDataTable(string fileName = null)
        {
            fileName = this.EnsureFileName(fileName);
            Guard.PathExisted(fileName);

            var csvData = new DataTable();
            try
            {
                using (var csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(this.FileName))
                {
                    var delimiter = this.GetDelimiter();
                    csvReader.SetDelimiters(new string[] { delimiter.ToString() });
                    csvReader.HasFieldsEnclosedInQuotes = true;

                    //read column names
                    var colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        var fieldData = csvReader.ReadFields();
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return csvData;
        }

        private string QuoteValue(object value)
        {
            if (value == null)
                return string.Empty;
            var str = value.ToString().Replace("\"", "\"\"");
            if (str.Contains(","))
                return string.Concat("\"", str, "\"");
            return str;
        }

        private string QuoteValues(object[] values)
        {
            var build = new StringBuilder();
            foreach (var val in values)
            {
                if (build.Length <= 0)
                    build.Append(QuoteValue(val));
                else build.AppendFormat(",{0}", QuoteValue(val));
            }

            return build.ToString();
        }

        public override void WriteFile(DataTable data)
        {
            if (data == null)
                return;

            using (var writer = new StreamWriter(this.FileName))
            {
                //Write Header
                writer.WriteLine(string.Join(",", data.Columns.Cast<DataColumn>().Select(c => QuoteValue(c.ColumnName)).ToArray()));

                foreach (DataRow row in data.Rows)
                    writer.WriteLine(QuoteValues(row.ItemArray));
            }
        }
    }
}
