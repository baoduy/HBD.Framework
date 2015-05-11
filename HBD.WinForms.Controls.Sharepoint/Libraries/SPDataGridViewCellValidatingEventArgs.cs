using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HBD.WinForms.Controls.Sharepoint.Libraries
{
    public class SPDataGridViewCellValidatingEventArgs : EventArgs
    {
        public SPDataGridViewCellValidatingEventArgs(DataGridViewCellValidatingEventArgs originalEvent, object oldCellValue)
        {
            this.OriginalEventArgs = originalEvent;
            this.OldCellValue = oldCellValue;
        }

        public DataGridViewCellValidatingEventArgs OriginalEventArgs { get; private set; }
        public object OldCellValue { get; private set; }
    }
}
