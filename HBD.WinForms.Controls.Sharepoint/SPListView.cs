using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls;
using HBD.Framework.Extension.WinForms;
using HBD.Framework.Extension;
using HBD.Framework.Data.Sharepoint;
using HBD.Framework.Data.Utilities;
using HBD.WinForms.Controls.Sharepoint.Libraries;

namespace HBD.WinForms.Controls.Sharepoint
{
    public partial class SPListView : HBDViewBase
    {
        public SPListView()
        {
            InitializeComponent();
        }

        protected override IList<ControlStates.ControlState> GetListControlState()
        {
            var list = base.GetListControlState();
            list.Add(this.GetControlState(this.spContentDetailsControl));
            list.Add(this.GetControlState(this.spContentDetailsControl.BrowserControl));
            return list;
        }

        private void spContentDetailsControl_CellValidating(object sender, SPDataGridViewCellValidatingEventArgs e)
        {
            if (this.spContentDetailsControl.IsEditing)
            {
                if (e.OldCellValue != null && e.OldCellValue.IsEquals(e.OriginalEventArgs.FormattedValue))
                    return;

                bool isCancelEdit = false;
                var row = this.spContentDetailsControl.Rows[e.OriginalEventArgs.RowIndex];
                if (row == null)
                    isCancelEdit = true;

                if (!isCancelEdit)
                {
                    //Get ID of Items, Updating field
                    var id = (int)Convert.ChangeType(row.Cells[DefaultSPValues.InternalFieldName.ID].Value, typeof(int));
                    var field = this.spContentDetailsControl.Columns[e.OriginalEventArgs.ColumnIndex].Name;
                    var newValue = e.OriginalEventArgs.FormattedValue;

                    //Show the confirm message
                    if (MessageBox.Show(string.Format("Do you want to changes value of Field '{0}' on Item '{1}' from '{2}' to '{3}'?", field, id, e.OldCellValue, newValue),
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        isCancelEdit = true;
                    }

                    if (!isCancelEdit)
                    {
                        try
                        {
                            using (var adapter = SPAdapterManager.Open(this.spContentDetailsControl.SourcePath))
                            {
                                adapter.Update(this.spContentDetailsControl.SourceName, id, field, newValue);
                            }
                        }
                        catch (Exception ex)
                        {
                            isCancelEdit = true;
                            MessageBox.Show(ex.Message, "Error When Update List Item Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (isCancelEdit)
                    {
                        e.OriginalEventArgs.Cancel = true;
                        this.spContentDetailsControl.CancelEdit();
                    }
                    else MessageBox.Show(string.Format("Item '{0}' has been updated field '{1}' with new value '{2}'", id, field, newValue), "Update List Item Value", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.spContentDetailsControl.SearchManager.Status != SearchStatus.None)
            {
                this.lb_Status.Text = string.Format("{0}..., Result: {1}", this.spContentDetailsControl.SearchManager.Status, this.spContentDetailsControl.SearchManager.Total);
                this.statusStrip1.Refresh();
            }
        }

        private void spContentDetailsControl_SearchStatusChanged(object sender, Events.SearchlEventArgs e)
        {
            if (e.SearchManager.Status == SearchStatus.Started)
            {
                this.timer.Enabled = true;
                this.timer.Start();
            }
            else if (e.SearchManager.Status == SearchStatus.Completed)
            {
                this.timer.Stop();
                this.timer.Enabled = false;
            }
        }
    }
}
