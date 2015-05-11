using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HBD.WinForms.Controls.ControlStates
{
    [Serializable, DebuggerDisplay("Name = {Name}, Count = {Properties.Count}")]
    public class ControlState
    {
        public ControlState() : this(string.Empty) { }

        public ControlState(string controlName)
        {
            this.Name = controlName;
            this.Properties = new ControlPropertyCollection();
        }

        public string Name { get; set; }
        public ControlPropertyCollection Properties { get; set; }
    }
}
