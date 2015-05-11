using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HBD.WinForms.Controls.Events
{
    public class ListControlCollectionEventArgs : EventArgs
    {
        public Control Control { get; private set; }
        public ListControlCollectionEventArgs(Control control)
        {
            this.Control = control;
        }
    }
}
