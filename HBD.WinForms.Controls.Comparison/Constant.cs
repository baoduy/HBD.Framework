using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HBD.WinForms.Controls.Comparison
{
    internal static class Constant
    {
        public static readonly Color NotFoundAColor = Color.OrangeRed;
        public static readonly Color NotFoundBColor = Color.OrangeRed;
        public static readonly Color DifferenceColor = Color.Yellow;
    }

    public enum CompareRowType
    { RowByRow = 0x0, PrimaryKey = 0x1 }

    public enum CompareColumnType
    { ColumnByColumn = 0x0, ColumnSpecification = 0x1 }
}
