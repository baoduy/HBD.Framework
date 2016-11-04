using DocumentFormat.OpenXml.Spreadsheet;
using HBD.Framework.Core;
using HBD.Framework.Data.Excel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Data.GetSetters
{
    internal class ExcelRowGetSetter : IGetSetter
    {
        internal Row Row { get; }
        internal ExcelAdapter ExcelAdapter { get; }

        public ExcelRowGetSetter(ExcelAdapter excelAdapter, Row row)
        {
            Guard.ArgumentIsNotNull(excelAdapter, nameof(excelAdapter));
            Guard.ArgumentIsNotNull(row, nameof(row));

            this.ExcelAdapter = excelAdapter;
            this.Row = row;
        }

        private IDictionary<int, Cell> _cells;

        private void LoadCells()
        {
            if (_cells != null) return;

            _cells = new SortedDictionary<int, Cell>();

            foreach (var c in Row.Descendants<Cell>())
                _cells.Add(ExcelAdapter.GetCellIndex(c.CellReference).ColumnIndex, c);
        }

        public IEnumerator<object> GetEnumerator()
        {
            this.LoadCells();
            return Enumerable.Range(0, this._cells.Keys.Max() + 1)
                    .Select(i => _cells.ContainsKey(i) ? _cells[i].GetValue(ExcelAdapter.WorkbookPart) : null)
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public object this[string name]
        {
            get
            {
                var index = ExcelAdapter.GetColumnIndex(name);
                return this[index];
            }
            set
            {
                var index = ExcelAdapter.GetColumnIndex(name);
                this[index] = value;
            }
        }

        public object this[int index]
        {
            get
            {
                this.LoadCells();
                if (!_cells.ContainsKey(index)) return null;

                var cell = this._cells[index];
                return cell.GetValue(ExcelAdapter.WorkbookPart);
            }
            set
            {
                this.LoadCells();
                Cell cell;

                if (_cells.ContainsKey(index))
                {
                    cell = this._cells[index];
                    cell.SetValue(ExcelAdapter.WorkbookPart, value);
                }
                else
                {
                    cell = ExcelAdapter.CreateCell((int)Row.RowIndex.Value, index, value);
                    this.Row.AppendChild(cell);
                    this._cells.Add(index, cell);
                }
            }
        }
    }
}