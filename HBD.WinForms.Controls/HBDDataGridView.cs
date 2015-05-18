using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using HBD.Framework.Core;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Utilities;
using HBD.WinForms.Controls.Events;
using System.Data;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Extension.WinForms;
using HBD.Framework.Log;
using HBD.Framework.Data;

namespace HBD.WinForms.Controls
{
    public class HBDDataGridView : DataGridView, IHBDDataGridView
    {
        public HBDDataGridView()
        {
            this.DoubleBuffered = true;
            this.ShowRowNumbers = true;
            this.AutoSwitchSelectionMode = true;
        }

        bool _isRowHeaderClicked = false;
        bool _isColumnHeaderClicked = false;

        #region Properties
        [DefaultValue(true)]
        public bool ShowRowNumbers { get; set; }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(DataGridViewSelectionMode.CellSelect)]
        public virtual new DataGridViewSelectionMode SelectionMode
        {
            get { return base.SelectionMode; }
            set
            {
                if (base.SelectionMode != value)
                {
                    if (value == DataGridViewSelectionMode.FullColumnSelect && this.ColumnSortMode == DataGridViewColumnSortMode.Automatic)
                        return;

                    base.SelectionMode = value;
                    this.OnSelectionModeChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public virtual bool AutoSwitchSelectionMode { get; set; }

        [Browsable(false), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditing { get; protected set; }

        DataGridViewColumnSortMode _columnSortMode = DataGridViewColumnSortMode.Automatic;
        /// <summary>
        /// Enable the AllowUserToOrderColumns and to enable the ColumnSortMode.
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(DataGridViewColumnSortMode.Automatic)]
        public virtual DataGridViewColumnSortMode ColumnSortMode
        {
            get
            {
                if (!this.AllowUserToOrderColumns)
                    return DataGridViewColumnSortMode.NotSortable;
                return _columnSortMode;
            }
            set { _columnSortMode = value; }
        }

        public event EventHandler SelectionModeChanged;
        protected virtual void OnSelectionModeChanged(EventArgs e)
        {
            if (this.SelectionModeChanged != null)
                this.SelectionModeChanged(this, e);
        }

        #endregion

        #region Searchable
        [Browsable(false), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ItemCount { get { return this.RowCount; } }

        ISearchManager _searchManager;
        public ISearchManager SearchManager
        {
            get
            {
                if (this._searchManager == null)
                {
                    this._searchManager = new DataGridViewSearchManager(this);
                    this._searchManager.StatusChanged += _searchManager_StatusChanged;
                }
                return this._searchManager;
            }
        }

        private void _searchManager_StatusChanged(object sender, EventArgs e)
        {
            this.OnSearchStatusChanged(new SearchlEventArgs(this.SearchManager));
        }

        /// <summary>
        /// Indidate whatever row added or removed
        /// </summary>
        public event EventHandler ItemsChanged;
        protected virtual void OnItemsChanged(EventArgs e)
        {
            if (this.ItemsChanged != null)
                this.ItemsChanged(this, e);
        }

        public event EventHandler<SearchlEventArgs> SearchStatusChanged;
        public virtual void OnSearchStatusChanged(SearchlEventArgs e)
        {
            if (this.SearchStatusChanged != null)
                this.SearchStatusChanged(this, e);
        }

        public virtual void Search(string keyword)
        {
            this.SearchManager.Search(keyword);
        }
        #endregion

        #region Public methods
        public virtual new void ClearSelection()
        {
            this.CurrentCell = null;
            base.ClearSelection();
        }

        public virtual void HideAllRows()
        {
            this.ClearSelection();
            foreach (DataGridViewRow row in this.Rows)
                row.Visible = false;
        }

        public virtual void HideSelectedRows()
        {
            var rows = this.SelectedRows;
            this.ClearSelection();

            foreach (DataGridViewRow r in rows)
                r.Visible = false;
        }

        public virtual void HideEmptyFormatRows()
        {
            this.ClearSelection();

            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.DefaultCellStyle.BackColor != Color.Empty)
                    continue;

                row.Visible = false;
                foreach (DataGridViewColumn col in this.Columns)
                {
                    if (row.Cells[col.Name].Style.BackColor != Color.Empty)
                    {
                        row.Visible = true;
                        break;
                    }
                }
            }
        }

        public virtual void ShowAllRows()
        {
            foreach (DataGridViewRow row in this.Rows)
                if (!row.Visible)
                    row.Visible = true;
        }


        public virtual int GetDisplayRowIndex(int rowIndex)
        {
            int i = -1;
            foreach (DataGridViewRow row in this.Rows)
            {
                if (row.Visible)
                    i++;
                if (row.Index == rowIndex)
                    return i;
            }
            return -1;
        }

        public virtual int GetDisplayColumnIndex(int columnIndex)
        {
            return this.Columns[columnIndex].DisplayIndex;
        }

        public virtual DataGridViewRow GetDisplayRow(int displayRowIndex)
        {
            int i = 0;
            foreach (DataGridViewRow row in this.Rows)
            {
                if (!row.Visible)
                    continue;

                if (i == displayRowIndex)
                    return row;
                i++;
            }

            return null;
        }

        public virtual DataGridViewColumn GetDisplayColumn(int displayColumnIndex)
        {
            foreach (DataGridViewColumn col in this.Columns)
            {
                if (col.Visible && col.DisplayIndex == displayColumnIndex)
                    return col;
            }

            return null;
        }

        public virtual DataGridViewRow GetDisplayRowByIndex(int rowIndex)
        {
            var disIndex = this.GetDisplayRowIndex(rowIndex);
            if (disIndex >= 0)
                return this.GetDisplayRow(disIndex);
            return null;
        }

        public virtual DataGridViewColumn GetDisplayColumnByIndex(int columnIndex)
        {
            var disIndex = this.GetDisplayColumnIndex(columnIndex);
            if (disIndex >= 0)
                return this.GetDisplayColumn(disIndex);
            return null;
        }

        public virtual DataGridViewCell GetDisplayCell(int displayRowIndex, int displayColumnIndex)
        {
            var row = this.GetDisplayRow(displayRowIndex);
            if (row == null)
                return null;

            var col = this.GetDisplayColumn(displayColumnIndex);
            if (col == null)
                return null;

            return row.Cells[col.Name];
        }

        public virtual DataGridViewCell GetDisplayCellByIndex(int rowIndex, int columnIndex)
        {
            var disRowIndex = this.GetDisplayRowIndex(rowIndex);
            if (disRowIndex < 0)
                return null;

            var disColIndex = this.GetDisplayColumnIndex(columnIndex);
            if (disColIndex < 0)
                return null;

            return GetDisplayCell(disRowIndex, disColIndex);
        }

        public virtual void SyncSelectedTo(HBDDataGridView syncGrid)
        {
            Guard.ArgumentNotNull(syncGrid, "syncGrid");

            var originalOfAutoSwitchSelectionMode = syncGrid.AutoSwitchSelectionMode;
            syncGrid.AutoSwitchSelectionMode = false;
            syncGrid.SelectionMode = this.SelectionMode;
            syncGrid.ClearSelection();

            switch (this.SelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect:
                    {
                        //Cell Selection Mode
                        for (int i = 0; i < this.SelectedCells.Count; i++)
                        {
                            var cell = this.SelectedCells[i];

                            var disRowIndex = this.GetDisplayRowIndex(cell.RowIndex);
                            var disColIndex = this.GetDisplayColumnIndex(cell.ColumnIndex);

                            var syncCell = syncGrid.GetDisplayCell(disRowIndex, disColIndex);
                            if (syncCell != null)
                                syncCell.Selected = true;
                        }
                    } break;
                case DataGridViewSelectionMode.FullRowSelect:
                    {
                        foreach (DataGridViewRow row in this.SelectedRows)
                        {
                            var displayRowIndex = this.GetDisplayRowIndex(row.Index);
                            var syncRow = syncGrid.GetDisplayRow(displayRowIndex);
                            if (syncRow != null)
                                syncRow.Selected = true;
                        }
                    } break;
                case DataGridViewSelectionMode.FullColumnSelect:
                    {
                        foreach (DataGridViewColumn col in this.SelectedColumns)
                        {
                            var displayIndex = this.GetDisplayColumnIndex(col.Index);
                            var syncCol = syncGrid.GetDisplayColumn(displayIndex);
                            if (syncCol != null)
                                syncCol.Selected = true;
                        }
                    } break;
            }

            syncGrid.AutoSwitchSelectionMode = originalOfAutoSwitchSelectionMode;
        }

        public virtual void SyncScrolledPositionTo(HBDDataGridView syncGrid)
        {
            Guard.ArgumentNotNull(syncGrid, "syncGrid");

            var disRowIndex = this.GetDisplayRowIndex(this.FirstDisplayedScrollingRowIndex);
            var disColIndex = this.GetDisplayColumnIndex(this.FirstDisplayedScrollingColumnIndex);

            var row = syncGrid.GetDisplayRowByIndex(disRowIndex);
            var col = syncGrid.GetDisplayColumn(disColIndex);

            if (row != null)
                syncGrid.FirstDisplayedScrollingRowIndex = row.Index;

            if (col != null)
                syncGrid.FirstDisplayedScrollingColumnIndex = col.Index;
        }
        #endregion

        #region Override Events
        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            base.OnColumnAdded(e);

            e.Column.SortMode = this.ColumnSortMode;

            if (this._columnItems != null)
                this._columnItems.Clear();

            this.OnColumnNamesChanged(EventArgs.Empty);
        }

        protected override void OnColumnRemoved(DataGridViewColumnEventArgs e)
        {
            base.OnColumnRemoved(e);
            this.ColumnItems.Remove(e.Column.Name);
            this.OnColumnNamesChanged(EventArgs.Empty);
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (this.ShowRowNumbers)
            {
                //this method overrides the DataGridView's RowPostPaint event 
                //in order to automatically draw numbers on the row header cells
                //and to automatically adjust the width of the column containing
                //the row header cells so that it can accommodate the new row
                //numbers,

                //store a string representation of the row number in 'strRowNumber'
                string strRowNumber = (e.RowIndex + 1).ToString();

                //prepend leading zeros to the string if necessary to improve
                //appearance. For example, if there are ten rows in the grid,
                //row seven will be numbered as "07" instead of "7". Similarly, if 
                //there are 100 rows in the grid, row seven will be numbered as "007".
                while (strRowNumber.Length < this.RowCount.ToString().Length) strRowNumber = "0" + strRowNumber;

                //determine the display size of the row number string using
                //the DataGridView's current font.
                SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);

                //adjust the width of the column that contains the row header cells 
                //if necessary
                if (this.RowHeadersWidth < (int)(size.Width + 20)) this.RowHeadersWidth = (int)(size.Width + 20);

                //this brush will be used to draw the row number string on the
                //row header cell using the system's current ControlText color
                Brush b = SystemBrushes.ControlText;

                //draw the row number string on the current row header cell using
                //the brush defined above and the DataGridView's default font
                e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));

