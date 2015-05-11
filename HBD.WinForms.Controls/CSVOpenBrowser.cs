using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data.CSV;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("SelectChange")]
    public partial class CSVOpenBrowser : FileOpenBrowser, IOpenBrowserConvertableControl
    {
        public CSVOpenBrowser()
        {
            InitializeComponent();
        }

        public CSVOpenBrowser(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public System.Data.DataTable GetDataTable()
        {
            if (!this.ValidateData())
                return null;

            this.Enabled = false;
            using (var adapter = new CSVAdapter(this.SourcePath))
            {
               var data = adapter.ToDataTable();

               this.Enabled = true;
               return data;
            }
        }
    }
}
