#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using HBD.Framework.Core;
using HBD.Framework.Data.Excel;

#endregion

namespace HBD.Framework.Data.GetSetters
{
    internal class ExcelSheetGetSetter : IGetSetterCollection
    {
        private IList<ExcelRowGetSetter> _rows;

        public ExcelSheetGetSetter(ExcelAdapter excelAdapter, string sheetName, SheetData sheetData)
        {
            Guard.ArgumentIsNotNull(excelAdapter, nameof(excelAdapter));
            Guard.ArgumentIsNotNull(sheetData, nameof(sheetData));
            Guard.ArgumentIsNotNull(sheetName, nameof(sheetName));

            ExcelAdapter = excelAdapter;
            SheetData = sheetData;
            Name = sheetName;
        }

        internal ExcelAdapter ExcelAdapter { get; }
        internal SheetData SheetData { get; }
        public string Name { get; }

        public IEnumerator<IGetSetter> GetEnumerator()
        {
            LoadRows();
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IGetSetter Header => null;

        private void LoadRows()
        {
            if (_rows == null)
                _rows = SheetData.Descendants<Row>().Select(r => new ExcelRowGetSetter(ExcelAdapter, r)).ToList();
        }
    }
}