                //call the base object's OnRowPostPaint method
            }

            base.OnRowPostPaint(e);
        }

        protected override void OnRowHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.AutoSwitchSelectionMode && !this.AllowUserToOrderColumns)
            {
                _isRowHeaderClicked = true;
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                if (e.RowIndex >= 0)
                    this.Rows[e.RowIndex].Selected = true;
            }
            base.OnRowHeaderMouseClick(e);
        }

        protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.AutoSwitchSelectionMode)
            {
                _isColumnHeaderClicked = true;
                this.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
                if (e.ColumnIndex >= 0)
                    this.Columns[e.ColumnIndex].Selected = true;
            }
            base.OnColumnHeaderMouseClick(e);
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.AutoSwitchSelectionMode)
            {
                if (!_isRowHeaderClicked && !_isColumnHeaderClicked)
                {
                    this.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                        this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }

            base.OnCellMouseClick(e);

            _isRowHeaderClicked = false;
            _isColumnHeaderClicked = false;

            if (this.SearchManager.Status == SearchStatus.Completed)
                this.SearchManager.SetCurrentIndex(this.CurrentCell);
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            base.OnCellBeginEdit(e);
            this.IsEditing = true;
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            base.OnCellEndEdit(e);
            this.IsEditing = false;
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
            this.SearchManager.Reset();
        }

        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            this.OnItemsChanged(e);
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            base.OnRowsRemoved(e);
            this.OnItemsChanged(e);
        }
        #endregion

        #region IFilterable
        ColumnItemCollection _columnItems = null;
        public ColumnItemCollection ColumnItems
        {
            get
            {
                if (this._columnItems == null)
                    this._columnItems = new ColumnItemCollection();
                if (this._columnItems.Count == 0 && this.ColumnCount > 0)
                {
                    foreach (DataGridViewColumn col in this.Columns)
                        this._columnItems.Add(col.Name, col.ValueType);
                }
                return this._columnItems;
            }
        }

        public event EventHandler ColumnNamesChanged;
        protected virtual void OnColumnNamesChanged(EventArgs e)
        {
            if (this.ColumnNamesChanged != null)
                this.ColumnNamesChanged(this, e);
        }

        public void Filter(IFilterClause filterClause)
        {
            if (this.DataSource != null)
            {
                try
                {
                    var filterString = new DatatableFilterRender().RenderFilter(filterClause);
                    if (this.DataSource is DataTable)
                        (this.DataSource as DataTable).DefaultView.RowFilter = filterString;
                    else if (this.DataSource is BindingSource)
                        (this.DataSource as BindingSource).Filter = filterString;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else this.FilterRows(filterClause);
        }
        #endregion
    }
}
