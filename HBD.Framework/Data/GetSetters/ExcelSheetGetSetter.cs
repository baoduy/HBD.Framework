using DocumentFormat.OpenXml.Spreadsheet;
using HBD.Framework.Core;
using HBD.Framework.Data.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    internal class ExcelSheetGetSetter : IGetSetterCollection
    {
        internal ExcelAdapter ExcelAdapter { get; }
        internal SheetData SheetData { get; }
        public string Name { get; }

        public ExcelSheetGetSetter(ExcelAdapter excelAdapter, string sheetName, SheetData sheetData)
        {
            Guard.ArgumentIsNotNull(excelAdapter, nameof(excelAdapter));
            Guard.ArgumentIsNotNull(sheetData, nameof(sheetData));
            Guard.ArgumentIsNotNull(sheetName, nameof(sheetName));

            this.ExcelAdapter = excelAdapter;
            this.SheetData = sheetData;
            this.Name = sheetName;
        }

        private IList<ExcelRowGetSetter> _rows;

        private void LoadRows()
        {
            if (_rows == null)
                _rows = SheetData.Descendants<Row>().Select(r => new ExcelRowGetSetter(this.ExcelAdapter, r)).ToList();
        }

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            this.LoadRows();
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IGetSetter Header => null;
    }
}