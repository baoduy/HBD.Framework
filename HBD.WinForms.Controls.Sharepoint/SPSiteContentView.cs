using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Sharepoint.Libraries;

namespace HBD.WinForms.Controls.Sharepoint
{
    public partial class SPSiteContentView : HBDViewBase
    {
        public SPSiteContentView()
        {
            InitializeComponent();
        }

        private void spAllSiteContent_Selected(object sender, Libraries.SPTreeNodeEventArgs e)
        {
            if (e.Node is SPListTreeNode)
                this.spContentDetailsControl.DataSource = e.Node.SPAdapter.ToDataTable(e.Node.Text);
            else if (e.Node is SPViewTreeNode)
                this.spContentDetailsControl.DataSource = e.Node.SPAdapter.ToDataTable(e.Node.Parent.Text, e.Node.Text);
            else if(e.Node is SPGroupPermission)
                this.spContentDetailsControl.DataSource = e.Node.SPAdapter.AllUsersToDataTable(e.Node.Text);
        }

        private void spAllSiteContent_SourceChanged(object sender, EventArgs e)
        {
            this.spContentDetailsControl.DataSource = null;
        }
    }
}
