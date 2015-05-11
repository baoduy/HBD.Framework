using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.WinForms.Controls.Sharepoint.Libraries
{
    public class SPTreeNodeEventArgs : EventArgs
    {
        public SPTreeNode Node { get; private set; }
        public SPTreeNodeEventArgs(SPTreeNode node)
        {
            this.Node = node;
        }
    }
}
