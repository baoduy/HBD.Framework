using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Events;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Data;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Core
{
    public interface IFilterableControl<TFilterClause> : IHBDControl where TFilterClause : IFilterClause
    {
        //public object DataSource { get; set; }

        /// <summary>
        /// List The names of Columns
        /// </summary>
        ColumnItemCollection ColumnItems { get; }
        /// <summary>
        /// Event when number of columns int the list/grid changed.
        /// </summary>
        event EventHandler ColumnNamesChanged;

        void Filter(IFilterClause filterClause);
    }
}
