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
    internal class ExcelRowGetSetter : IGetSetter
    {
        private IDictionary<int, Cell> _cells;

        public ExcelRowGetSetter(ExcelAdapter excelAdapter, Row row)
        {
            Guard.ArgumentIsNotNull(excelAdapter, nameof(excelAdapter));
            Guard.ArgumentIsNotNull(row, nameof(row));

            ExcelAdapter = excelAdapter;
            Row = row;
        }

        internal Row Row { get; }
        internal ExcelAdapter ExcelAdapter { get; }

        public IEnumerator<object> GetEnumerator()
        {
            LoadCells();
            return Enumerable.Range(0, _cells.Keys.Max() + 1)
                .Select(i => _cells.ContainsKey(i) ? _cells[i].GetValue(ExcelAdapter.WorkbookPart) : null)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object this[string name]
        {
            get
            {
                var index = CommonFuncs.GetExcelColumnIndex(name);
                return this[index];
            }
            set
            {
                var index = CommonFuncs.GetExcelColumnIndex(name);
                this[index] = value;
            }
        }

        public object this[int index]
        {
            get
            {
                LoadCells();
                if (!_cells.ContainsKey(index)) return null;

                var cell = _cells[index];
                return cell.GetValue(ExcelAdapter.WorkbookPart);
            }
            set
            {
                LoadCells();
                Cell cell;

                if (_cells.ContainsKey(index))
                {
                    cell = _cells[index];
                    cell.SetValue(ExcelAdapter.WorkbookPart, value);
                }
                else
                {
                    cell = ExcelAdapter.CreateCell((int) Row.RowIndex.Value, index, value);
                    Row.AppendChild(cell);
                    _cells.Add(index, cell);
                }
            }
        }

        private void LoadCells()
        {
            if (_cells != null) return;

            _cells = new SortedDictionary<int, Cell>();

            foreach (var c in Row.Descendants<Cell>())
                _cells.Add(ExcelAdapter.GetCellIndex(c.CellReference).ColumnIndex, c);
        }
    }
}