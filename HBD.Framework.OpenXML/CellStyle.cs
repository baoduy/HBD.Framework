using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HBD.Framework.OpenXML
{
    public class CellStyle
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public Color BackgroundColor { get; set; }
        public Color ForeColor { get; set; }

        public bool IsEmpty
        {
            get { return this.BackgroundColor.IsEmpty && this.ForeColor.IsEmpty; }
        }
    }
}
