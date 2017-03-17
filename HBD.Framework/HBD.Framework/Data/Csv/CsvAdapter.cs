#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using HBD.Framework.Core;
using HBD.Framework.Data.Base;
using HBD.Framework.Data.Csv.Base;
using HBD.Framework.Data.GetSetters;
using Microsoft.VisualBasic.FileIO;

#endregion

namespace HBD.Framework.Data.Csv
{
    public class CsvAdapter : DataFileAdapterBase
    {
        public CsvAdapter(string documentFile) : base(documentFile)
        {
        }

        private static string QuoteValue(object value, string dateFormat = null, string numericFormat = null)
        {
            if (value == null) return string.Empty;
            string str;

            if (value is DateTime && dateFormat.IsNotNullOrEmpty())
                // ReSharper disable once AssignNullToNotNullAttribute
                str = string.Format(dateFormat, value);
            else if (value.IsNumericType() && numericFormat.IsNotNullOrEmpty())
                // ReSharper disable once AssignNullToNotNullAttribute
                str = string.Format(numericFormat, value);
            else str = value.ToString();

            return str.Contains(",") && !str.StartsWith("\"") && !str.EndsWith("\"")
                ? string.Concat("\"", str, "\"")
                : str;
        }

        private static string QuoteValues(WriteCsvOption options, params object[] values)
        {
            var dilimiter = options.Dilimiters.FirstOrDefault();

            var build = new StringBuilder();
            for (var i = 0; i < values.Length; i++)
            {
                var a = QuoteValue(values[i], options.DateFormat, options.NumericFormat);

                if (options.TextFieldType == FieldType.Delimited)
                {
                    if (build.Length <= 0) build.Append(a);
                    else build.AppendFormat("{0}{1}", dilimiter, a);
                }
                else
                {
                    var s = options.FieldWidths.Length == values.Length
                        ? options.FieldWidths[i]
                        : options.FieldWidths[0];

                    var format = "{0,-" + s + "}";
                    build.AppendFormat(format, a);
                }
            }

            return build.ToString();
        }

        public virtual DataTable Read()
        {
            var data = new DataTable();
            ReadInto(data);
            return data;
        }

        public virtual void ReadInto(DataTable table, Action<ReadCsvOption> options = null)
        {
            if (table == null || !File.Exists(DocumentFile)) return;

            var op = new ReadCsvOption();
            options?.Invoke(op);

            table.TableName = Path.GetFileNameWithoutExtension(DocumentFile);
            table.Columns.Clear();
            table.Rows.Clear();

            using (var csvReader = new TextFieldParser(DocumentFile))
            {
                csvReader.SetDelimiters(op.Dilimiters);
                csvReader.HasFieldsEnclosedInQuotes = true;
                csvReader.TextFieldType = op.TextFieldType;

                if (op.FieldWidths.AnyItem())
                    csvReader.SetFieldWidths(op.FieldWidths);

                if (op.FirstRowIsHeader)
                {
                    //read column names
                    var colFields = csvReader.ReadFields();

                    if (colFields != null)
                        foreach (var column in colFields)
                            table.Columns.Add(new DataColumn(column));
                }

                while (!csvReader.EndOfData)
                {
                    var fieldData = csvReader.ReadFields();
                    if (fieldData == null) continue;

                    if (fieldData.Length >= table.Columns.Count)
                        table.AddMoreColumns(fieldData.Length);
                    table.Rows.Add(fieldData.Cast<object>().ToArray());
                }
            }
        }

        public virtual void Write(DataTable table, Action<WriteCsvOption> options = null)
            => Write(new DataTableGetSetterCollection(table), options);

        public virtual void Write(IGetSetterCollection data, Action<WriteCsvOption> options = null)
        {
            Guard.ArgumentIsNotNull(data, nameof(data));

            var op = new WriteCsvOption();
            options?.Invoke(op);

            using (var writer = new StreamWriter(DocumentFile))
            {
                if (!op.IgnoreHeader)
                    writer.WriteLine(QuoteValues(op,
                        data.Header.Select(c => QuoteValue(c)).Cast<object>().ToArray()));

                foreach (var row in data)
                    writer.WriteLine(QuoteValues(op, row.ToArray()));
            }
        }

        public override void Dispose()
        {
        }

        public override void Save()
        {
        }

        public override DataSet ReadData(bool firstRowIsColumnName = true)
        {
            var data = Read();
            var set = new DataSet();
            set.Tables.Add(data);
            return set;
        }

        public override void WriteData(DataSet data, bool ignoreHeader = true)
        {
            if (data.Tables.Count > 1) throw new NotSupportedException("The DataSet has more than 1 table.");
            Write(new DataTableGetSetterCollection(data.Tables[0]), op => op.IgnoreHeader = ignoreHeader);
        }
    }
}