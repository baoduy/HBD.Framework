using System;
using HBD.WinForms.Controls.Utilities;
using HBD.WinForms.Controls.Events;
using System.Windows.Forms;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Core
{
    public interface IHBDDataGridView : ISearchable, IFilterableControl<FilterClause>, IHBDControl
    {
        event DataGridViewCellCancelEventHandler CellBeginEdit;
        event DataGridViewCellEventHandler CellEndEdit;
        event DataGridViewCellEventHandler CellErrorTextChanged;
        event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded;
        event DataGridViewCellEventHandler CellLeave;
        event DataGridViewCellMouseEventHandler CellMouseClick;
        event DataGridViewCellMouseEventHandler CellMouseDoubleClick;
        event DataGridViewCellMouseEventHandler CellMouseDown;
        event DataGridViewCellEventHandler CellMouseEnter;
        event DataGridViewCellEventHandler CellMouseLeave;
        event DataGridViewCellMouseEventHandler CellMouseMove;
        event DataGridViewCellMouseEventHandler CellMouseUp;
        event DataGridViewCellPaintingEventHandler CellPainting;
        event DataGridViewCellParsingEventHandler CellParsing;
        event DataGridViewCellStateChangedEventHandler CellStateChanged;
        event DataGridViewCellEventHandler CellValidated;
        event DataGridViewCellValidatingEventHandler CellValidating;
        event DataGridViewCellEventHandler CellValueChanged;
        event DataGridViewBindingCompleteEventHandler DataBindingComplete;
        event DataGridViewRowsAddedEventHandler RowsAdded;
        event DataGridViewRowsRemovedEventHandler RowsRemoved;
        event EventHandler DataSourceChanged;

        DataGridViewColumnCollection Columns { get; }
        DataGridViewCell CurrentCell { get; set; }
        bool EndEdit();
        bool EndEdit(DataGridViewDataErrorContexts contexts);
        bool IsEditing { get; }
        bool ReadOnly { get; set; }
        DataGridViewRowCollection Rows { get; }
        DataGridViewSelectedCellCollection SelectedCells { get; }
        DataGridViewSelectedColumnCollection SelectedColumns { get; }
        DataGridViewSelectedRowCollection SelectedRows { get; }
        DataGridViewSelectionMode SelectionMode { get; set; }
        object DataSource { get; set; }
    }
}
