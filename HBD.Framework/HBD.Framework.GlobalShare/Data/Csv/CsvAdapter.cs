#region using

using System.IO;
using HBD.Framework.Data.Base;
using HBD.Framework.Data.GetSetters;
using CsvHelper;
using System.Collections.Generic;
using CsvHelper.Configuration;
using System;

#endregion

namespace HBD.Framework.Data.Csv
{
    public class CsvAdapter : DataFileAdapterBase
    {
        public CsvAdapter(string documentFile) : base(documentFile)
        {
        }

        protected override void Dispose(bool isDisposing) { }

        public override void Save()
        {
        }
        public override IGetSetterCollection Read(bool firstRowIsHeader = true)
            => Read(new ReadCsvOption { FirstRowIsHeader = firstRowIsHeader });

        public virtual IGetSetterCollection Read(ReadCsvOption option)
        {
            var list = new List<IGetSetter>();
            IGetSetter header = null;

            using (var reader = new CsvReader(File.OpenText(this.DocumentFile), new CsvConfiguration { Delimiter = option.Delimiter }))
            {
                if (reader.ReadHeader())
                {
                    if (option.FirstRowIsHeader)
                        header = new ArrayGetSetter(reader.FieldHeaders);
                    else list.Add(new ArrayGetSetter(reader.FieldHeaders));
                }

                while (reader.Read())
                    list.Add(new ArrayGetSetter(reader.CurrentRecord));
            }

            return new ArrayGetSetterCollection(header, list);
        }

        public override void Write(IGetSetterCollection data, bool ignoreHeader = false)
            => Write(data, new WriteCsvOption { IgnoreHeader = ignoreHeader });

        public virtual void Write(IGetSetterCollection data, WriteCsvOption option)
        {
            using (var writer = new CsvWriter(File.CreateText(this.DocumentFile), new CsvConfiguration { Delimiter = option.Delimiter }))
            {
                if (!option.IgnoreHeader && data.Header != null)
                {
                    foreach (var item in data.Header)
                        writer.WriteField(item);
                }
                writer.NextRecord();

                foreach (var row in data)
                {
                    foreach (var item in row)
                    {
                        var val = item;

                        //Apply NumericFormat before write
                        if (option.NumericFormat.IsNotNullOrEmpty() && (item.IsNotNumericType() || item.IsNumber()))
                            val = string.Format(option.NumericFormat, item);

                        //Apply DateFormat before write
                        if (option.DateFormat.IsNotNullOrEmpty() && (item is DateTime || item is DateTimeOffset))
                            val = string.Format(option.DateFormat, item);

                        writer.WriteField(val);
                    }

                    writer.NextRecord();
                }
            }
        }
    }
}