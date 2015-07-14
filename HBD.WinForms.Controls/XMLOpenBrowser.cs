using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data.CSV;
using HBD.Framework.Data.XML;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("SelectChange")]
    public partial class XMLOpenBrowser : FileOpenBrowser, IOpenBrowserConvertableControl
    {
        public XMLOpenBrowser()
        {
            InitializeComponent();
        }

        public XMLOpenBrowser(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public System.Data.DataTable GetDataTable()
        {
            if (!this.ValidateData())
                return null;

            this.Enabled = false;
            using (var adapter = new XMLAdapter(this.SourcePath))
            {
                var data = adapter.ToDataTable();

                this.Enabled = true;
                return data;
            }
        }
    }
}
