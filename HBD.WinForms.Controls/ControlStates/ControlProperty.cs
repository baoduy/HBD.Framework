using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace HBD.WinForms.Controls.ControlStates
{
    [Serializable, DebuggerDisplay("Name = {Name}")]
    public class ControlProperty
    {
        public ControlProperty() { }
        public ControlProperty(string name, object value,Type propertyType)
        {
            this.Name = name;
            this.Value = value;
            this.PropertyType = propertyType;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        
        [XmlIgnore]
        public Type PropertyType { get; private set; }
    }
}
