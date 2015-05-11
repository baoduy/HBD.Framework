using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using HBD.Framework.Extension;
using HBD.Framework.Extension.WinForms;
using HBD.WinForms.Controls.Attributes;
using System.Collections;
using HBD.Framework.Core;

namespace HBD.WinForms.Controls.Core
{
    public class HBDViewBase : HBDControl, IHBDViewBase
    {
        string _fullName = string.Empty;
        protected string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                    _fullName = this.GetType().FullName + "." + this.Name;
                return _fullName;
            }
        }

        protected virtual Type[] GetPropertyTypes(IList<ControlStates.ControlState> listControlState)
        {
            if (listControlState == null || listControlState.Count == 0)
                return null;

            return (from c in listControlState
                    from p in c.Properties
                    where p.PropertyType != typeof(string)
                    select p.PropertyType).ToArray();
        }

        protected virtual ControlStates.ControlState GetControlState(ToolStripItem control)
        {
            if (control == null)
                return new ControlStates.ControlState();

            var ctrlState = new ControlStates.ControlState(control.Name);

            if (control is ToolStripComboBox)
            {
                var c = control as ToolStripComboBox;
                ctrlState.Properties.Add(new ControlStates.ControlProperty("Text", c.Text, typeof(string)));
            }
            else
            {
                var property = control.GetDefaultProperty();
                if (property != null)
                    ctrlState.Properties.Add(new ControlStates.ControlProperty(property.Name, control.GetValue(property), property.PropertyType));
            }
            return ctrlState;
        }

        protected virtual ControlStates.ControlState GetControlState(UserControl control)
        {
            if (control == null)
                return new ControlStates.ControlState();

            var defProp = control.GetDefaultProperty();
            var listControlStateAtt = control.GetPropertiesByAttribute<ControlPropertyStateAttribute>();

            var controlState = new ControlStates.ControlState(control.Name);
            controlState.Properties.Add(defProp.Name, control.GetValue(defProp), defProp.PropertyType);

            foreach (var p in listControlStateAtt)
            {
                var value = control.GetValue(p);
                if (value != null && value is ICollection)
                {
                    controlState.Properties.Add(p.Name, XmlSerializeManager.Serialize(value), p.PropertyType);
                }
                else controlState.Properties.Add(p.Name, value, p.PropertyType);
            }

            return controlState;
        }

        /// <summary>
        /// Implement this method for Load and Save State
        /// </summary>
        /// <returns></returns>
        protected virtual IList<ControlStates.ControlState> GetListControlState()
        {
            return new List<ControlStates.ControlState>();
        }

        public void LoadState(StringCollection collection)
        {
            if (collection == null)
                return;

            var value = collection.Cast<string>().FirstOrDefault(s => s.StartsWith(this.FullName));
            if (string.IsNullOrEmpty(value))
                return;

            var listControlState = this.GetListControlState();
            if (listControlState.Count == 0)
                return;

            var currentExtraType = GetPropertyTypes(listControlState);
            value = value.Replace(string.Format("{0}:", this.FullName), string.Empty);
            var currentSate = HBD.Framework.Core.XmlSerializeManager.Deserialize<List<ControlStates.ControlState>>(value, currentExtraType);

            if (currentSate == null) return;

            //Ensure Childrend Control
            if (!this.IsCreateChildrenControlCreated)
                this.CreateChildrenControl();

            foreach (var c in currentSate)
            {
                var item = this.FindControl(c.Name);
                if (item == null)
                    continue;

                foreach (var p in c.Properties)
                {
                    var prop = item.GetProperty(p.Name);
                    if (prop == null)
                        continue;

                    if (typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                    {
                        var val = HBD.Framework.Core.XmlSerializeManager.Deserialize(prop.PropertyType, p.Value as string, currentExtraType);
                        prop.SetValue(item, val, null);
                    }
                    else item.SetValue(p.Name, p.Value);
                }
            }

            //Reload data from state to control
            this.LoadControlData();
        }

        public void SaveState(StringCollection collection)
        {
            if (collection == null)
                return;

            var value = collection.Cast<string>().FirstOrDefault(s => s.StartsWith(this.FullName));
            if (!string.IsNullOrEmpty(value))
                collection.Remove(value);

            var listControlState = this.GetListControlState();
            if (listControlState.Count == 0)
                return;

            var extraType = GetPropertyTypes(listControlState);

            value = HBD.Framework.Core.XmlSerializeManager.Serialize(listControlState, extraType);
            collection.Add(string.Format("{0}:{1}", this.FullName, value));
        }
    }
}
