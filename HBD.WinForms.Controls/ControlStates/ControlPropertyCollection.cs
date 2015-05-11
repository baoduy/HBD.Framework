using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Core;
using System.Diagnostics;

namespace HBD.WinForms.Controls.ControlStates
{
    [Serializable, DebuggerDisplay("Count = {Count}")]
    public class ControlPropertyCollection : List<ControlProperty>
    {
        public new void Add(ControlProperty property)
        {
            Guard.ArgumentNotNull(property, "property");
            Guard.ArgumentNotNull(property.Name, "property.Name");

            if (this.FirstOrDefault(p => p.Name.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase)) != null)
                return;
            base.Add(property);
        }

        public virtual void Add(string name, object value,Type propertyType)
        {
            this.Add(new ControlProperty(name, value, propertyType));
        }
    }
}
