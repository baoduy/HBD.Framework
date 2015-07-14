using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Utilities;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls.Events
{
    public class SearchlEventArgs : EventArgs
    {
        public ISearchManager SearchManager { get; set; }

        public SearchlEventArgs(ISearchManager searchManager)
        { this.SearchManager = searchManager; }
    }
}
