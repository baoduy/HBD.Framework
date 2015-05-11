using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HBD.WinForms.Controls
{
    public static class WinFormManager
    {
        public static void ShowDialog(this UserControl parent,string message, Control control, int width = 600, int height = 800)
        {
            var form = new HBDForm()
            {
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowIcon=false,
                ShowInTaskbar=false,
                Width = width,
                Height = height,
                Text = message
            };
            control.Dock = DockStyle.Fill;
            form.Controls.Add(control);
            form.ShowDialog(parent.ParentForm);
        }
    }
}
