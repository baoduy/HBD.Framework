using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HBD.WinForms.Controls.Comparison.Core
{
    public class DataGridSelectionChangedEventArgs : EventArgs
    {
        public DataGridView Grid { get;private set; }

        public DataGridSelectionChangedEventArgs(DataGridView grid)
        { this.Grid = grid; }
    }
}
