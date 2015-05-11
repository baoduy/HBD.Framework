using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Events;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("SelectChange")]
    public partial class ViewDataControl : HBDControl, IOpenBrowserConvertableControl, IMultiBrowserControl, IHBDDataGridView
    {
        DataGridViewFilterControl _filterControl;
        public ViewDataControl()
        {
            InitializeComponent();
        }

        [Browsable(true), DefaultValue(true)]
        public virtual bool BrowseControlVisible
        {
            get { return this.multiBrowserControl.Visible; }
            set
            {
                this.multiBrowserControl.Visible = value;
                this.CheckTableControlVisible();
            }
        }

        [Browsable(true), DefaultValue(true)]
        public virtual bool SearchAndFilterVisible
        {
            get { return this.dataGridViewSearchControl.Visible; }
            set
            {
                this.dataGridViewSearchControl.Visible = value;
                this.bt_Filter.Visible = value;
                this.CheckTableControlVisible();
            }
        }

        private void CheckTableControlVisible()
        {
            this.tableLayoutPanel.Visible = this.BrowseControlVisible || this.SearchAndFilterVisible;
        }

        #region IOpenBrowserConvertable
        DataTable _data;
        public DataTable GetDataTable()
        {
            return this._data;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState()]
        public string SourcePath
        {
            get { return this.multiBrowserControl.SourcePath; }
            set { this.multiBrowserControl.SourcePath = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        [ControlPropertyState()]
        public string SourceName
        {
            get { return this.multiBrowserControl.SourceName; }
            set { this.multiBrowserControl.SourceName = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public UserControl BrowserControl
        {
            get { return this.multiBrowserControl.Control; }
        }

        public event EventHandler SelectChange;
        protected virtual void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null)
                this.SelectChange(this, e);
        }
        #endregion

        #region IMultiBrowserControl
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlType
        {
            get { return this.multiBrowserControl.CustomBrowserControlType; }
            set { this.multiBrowserControl.CustomBrowserControlType = value; }
        }

        [DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        [ControlPropertyState]
        public OpenBrowserType OpenBrowserType
        {
            get { return this.multiBrowserControl.OpenBrowserType; }
            set { this.multiBrowserControl.OpenBrowserType = value; }
        }
        #endregion

        #region DataGridView properties
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        [ControlPropertyState]
        public bool ReadOnly
        {
            get { return this.hbdDataGridView1.ReadOnly; }
            set { this.hbdDataGridView1.ReadOnly = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsEditing
        { get { return this.hbdDataGridView1.IsEditing; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewColumnCollection Columns
        { get { return this.hbdDataGridView1.Columns; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewRowCollection Rows
        { get { return this.hbdDataGridView1.Rows; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell CurrentCell
        {
            get { return this.hbdDataGridView1.CurrentCell; }
            set { this.hbdDataGridView1.CurrentCell = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedCellCollection SelectedCells
        { get { return this.hbdDataGridView1.SelectedCells; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedRowCollection SelectedRows
        { get { return this.hbdDataGridView1.SelectedRows; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedColumnCollection SelectedColumns
        {
            get { return this.hbdDataGridView1.SelectedColumns; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DataGridViewSelectionMode SelectionMode
        {
            get { return this.hbdDataGridView1.SelectionMode; }
            set { this.hbdDataGridView1.SelectionMode = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null)]
        public object DataSource
        {
            get { return this.hbdDataGridView1.DataSource; }
            set { this.hbdDataGridView1.DataSource = value; }
        }
        #endregion

        #region Events
        public event DataGridViewCellCancelEventHandler CellBeginEdit
        {
            add { this.hbdDataGridView1.CellBeginEdit += value; }
            remove { this.hbdDataGridView1.CellBeginEdit -= value; }
        }
        public event DataGridViewCellEventHandler CellEndEdit
        {
            add { this.hbdDataGridView1.CellEndEdit += value; }
            remove { this.hbdDataGridView1.CellEndEdit -= value; }
        }
        public event DataGridViewCellEventHandler CellLeave
        {
            add { this.hbdDataGridView1.CellLeave += value; }
            remove { this.hbdDataGridView1.CellLeave -= value; }
        }
        public event DataGridViewCellMouseEventHandler CellMouseClick
        {
            add { this.hbdDataGridView1.CellMouseClick += value; }
            remove { this.hbdDataGridView1.CellMouseClick -= value; }
        }
        public event DataGridViewCellMouseEventHandler CellMouseDoubleClick
        {
            add { this.hbdDataGridView1.CellMouseDoubleClick += value; }
            remove { this.hbdDataGridView1.CellMouseDoubleClick -= value; }
        }
        public event DataGridViewCellMouseEventHandler CellMouseDown
        {
            add { this.hbdDataGridView1.CellMouseDown += value; }
            remove { this.hbdDataGridView1.CellMouseDown -= value; }
        }
        public event DataGridViewCellEventHandler CellMouseEnter
        {
            add { this.hbdDataGridView1.CellMouseEnter += value; }
            remove { this.hbdDataGridView1.CellMouseEnter -= value; }
        }
        public event DataGridViewCellEventHandler CellMouseLeave
        {
            add { this.hbdDataGridView1.CellLeave += value; }
            remove { this.hbdDataGridView1.CellLeave -= value; }
        }
        public event DataGridViewCellMouseEventHandler CellMouseMove
        {
            add { this.hbdDataGridView1.CellMouseMove += value; }
            remove { this.hbdDataGridView1.CellMouseMove -= value; }
        }
        public event DataGridViewCellMouseEventHandler CellMouseUp
        {
            add { this.hbdDataGridView1.CellMouseUp += value; }
            remove { this.hbdDataGridView1.CellMouseUp -= value; }
        }
        public event DataGridViewCellPaintingEventHandler CellPainting
        {
            add { this.hbdDataGridView1.CellPainting += value; }
            remove { this.hbdDataGridView1.CellPainting -= value; }
        }
        public event DataGridViewCellParsingEventHandler CellParsing
        {
            add { this.hbdDataGridView1.CellParsing += value; }
            remove { this.hbdDataGridView1.CellParsing -= value; }
        }
        public event DataGridViewCellStateChangedEventHandler CellStateChanged
        {
            add { this.hbdDataGridView1.CellStateChanged += value; }
            remove { this.hbdDataGridView1.CellStateChanged -= value; }
        }
        public event DataGridViewCellEventHandler CellValidated
        {
            add { this.hbdDataGridView1.CellValidated += value; }
            remove { this.hbdDataGridView1.CellValidated -= value; }
        }
        public event DataGridViewCellValidatingEventHandler CellValidating
        {
            add { this.hbdDataGridView1.CellValidating += value; }
            remove { this.hbdDataGridView1.CellValidating -= value; }
        }
        public event DataGridViewCellEventHandler CellValueChanged
        {
            add { this.hbdDataGridView1.CellValueChanged += value; }
            remove { this.hbdDataGridView1.CellValueChanged -= value; }
        }
        public event DataGridViewCellEventHandler CellErrorTextChanged
        {
            add { this.hbdDataGridView1.CellErrorTextChanged += value; }
            remove { this.hbdDataGridView1.CellErrorTextChanged -= value; }
        }
        public event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded
        {
            add { this.hbdDataGridView1.CellErrorTextNeeded += value; }
            remove { this.hbdDataGridView1.CellErrorTextNeeded -= value; }
        }
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete
        {
            add { this.hbdDataGridView1.DataBindingComplete += value; }
            remove { this.hbdDataGridView1.DataBindingComplete -= value; }
        }
        public event EventHandler DataSourceChanged
        {
            add { this.hbdDataGridView1.DataSourceChanged += value; }
            remove { this.hbdDataGridView1.DataSourceChanged -= value; }
        }
        public event DataGridViewRowsAddedEventHandler RowsAdded
        {
            add { this.hbdDataGridView1.RowsAdded += value; }
            remove { this.hbdDataGridView1.RowsAdded -= value; }
        }
        public event DataGridViewRowsRemovedEventHandler RowsRemoved
        {
            add { this.hbdDataGridView1.RowsRemoved += value; }
            remove { this.hbdDataGridView1.RowsRemoved -= value; }
        }
        #endregion

        #region DataGridView methods
        public bool BeginEdit(bool selectAll = true)
        {
            return this.hbdDataGridView1.BeginEdit(selectAll);
        }

        public bool EndEdit()
        {
            return this.hbdDataGridView1.EndEdit();
        }

        public bool EndEdit(DataGridViewDataErrorContexts contexts)
        {
            return this.hbdDataGridView1.EndEdit(contexts);
        }

        public bool CancelEdit()
        {
            return this.hbdDataGridView1.CancelEdit();
        }
        #endregion

        #region Searchable
        [Browsable(false), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ItemCount { get { return this.Rows.Count; } }

        [Browsable(false), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISearchManager SearchManager
        {
            get { return this.hbdDataGridView1.SearchManager; }
        }

        /// <summary>
        /// Indidate whatever row added or removed
        /// </summary>
        public event EventHandler ItemsChanged
        {
            add { this.hbdDataGridView1.ItemsChanged += value; }
            remove { this.hbdDataGridView1.ItemsChanged -= value; }
        }
        public event EventHandler<SearchlEventArgs> SearchStatusChanged
        {
            add { this.hbdDataGridView1.SearchStatusChanged += value; }
            remove { this.hbdDataGridView1.SearchStatusChanged -= value; }
        }
        public virtual void Search(string keyword)
        {
            this.hbdDataGridView1.Search(keyword);
        }
        #endregion

        #region IFilterable
        public Framework.Data.ColumnItemCollection ColumnItems
        {
            get { return this.hbdDataGridView1.ColumnItems; }
        }

        public event EventHandler ColumnNamesChanged
        {
            add { this.hbdDataGridView1.ColumnNamesChanged += value; }
            remove { this.hbdDataGridView1.ColumnNamesChanged -= value; }
        }

        public void Filter(IFilterClause filterClause)
        {
            this.hbdDataGridView1.Filter(filterClause);
        }
        #endregion

        public virtual void HideAllRows()
        {
            this.hbdDataGridView1.HideAllRows();
        }

        public virtual void HideSelectedRows()
        {
            this.hbdDataGridView1.HideSelectedRows();
        }

        public virtual void ShowAllRows()
        {
            this.hbdDataGridView1.ShowAllRows();
        }

        public override bool ValidateData()
        {
            return this.multiBrowserControl.ValidateData();
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();
            this.multiBrowserControl.CreateChildrenControl();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            if (this.BrowseControlVisible)
                this.multiBrowserControl.LoadControlData();
        }

        private void multiBrowserControl_SelectChange(object sender, EventArgs e)
        {
            this._data = this.multiBrowserControl.GetDataTable();
            this.hbdDataGridView1.DataSource = this._data;

            this.OnSelectChange(e);
        }

        private void bt_Filter_Click(object sender, EventArgs e)
        {
            if (this._filterControl == null)
                this._filterControl = new DataGridViewFilterControl() { FilterableControl = this.hbdDataGridView1 };

            this.ShowDialog("Data Filtering", this._filterControl, 800, 600);
        }
    }
}
