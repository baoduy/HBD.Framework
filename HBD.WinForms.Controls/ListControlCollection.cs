using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Events;
using HBD.Framework.Core;
using HBD.Framework.Extension;
using System.Collections.ObjectModel;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("ChildrenControlAdded")]
    public partial class ListControlCollection : HBDControl, IListControlCollection
    {
        public ListControlCollection()
        {
            InitializeComponent();
            this.childredControls = new List<Control>();
            this.ChildrenControls = new ReadOnlyCollection<Control>(this.childredControls);
            this.AllowAddControl = true;
            this.AllowRemoveControl = true;

            this.bt_AddRemove.CollectionControl = this;
        }

        /// <summary>
        /// The Type of control when click button Add/Remove
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("")]
        public Type ChildrenControlType { get; set; }

        private bool allowAddControl;
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool AllowAddControl
        {
            get { return allowAddControl; }
            set
            {
                allowAddControl = value;
                this.OnAllowancePropertiesChanged(EventArgs.Empty);
            }
        }

        private bool allowRemoveControl;
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool AllowRemoveControl
        {
            get { return allowRemoveControl; }
            set
            {
                allowRemoveControl = value;
                this.OnAllowancePropertiesChanged(EventArgs.Empty);
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool ShowAddRemoveButton
        {
            get { return this.bt_AddRemove.Visible; }
            set { this.bt_AddRemove.Visible = value; }
        }

        IList<Control> childredControls;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        public ReadOnlyCollection<Control> ChildrenControls { get; private set; }

        public event EventHandler<ListControlCollectionEventArgs> ChildrenControlAdded;
        protected virtual void OnChildrenControlAdded(ListControlCollectionEventArgs e)
        {
            if (this.ChildrenControlAdded != null)
                this.ChildrenControlAdded(this, e);
        }

        public event EventHandler<ListControlCollectionEventArgs> ChildrenControlRemoved;
        protected virtual void OnChildrenControlRemoved(ListControlCollectionEventArgs e)
        {
            if (this.ChildrenControlRemoved != null)
                this.ChildrenControlRemoved(this, e);
        }

        public event EventHandler AllowancePropertiesChanged;
        protected virtual void OnAllowancePropertiesChanged(EventArgs e)
        {
            if (this.AllowancePropertiesChanged != null)
                this.AllowancePropertiesChanged(this, e);
        }

        public virtual Control AddNewControl()
        {
            if (this.ChildrenControlType == null)
            {
                MessageBox.Show("Please provide ChildrenControlType", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var control = this.ChildrenControlType.CreateInstance() as Control;
            if (control == null)
                throw new Exception(string.Format("Cannot create instance of {0}", this.ChildrenControlType.Name));

            control.Dock = DockStyle.Top;
            this.childredControls.Add(control);

            var newIndex = this.tableColumns.GetRow(this.bt_AddRemove);
            if (this.childredControls.Count > 0)
                newIndex += 1;

            this.tableColumns.Controls.Add(control, 0, newIndex);
            this.tableColumns.SetRow(this.bt_AddRemove, newIndex);

            this.OnChildrenControlAdded(new ListControlCollectionEventArgs(control));
            return control;
        }

        public virtual void RemoveControl()
        {
            if (this.childredControls.Count > 0)
            {
                var control = this.childredControls[this.childredControls.Count - 1];
                this.childredControls.RemoveAt(this.childredControls.Count - 1);
                this.tableColumns.Controls.Remove(control);
                control.Dispose();

                var index = this.tableColumns.GetRow(this.bt_AddRemove);
                this.tableColumns.SetRow(this.bt_AddRemove, index - 1);
                this.OnChildrenControlRemoved(new ListControlCollectionEventArgs(control));
            }
        }

        public virtual void Clear()
        {
            if (this.childredControls != null)
            {
                foreach (var c in this.childredControls)
                {
                    this.tableColumns.Controls.Remove(c);
                    c.Dispose();
                }

                this.tableColumns.SetRow(this.bt_AddRemove, this.tableColumns.GetRow(this.bt_AddRemove) - this.childredControls.Count + 1);
                this.childredControls.Clear();

                this.OnChildrenControlRemoved(new ListControlCollectionEventArgs(null));
            }
        }

        protected override void SuspendLayout()
        {
            if (this.tableColumns != null)
                this.tableColumns.SuspendLayout();
            base.SuspendLayout();

        }

        public override void ResumeLayout()
        {
            base.ResumeLayout();
            if (this.tableColumns != null)
                this.tableColumns.ResumeLayout();
        }

        public override void ResumeLayout(bool performLayout)
        {
            base.ResumeLayout(performLayout);
            if (this.tableColumns != null)
                this.tableColumns.ResumeLayout(performLayout);
        }
    }
}
