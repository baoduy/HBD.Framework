using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HBD.Framework.OpenXML
{
    public class CellStyleCollection : List<CellStyle>
    {
        public void Add(int rowIndex, int columnIndex, Color ForceColor, Color BackgroundColor)
        {
            this.Add(new CellStyle() { RowIndex = rowIndex, ColumnIndex = columnIndex, BackgroundColor = BackgroundColor, ForeColor = ForceColor });
        }
    }
}
