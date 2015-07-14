using HBD.WinForms.Controls.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Extension.WinForms;

namespace HBD.WinForms.Controls
{
    public partial class HBDPanel : Panel
    {
        public HBDPanel() { }

        public void ClearControls()
        {
            this.SuspendLayout();

            foreach (UserControl c in this.Controls)
                c.Dispose();
            this.Controls.Clear();

            this.ResumeLayout();
        }

        /// <summary>
        /// Add and Apply DockStyle.Fill to control
        /// </summary>
        /// <param name="control"></param>
        public void AddControlWithFillDock(UserControl control)
        {
            if (control == null)
                return;

            this.ClearControls();
            this.Controls.Add(control);
            control.Dock = DockStyle.Fill;
        }
    }
}
