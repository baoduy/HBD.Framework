using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripSearchControl : HBDToolStripControlHost<DataGridViewSearchControl>
    {
        [DefaultValue(""), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ISearchable SearchableControl
        {
            get { return this.ChildControl.SearchableControl; }
            set { this.ChildControl.SearchableControl = value; }
        }
    }
}
