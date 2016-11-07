namespace HBD.Framework.Data.Excel.Base
{
    public class CellIndex
    {
        public CellIndex(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public int ColumnIndex { get; private set; }
        public int RowIndex { get; private set; }
    }
}