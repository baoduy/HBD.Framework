using HBD.Framework.Data.Utilities;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.Data.Sharepoint.Client2010
{
    public class SPClientCamlQueryRender : SPCamlQueryRender
    {
        public virtual CamlQuery RenderCamlQuery(IFilterClause filter, params string[] fields)
        {
            return new CamlQuery() { ViewXml = RenderViewXml(filter, fields) };
        }

        public virtual CamlQuery RenderCamlQuery(View view)
        {
            view.Context.Load(view.ViewFields);
            view.Context.ExecuteQuery();

            var filedsString = this.RenderViewFields(view.ViewFields.ToArray());
            return new CamlQuery() { ViewXml = string.Format(SPCamlQueryRender.ViewFormat, filedsString + string.Format(SPCamlQueryRender.QueryFormat, view.ViewQuery)) };
        }
    }
}
