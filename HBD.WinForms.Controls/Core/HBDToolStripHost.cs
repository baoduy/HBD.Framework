using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Extension;
using System.Drawing;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace HBD.WinForms.Controls.Core
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class HBDToolStripControlHost<TControl> : ToolStripControlHost where TControl : Control
    {
        public HBDToolStripControlHost() : base(typeof(TControl).CreateInstance() as Control) { }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public TControl ChildControl
        {
            get { return (TControl)base.Control; }
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            return new Size(this.ChildControl.Width > this.Width ? this.ChildControl.Width : this.Width, 0);
        }
    }
}
