using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HBD.WinForms.Controls.Core
{
    public interface IOpenBrowserConvertableControl : IOpenBrowserControl
    {
        DataTable GetDataTable();
    }
}
