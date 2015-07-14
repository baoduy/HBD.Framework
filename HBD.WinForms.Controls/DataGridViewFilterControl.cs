using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data;
using HBD.WinForms.Controls.Utilities;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Core;
using HBD.Framework.Log;

namespace HBD.WinForms.Controls
{
    public partial class DataGridViewFilterControl : HBDControl
    {
        public DataGridViewFilterControl()
        {
            InitializeComponent();

            this.filterCollection.ChildrenControlType = typeof(FilterItemControl);
        }

        IFilterableControl<FilterClause> _filterableControl = null;
        public IFilterableControl<FilterClause> FilterableControl
        {
            get { return _filterableControl; }
            set
            {
                if (this._filterableControl != value)
                {
                    if (this._filterableControl != null)
                        this._filterableControl.ColumnNamesChanged -= _filterableControl_ColumnNamesChanged;
                    _filterableControl = value;
                    this._filterableControl.ColumnNamesChanged += _filterableControl_ColumnNamesChanged;
                }
            }
        }

        private void _filterableControl_ColumnNamesChanged(object sender, EventArgs e)
        {
            this.LoadControlData();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            if (this == null || this.FilterableControl == null) return;
            this.Enabled = this.FilterableControl.ColumnItems.Count > 0;
        }

        public override bool ValidateData()
        {
            foreach (HBDControl control in this.filterCollection.ChildrenControls)
            {
                if (control != null && !control.ValidateData())
                    return false;
            }
            return true;
        }

        private void bt_Filter_Click(object sender, EventArgs e)
        {
            try
            {
                Guard.ArgumentNotNull(this.FilterableControl, "FilterableControl");

                if (!this.ValidateData())
                    return;

                IFilterClause filter = null;

                foreach (var f in this.filterCollection.ChildrenControls.Cast<FilterItemControl>().Select(c => c.Item))
                {
                    if (filter == null)
                    {
                        filter = f;
                        continue;
                    }

                    if (this.ch_MatchAnyRule.Checked)
                        filter = filter.OrWith(f);
                    else filter = filter.AndsWith(f);
                };

                this.FilterableControl.Filter(filter);
                this.ParentForm.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filterCollection_ChildrenControlAdded(object sender, Events.ListControlCollectionEventArgs e)
        {
            if (this.FilterableControl != null)
            {
                var control = e.Control as FilterItemControl;
                control.DataSource = this.FilterableControl.ColumnItems.Clone();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.filterCollection.Clear();
        }
    }
}
