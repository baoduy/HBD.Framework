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
using HBD.WinForms.Controls.Events;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Sharepoint.Libraries;

namespace HBD.WinForms.Controls.Sharepoint
{
    [DefaultEvent("CellValidating")]
    public partial class SPContentDetailsControl : ViewDataControl
    {
        object _oldCellValue = null;
        bool hasHiddenRows = false;

        public SPContentDetailsControl()
        {
            InitializeComponent();
            this.AutoLoadChildrenControlData = false;

            this.ContextMenuStrip = this.contextMenu;
            this.CustomBrowserControlType = typeof(SPListOpenBrowser);
            this.OpenBrowserType = OpenBrowserType.Custom;

            this.CellMouseDoubleClick += SPContentDetailsControl_CellMouseDoubleClick;
            this.CellEndEdit += SPContentDetailsControl_CellEndEdit;
            base.CellValidating += SPContentDetailsControl_CellValidating;
        }

        public new event EventHandler<SPDataGridViewCellValidatingEventArgs> CellValidating;
        protected virtual void OnCellValidating(SPDataGridViewCellValidatingEventArgs e)
        {
            if (this.CellValidating != null)
                this.CellValidating(this, e);
        }
        private void SPContentDetailsControl_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            this.OnCellValidating(new SPDataGridViewCellValidatingEventArgs(e, this._oldCellValue));
        }

        private void SPContentDetailsControl_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.ReadOnly = true;
            this._oldCellValue = null;
        }

        private void SPContentDetailsControl_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViewDetails(e.RowIndex);
        }

        private void EditCell()
        {
            this.ReadOnly = false;
            this._oldCellValue = this.CurrentCell.Value;
            this.BeginEdit();
        }

        private void ViewDetails(int rowIndex)
        {
            if (rowIndex < 0)
                return;

            var currentRow = this.Rows[rowIndex];
            this.ShowDialog(string.Format("Details Item: {0}", currentRow.Cells[DefaultSPValues.InternalFieldName.ID].Value)
                , new DetailViewControl() { DataSource = currentRow });
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == this.ct_Details)
            {
                if (this.CurrentCell == null)
                    return;
                ViewDetails(this.CurrentCell.RowIndex);
            }
            else if (e.ClickedItem == this.ct_EditValue)
                this.EditCell();
            else if (this.ct_HideSelectedRows == e.ClickedItem)
            {
                this.HideSelectedRows();
                this.hasHiddenRows = true;
            }
            else if (this.ct_ShowHiddenRows == e.ClickedItem)
            {
                this.ShowAllRows();
                this.hasHiddenRows = false;
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.contextMenu.Enabled = this.SelectedCells.Count > 0;
            this.ct_ShowHiddenRows.Enabled = this.hasHiddenRows;
            this.ct_HideSelectedRows.Enabled = this.SelectedRows.Count > 0;
        }
    }
}
