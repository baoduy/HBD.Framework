using HBD.Framework.Data.Sharepoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HBD.WinForms.Controls.Sharepoint.Libraries
{
    public abstract class SPTreeNode : TreeNode
    {
        private ISPAdapter _sPAdapter;
        public ISPAdapter SPAdapter
        {
            get
            {
                if (_sPAdapter == null && this.Parent != null && this.Parent is SPTreeNode)
                    return (this.Parent as SPTreeNode).SPAdapter;
                return _sPAdapter;
            }
            set { _sPAdapter = value; }
        }

        ~SPTreeNode()
        {
            if (this._sPAdapter != null)
            {
                this._sPAdapter.Dispose(); this._sPAdapter = null;
            }
        }
    }
}
