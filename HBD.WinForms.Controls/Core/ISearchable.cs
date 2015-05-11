using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Events;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Core
{
    public interface ISearchable : IHBDControl
    {
        ISearchManager SearchManager { get; }
        int ItemCount { get; }
        event EventHandler<SearchlEventArgs> SearchStatusChanged;
        /// <summary>
        /// Event when number of items in the List/Gird changed
        /// </summary>
        event EventHandler ItemsChanged;
        void Search(string keyword);
    }
}
