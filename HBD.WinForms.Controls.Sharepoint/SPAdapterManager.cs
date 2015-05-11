using HBD.Framework.Data.Sharepoint;
using HBD.Framework.Data.Sharepoint.Client2010;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.WinForms.Controls.Sharepoint
{
    public class SPAdapterManager
    {
        public static ISPAdapter Open(string url)
        {
            try
            {
                //if (Microsoft.SharePoint.SPContext.Current == null)
                return new SPClientAdapter(url);
                //return new SPServerAdapter(url);
            }
            catch { return new SPClientAdapter(url); }
        }
    }
}
