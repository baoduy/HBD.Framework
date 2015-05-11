using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.WinForms.Controls.Comparison.Events
{
    public class CompareFieldEventArgs : EventArgs
    {
        FieldComparisonControl _compareColumnControl;
        public FieldComparisonControl CompareColumnControl { get { return this._compareColumnControl; } }

        public CompareFieldEventArgs(FieldComparisonControl control)
        {
            this._compareColumnControl = control;
        }
    }
}
