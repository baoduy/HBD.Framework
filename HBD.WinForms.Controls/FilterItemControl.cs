using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Extension;
using HBD.WinForms.Controls.Attributes;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls
{
    [DefaultProperty("Item")]
    public partial class FilterItemControl : HBDControl
    {
        public FilterItemControl()
        {
            InitializeComponent();
            this.Operation = CompareOperation.Contains;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public ColumnItemCollection DataSource
        {
            get { return this.cb_Field.DataSource as ColumnItemCollection; }
            set { this.cb_Field.DataSource = value; }
        }

        private string FieldName { get; set; }
        private CompareOperation Operation { get; set; }
        private object Value { get; set; }
        private Control _valueControl = null;

        //Don't keep FilterItem object as it may referenced by another control
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState]
        public IFilterClause Item
        {
            get { return FilterManager.CreateFilterClause(this.FieldName, this.Operation, this.Value); }
            set
            {
                if (value != null && value is FilterClause)
                {
                    var f = value as FilterClause;
                    this.FieldName = f.FieldName;
                    this.Operation = f.Operation;
                    this.Value = f.Value;
                }
            }
        }

        private CompareOperation[] GetOperation()
        {
            if (!this.ValidateControls(this.cb_Field)) return null;

            var col = this.cb_Field.SelectedItem as ColumnItem;
            if (col == null || col.DataType == typeof(string))
            {
                return new CompareOperation[] { 
                    CompareOperation.Contains,
                    CompareOperation.NotContains,
                    CompareOperation.StartsWith, 
                    CompareOperation.EndsWith,
                    CompareOperation.Equals
                };
            }
            else if (col.DataType == typeof(DateTime))
            {
                return new CompareOperation[] { 
                    CompareOperation.GreaterThan,
                    CompareOperation.GreaterThanOrEquals,
                    CompareOperation.LessThan, 
                    CompareOperation.LessThanOrEquals,
                    CompareOperation.Equals
                };
            }
            else if (col.DataType != null)
            {
                return new CompareOperation[] { 
                    CompareOperation.GreaterThan,
                    CompareOperation.GreaterThanOrEquals,
                    CompareOperation.LessThan, 
                    CompareOperation.LessThanOrEquals,
                    CompareOperation.Equals
                };
            }
            else
            {
                return new CompareOperation[] { 
                     CompareOperation.Contains,
                    CompareOperation.NotContains,
                    CompareOperation.StartsWith, 
                    CompareOperation.EndsWith,
                    CompareOperation.GreaterThan,
                    CompareOperation.GreaterThanOrEquals,
                    CompareOperation.LessThan, 
                    CompareOperation.LessThanOrEquals,
                    CompareOperation.Equals
                };
            }
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();
            this.Height = this.cb_Field.Height + 6;

            this.LoadValueControl();
        }

        private void LoadValueControl()
        {
            var col = this.cb_Field.SelectedItem as ColumnItem;

            if (col == null || col.DataType == typeof(string))
                this._valueControl = new TextBox();
            else if (col.DataType == typeof(DateTime))
                this._valueControl = new DateTimePicker();
            else
            {
                this._valueControl = new NumericUpDown();
                ((NumericUpDown)this._valueControl).Maximum = decimal.MaxValue;
            }

            if (this._valueControl != null)
            {
                if (this.Value != null)
                    this._valueControl.SetDefaultValue(this.Value);

                this._valueControl.TextChanged += control_TextChanged;
                this._valueControl.Dock = DockStyle.Fill;
                this.tableLayoutPanel1.Controls.Add(this._valueControl, 2, 0);
            }
        }

        public override bool ValidateData()
        {
            return this.ValidateControls(this.cb_Field, this.cb_Ope, this._valueControl);
        }

        private void control_TextChanged(object sender, EventArgs e)
        {
            this.Value = sender.GetDefaultValue();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            this.cb_Field.Text = this.FieldName;
        }

        private void cb_Field_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FieldName = this.cb_Field.Text;
            this.cb_Ope.DataSource = this.GetOperation();
            this.cb_Ope.SelectedItem = this.Operation;
        }

        private void cb_Ope_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Operation = (CompareOperation)this.cb_Ope.SelectedItem;
        }
    }
}
