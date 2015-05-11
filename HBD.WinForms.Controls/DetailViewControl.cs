using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Events;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Data;

namespace HBD.WinForms.Controls
{
    public partial class DetailViewControl : HBDControl, IHBDDataGridView
    {
        public DetailViewControl()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(null), Browsable(false)]
        public virtual object DataSource { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true), Browsable(true)]
        public bool SearchPanelVisuble
        { get { return this.groupBox1.Visible; }
            set { this.groupBox1.Visible = value; }
        }

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
        { get { return this.hbdDataGridView1.SelectedColumns; } }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DataGridViewSelectionMode SelectionMode
        {
            get { return this.hbdDataGridView1.SelectionMode; }
            set { this.hbdDataGridView1.SelectionMode = value; }
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
        public ColumnItemCollection ColumnItems
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

        public override void LoadControlData()
        {
            base.LoadControlData();
            if (this.DataSource == null)
                return;

            this.SuspendLayout();

            if (this.DataSource is DataRow)
            {
                var row = this.DataSource as DataRow;
                foreach (DataColumn col in row.Table.Columns)
                    this.Add(col.ColumnName, row[col]);
            }
            else if (this.DataSource is DataGridViewRow)
            {
                var row = this.DataSource as DataGridViewRow;
                foreach (DataGridViewColumn col in row.DataGridView.Columns)
                {
                    if (!col.Visible)
                        continue;
                    var addedIndex = this.Add(col.Name, row.Cells[col.Name].Value);
                    if (row.DataGridView.CurrentCell != null)
                    {
                        if (row.DataGridView.CurrentCell.ColumnIndex == col.Index)
                            this.hbdDataGridView1.CurrentCell = this.Rows[addedIndex].Cells[1];
                    }
                }
            }

            this.ResumeLayout(false);
        }

        private int Add(string title, object value)
        {
            return this.hbdDataGridView1.Rows.Add(new object[] { title, value });
        }
    }
}
