using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Data;
using System.Xml.Linq;
using HBD.Framework.Data.XML;
using HBD.Framework.Core;
using HBD.Framework.Data.Utilities;

namespace HBD.Framework.Data.Sharepoint.Server2010
{
    /// <summary>
    /// Sharepoint Server Object Model Adapter
    /// </summary>
    public class SPServerAdapter : SPAdapterBase
    {
        public SPServerAdapter(string siteURL) : base(siteURL) { }

        SPSite _site;
        private string[] _groups = null;

        public override string Title
        {
            get { return this.Site.RootWeb.Title; }
        }

        private SPSite Site
        {
            get
            {
                if (this._site == null)
                    this._site = new SPSite(this.SiteURL);
                return this._site;
            }
        }

        public virtual string[] Groups
        {
            get
            {
                if (_groups == null)
                    _groups = this.Site.RootWeb.Groups.Cast<SPGroup>().Select(g => g.Name).ToArray();
                return _groups;
            }
        }

        protected override string[] GetListTitles()
        {
            return this.Site.RootWeb.Lists.Cast<SPList>().Select(l => l.Title).OrderBy(l => l).ToArray();
        }

        protected override string[] GetSiteGroups()
        {
            return this.Site.RootWeb.SiteGroups.Cast<SPGroup>().Select(g => g.Name).OrderBy(g => g).ToArray();
        }

        public virtual SPList GetList(string listTitle)
        {
            Guard.ArgumentNotNull(listTitle, "SPList Title");
            var web = this.Site.RootWeb;
            return web.Lists.TryGetList(listTitle);
        }

        protected override IDictionary<string, string> GetSubSites()
        {
            var sites = this.Site.RootWeb.Webs.Select(w => new { Title = w.Title, Url = w.ServerRelativeUrl });
            var dic = new Dictionary<string, string>();
            foreach (var s in sites)
                dic.Add(s.Url, s.Title);
            return dic;
        }

        public override bool CanConnect()
        {
            return !string.IsNullOrEmpty(this.Site.RootWeb.Title);
        }

        // Summary:
        //     Executes the specified method with Full Control rights even if the user does
        //     not otherwise have Full Control.
        //
        // Parameters:
        //   secureCode:
        //     A Microsoft.SharePoint.SPSecurity.CodeToRunElevated object that represents
        //     a method that is to run with elevated rights.
        public void RunWithElevatedPrivileges(SPSecurity.CodeToRunElevated secureCode)
        {
            SPSecurity.RunWithElevatedPrivileges(secureCode);
        }

        #region Convert Data
        /// <summary>
        /// Conver list to datatable. If listTitle is null then get the first list on the listTitles
        /// </summary>
        /// <param name="listTitle">ListTitle</param>
        /// <returns>DataTable</returns>
        public override DataTable ToDataTable(string listTitle)
        {
            if (string.IsNullOrEmpty(listTitle) && this.ListTitles != null && this.ListTitles.Length > 0)
                listTitle = this.ListTitles[0];

            return this.ToDataTable(listTitle, null, null);
        }

        public override DataTable ToDataTable(string listTitle, IFilterClause filterClause, params string[] fields)
        {
            Guard.ArgumentNotNull(listTitle, "List Name");

            var data = new DataTable();

            var list = this.GetList(listTitle);

            if (list == null)
                return data;

            if (fields == null && filterClause == null)
                data = list.Items.GetDataTable();
            else
            {
                var render = new SPCamlQueryRender();
                var spQuery = new SPQuery()
                {
                    Query = render.RenderWhereClause(filterClause),
                    ViewFields = render.RenderFields(fields)
                };

                data = list.GetItems(spQuery).GetDataTable();
            }

            return data;
        }

        public override DataSet ToDataSet()
        {
            var data = new DataSet();

            foreach (var l in this.ListTitles)
                data.Tables.Add(this.ToDataTable(l));

            return data;
        }

        private DataTable ConvertToDataTable(string listTitle, SPView view)
        {
            Guard.ArgumentNotNull(view, "SPView");

            var list = this.GetList(listTitle);
            if (list != null)
                return list.GetItems(view).GetDataTable();
            return null;
        }

        public override DataTable ToDataTable(string listTitle, string viewTitle = null)
        {
            if (string.IsNullOrEmpty(viewTitle))
                return this.ToDataTable(listTitle, null, null);

            var list = this.GetList(listTitle);
            var view = this.GetView(list, viewTitle);

            if (list != null && view != null)
                return list.GetItems(view).GetDataTable();
            return null;
        }
        #endregion

        public override string[] GetViewTitles(string listTitle)
        {
            var list = this.GetList(listTitle);

            if (list != null)
            {
                var lv = list.Views.Cast<SPView>().Select(v => v.Title).ToList();
                lv.Sort();
                return lv.ToArray();
            }
            return new string[] { };
        }

