using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using HBD.WinForms.Controls.Comparison.Events;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls.Comparison.Core
{
     [DefaultEvent("SelectChange")]
    public class ComparisonBrowser : HBDControl, IComparisonBrowser
    {
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable TableA { get; protected set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable TableB { get; protected set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public string OriginalSourceA { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public string OriginalSourceB { get; set; }

        public event EventHandler<FileSelectedEventArgs> SelectionChanged;
        protected virtual void OnSelectionChanged(FileSelectedEventArgs e)
        {
            if (this.SelectionChanged != null)
                this.SelectionChanged(this, e);
        }
    }
}