        private SPView GetView(SPList list, string viewTitle)
        {
            Guard.ArgumentNotNull(list, "SPList");
            return list.Views.Cast<SPView>().FirstOrDefault(v => v.Title.Equals(viewTitle, StringComparison.CurrentCultureIgnoreCase));
        }

        private SPView GetView(string listTitle, string viewTitle)
        {
            var list = this.GetList(listTitle);
            if (list != null)
                return GetView(list, viewTitle);
            return null;
        }


        //private string[] GetUserNames(SPGroup group)
        //{
        //    Guard.ArgumentNotNull(group, "SPGroup");

        //    var list = new List<string>();
        //    foreach (SPUser u in group.Users)
        //    {
        //        if (!list.Any(x => x == u.LoginName))
        //            list.Add(u.LoginName);
        //    }
        //    return list.ToArray();
        //}

        //private string[] GetAllUserNames(SPGroupCollection groups)
        //{
        //    var list = new List<string>();
        //    foreach (SPGroup g in groups)
        //        list.AddRange(this.GetUserNames(g));
        //    return list.ToArray();
        //}

        /// <summary>
        /// Get all acount is pacticular group
        /// </summary>
        /// <param name="groupName">Group name</param>
        /// <returns>Accounts</returns>
        public override string[] GetUserNames(string groupName)
        {
            var data = this.AllUsersToDataTable(groupName);
            return (from DataRow row in data.Rows
                    select row[SPStaticProperties.UserInternalFieldNames.LoginName] as string).ToArray();
        }

        /// <summary>
        /// Get All Accounts in RootWeb.Groups
        /// </summary>
        /// <returns>Accounts</returns>
        public override string[] GetAllUserNames()
        {
            var data = this.AllUsersToDataTable();
            return (from DataRow row in data.Rows
                    select row[SPStaticProperties.UserInternalFieldNames.LoginName] as string).ToArray();
        }

        public override DataTable AllUsersToDataTable(string groupName)
        {
            var data = new DataTable();
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.ID);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.LoginName);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Title);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Email);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.PrincipalType);

            var group = this.Site.RootWeb.Groups.Cast<SPGroup>().FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            if (group != null)
            {
                foreach (SPUser u in group.Users)
                    data.Rows.Add(new object[] { u.ID, u.LoginName, u.Name, u.Email, "User" });
            }
            return data;
        }

        public override DataTable AllUsersToDataTable()
        {
            var data = new DataTable();
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.ID);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.LoginName);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Title);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Email);
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.PrincipalType);

            foreach (SPGroup gr in this.Site.RootWeb.SiteGroups)
            {
                foreach (SPUser u in gr.Users)
                    data.Rows.Add(new object[] { u.ID, u.LoginName, u.Name, u.Email, "User" });
            }
            return data;
        }

        /// <summary>
        /// Get All Accounts in RootWeb.SiteGroups
        /// </summary>
        /// <returns>Accounts</returns>
        //public virtual string[] GetAllAccountsInSiteGroups()
        //{
        //    return GetAllUserNames(this.Site.RootWeb.SiteGroups);
        //}


        private bool Update(SPItem item, string fieldName, object value)
        {
            Guard.ArgumentNotNull(item, "SPItem");
            Guard.ArgumentNotNull(fieldName, "fieldName");

            if (item.Fields.ContainsField(fieldName))
            {
                item[fieldName] = value;
                item.Update();
                return true;
            }
            return false;
        }

        private bool Update(SPList list, int itemID, string fieldName, object value)
        {
            Guard.ArgumentNotNull(list, "SPList");
            Guard.GreaterThanOrEqualsZero(itemID, "itemID");

            var item = list.GetItemById(itemID);
            if (item != null)
                return this.Update(item, fieldName, value);
            return false;
        }

        public override bool Update(string listTitle, int itemID, string fieldName, object value)
        {
            var list = this.GetList(listTitle);
            if (list != null)
                return this.Update(list, itemID, fieldName, value);
            return false;
        }


        public override void Dispose()
        {
            base.Dispose();

            if (this._site != null)
            {
                this._site.Close();
                this._site.Dispose();
                this._site = null;
            }
        }

        public override Dictionary<string, object> GetItemByID(string listTitle, int itemID)
        {
            var list = this.GetList(listTitle);
            if (list == null)
                return null;

            var item = list.GetItemById(itemID);
            if (item == null)
                return null;

            var result = new Dictionary<string, object>();
            foreach (SPField f in list.Fields)
                result.Add(f.Title, item[f.Id]);
            return result;
        }
    }
}
