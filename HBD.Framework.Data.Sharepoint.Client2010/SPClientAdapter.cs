using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using HBD.Framework.Core;
using System.Data;
using HBD.Framework.Data.Utilities;
using HBD.Framework.Log;
using Microsoft.SharePoint.Client.Utilities;
using System.Net;

namespace HBD.Framework.Data.Sharepoint.Client2010
{
    /// <summary>
    /// Sharepoint Client Object Model Adapter
    /// </summary>
    public class SPClientAdapter : SPAdapterBase
    {
        public SPClientAdapter(string siteURL) : this(siteURL, null, null) { }
        public SPClientAdapter(string siteURL, string loginName, string password) : base(siteURL) { }

        public string LoginName { get; set; }
        public string Password { get; set; }

        string _title = null;
        public override string Title
        {
            get
            {
                if (string.IsNullOrEmpty(this._title))
                    this.CanConnect();
                return this._title;
            }
        }

        ClientContext _site;
        private ClientContext Site
        {
            get
            {
                if (this._site == null)
                {
                    this._site = new ClientContext(this.SiteURL);

                    if (!string.IsNullOrEmpty(this.LoginName) && !string.IsNullOrEmpty(this.Password))
                        this._site.Credentials = new NetworkCredential(this.LoginName, this.Password);

                }
                return this._site;
            }
        }

        SPClientCamlQueryRender queryRender = null;
        private SPClientCamlQueryRender QueryRender
        {
            get
            {
                if (this.queryRender == null)
                    this.queryRender = new SPClientCamlQueryRender();
                return queryRender;
            }
        }

        protected override string[] GetListTitles()
        {
            var web = this.Site.Web;
            var result = this.Site.LoadQuery(this.Site.Web.Lists.Include(list => list.Title, list => list.Id));
            this.Site.ExecuteQuery();
            return result.Select(l => l.Title).OrderBy(l => l).ToArray();
        }

        protected override string[] GetSiteGroups()
        {
            var groups = this.Site.LoadQuery(this.Site.Web.SiteGroups.Include(g => g.Title, g => g.Id));
            this.Site.ExecuteQuery();

            return groups.Select(g => g.Title).OrderBy(g => g).ToArray();
        }

        protected override IDictionary<string, string> GetSubSites()
        {
            var webs = this.Site.LoadQuery(this.Site.Web.Webs.Include(web => web.Title, web => web.ServerRelativeUrl));
            this.Site.ExecuteQuery();
            var dic = new Dictionary<string, string>();
            foreach (var w in webs)
                dic.Add(w.ServerRelativeUrl, w.Title);
            return dic;
        }

        public virtual List GetList(string listTitle)
        {
            Guard.ArgumentNotNull(listTitle, "List Title");
            var list = this.Site.Web.Lists.GetByTitle(listTitle);
            this.Site.Load(list, l => l.Title, l => l.Id, l => l.ItemCount, l => l.Fields);
            this.Site.ExecuteQuery();
            return list;
        }

        public virtual View GetView(List list, string viewTitle)
        {
            Guard.ArgumentNotNull(list, "SPClient.List");
            var views = list.Context.LoadQuery(list.Views.Include(v => v.Title, v => v.ViewQuery, v => v.ViewFields, v => v.ViewData));
            list.Context.ExecuteQuery();
            return views.FirstOrDefault(v => v.Title == viewTitle);
        }

        public override string[] GetViewTitles(string listTitle)
        {
            var list = this.GetList(listTitle);
            if (list != null)
            {
                var views = list.Context.LoadQuery(list.Views.Include(v => v.Title));
                list.Context.ExecuteQuery();
                return views.Select(v => v.Title).OrderBy(t => t).ToArray();
            }
            return new string[] { };
        }

        private ListItem GetListItemByID(string listTitle, int itemID)
        {
            Guard.ArgumentNotNull(listTitle, "List Title");
            Guard.GreaterThanOrEqualsZero(itemID, "ItemID");

            var list = this.GetList(listTitle);
            if (list == null)
                return null;

            var item = list.GetItemById(itemID);
            list.Context.Load(item);
            list.Context.ExecuteQuery();

            return item;
        }

        public override bool Update(string listTitle, int itemID, string fieldName, object value)
        {
            var item = this.GetListItemByID(listTitle, itemID);
            if (item == null)
                return false;

            item[fieldName] = value;
            item.Update();
            item.Context.ExecuteQuery();
            return true;
        }

        public override Dictionary<string, object> GetItemByID(string listTitle, int itemID)
        {
            var item = this.GetListItemByID(listTitle, itemID);
            if (item == null)
                return null;
            return item.FieldValues;
        }

        public override bool CanConnect()
        {
            //Try to load web title as to check the connection
            try
            {
                this.Site.Load(this.Site.Web, w => w.Title);
                this.Site.ExecuteQuery();
                this._title = this.Site.Web.Title;
                return true;
            }
            catch (Exception ex)
            {
                this._title = ex.Message;
                LogManager.Write(ex);
                return false;
            }
        }

        protected virtual ListItemCollection LoadListItems(List list, CamlQuery query)
        {
            Guard.ArgumentNotNull(list, "SP.Client.List");
            var items = list.GetItems(query);
            list.Context.Load(items);
            list.Context.ExecuteQuery();
            return items;
        }

        public static DataTable GetTableSchema(List list)
        {
            var data = new DataTable() { TableName = list.Title };
            if (list == null || list.Fields.Count == 0) return data;

            foreach (var f in list.Fields)
                data.Columns.Add(f.InternalName, GetSystemType(f.FieldTypeKind));
            return data;
        }

        private static Type GetSystemType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Boolean:
                    return typeof(bool);

                case FieldType.Currency:
                    return typeof(double);
                case FieldType.DateTime:
                    return typeof(DateTime);
                case FieldType.Guid:
                    return typeof(Guid);
                case FieldType.MaxItems:
                case FieldType.Number:
                    return typeof(decimal);
                case FieldType.ThreadIndex:
                case FieldType.Integer:
                    return typeof(int);
                case FieldType.URL:
                case FieldType.User:
                case FieldType.WorkflowStatus:
                case FieldType.Text:
                case FieldType.Note:
                case FieldType.Lookup:
                case FieldType.Choice:
                    return typeof(string);
                case FieldType.ModStat:
                case FieldType.MultiChoice:
                case FieldType.Threading:
                case FieldType.WorkflowEventType:
                case FieldType.PageSeparator:
                case FieldType.Recurrence:
                case FieldType.Invalid:
                case FieldType.Error:
                case FieldType.File:
                case FieldType.GridChoice:
                case FieldType.AllDayEvent:
                case FieldType.Attachments:
                case FieldType.Calculated:
                case FieldType.Computed:
                case FieldType.ContentTypeId:
                case FieldType.Counter:
                case FieldType.CrossProjectLink:
                default: return typeof(object);
            }
        }

        private void ConvertToDataTable(string listTitle, ListItemCollection items, ref DataTable data)
        {
            if (data == null)
            {
                data = new DataTable() { TableName = listTitle };
            }

            if (items == null || items.Count == 0) return;

            foreach (var field in items[0].FieldValues)
            {
                var type = field.Value == null ? typeof(string) : field.Value.GetType();

                if (type.FullName == "Microsoft.SharePoint.Client.FieldLookupValue"
                    || type.FullName == "Microsoft.SharePoint.Client.FieldUserValue")
                {
                    type = typeof(string);
                }
                data.Columns.Add(field.Key, type);
            }

            foreach (var item in items)
            {
                DataRow row = data.NewRow();

                foreach (var obj in item.FieldValues)
                {
                    if (obj.Value != null)
                    {
                        string type = obj.Value.GetType().FullName;

                        if (type == "Microsoft.SharePoint.Client.FieldLookupValue")
                        {
                            row[obj.Key] = ((FieldLookupValue)obj.Value).LookupValue;
                        }
                        else if (type == "Microsoft.SharePoint.Client.FieldUserValue")
                        {
                            row[obj.Key] = ((FieldUserValue)obj.Value).LookupValue;
                        }
                        else
                        {
                            row[obj.Key] = obj.Value;
                        }
                    }
                    else
                    {
                        row[obj.Key] = null;
                    }
                }

                data.Rows.Add(row);
            }
        }
        public override DataTable ToDataTable(string listTitle, IFilterClause filter, params string[] fields)
        {
            Guard.ArgumentNotNull(listTitle, "List Title");
            var data = new DataTable();
            var list = this.GetList(listTitle);

            if (list == null)
                return data;

            var camQuery = this.QueryRender.RenderCamlQuery(filter, fields);
            var items = LoadListItems(list, camQuery);
            
            var dataTable = GetTableSchema(list);
            ConvertToDataTable(listTitle, items, ref dataTable);
            return dataTable;
        }
        public override DataTable ToDataTable(string listTitle, string viewTitle = null)
        {
            if (string.IsNullOrEmpty(viewTitle))
                return this.ToDataTable(listTitle, (IFilterClause)null, (string[])null);

            var list = this.GetList(listTitle);
            var view = this.GetView(list, viewTitle);
            var items = this.LoadListItems(list, this.QueryRender.RenderCamlQuery(view));
            
            var dataTable = GetTableSchema(list);
            ConvertToDataTable(listTitle, items, ref dataTable);
            return dataTable;
        }
        public override DataTable ToDataTable(string listTitle = null)
        {
            if (string.IsNullOrEmpty(listTitle) && this.ListTitles != null && this.ListTitles.Length > 0)
                listTitle = this.ListTitles[0];

            return this.ToDataTable(listTitle, string.Empty);
        }

        protected virtual IEnumerable<User> GetUsersInGroup(string groupName)
        {
            var groups = this.Site.LoadQuery(this.Site.Web.SiteGroups.Include(g => g.Title, g => g.Id));
            this.Site.ExecuteQuery();
            var group = groups.FirstOrDefault(g => g.Title == groupName);
            if (group != null)
            {
                var users = this.Site.LoadQuery(group.Users);
                this.Site.ExecuteQuery();
                return users;
            }
            return null;
        }
        protected virtual IEnumerable<User> GetAllUsers()
        {
            var groups = this.Site.LoadQuery(this.Site.Web.SiteGroups.Include(g => g.Users));
            this.Site.ExecuteQuery();
            foreach (var g in groups)
                foreach (var u in g.Users)
                    yield return u;
        }
        protected virtual DataTable UsersToDataTable(string groupName, IEnumerable<User> users)
        {
            var data = new DataTable() { TableName = groupName };

            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.ID, typeof(int));
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.LoginName, typeof(string));
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Title, typeof(string));
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.Email, typeof(string));
            data.Columns.Add(SPStaticProperties.UserInternalFieldNames.PrincipalType, typeof(PrincipalType));

            foreach (var u in users)
            {
                if (u.PrincipalType != PrincipalType.User)
                    continue;
                data.Rows.Add(new object[] { u.Id, u.LoginName, u.Title, u.Email, u.PrincipalType });
            }

            return data;
        }

        public override string[] GetUserNames(string groupName)
        {
            var users = GetUsersInGroup(groupName);
            if (users != null)
                return users.Select(u => u.LoginName).ToArray();
            return new string[] { };
        }
        public override string[] GetAllUserNames()
        {
            var users = GetAllUsers();
            if (users != null)
                return users.Select(u => u.LoginName).ToArray();
            return new string[] { };
        }

        public override DataTable AllUsersToDataTable(string groupName)
        {
            var users = GetUsersInGroup(groupName);
            if (users != null)
                return UsersToDataTable(groupName, users);
            return new DataTable();
        }

        public override DataTable AllUsersToDataTable()
        {
            var users = this.GetAllUsers();
            if (users != null)
                return this.UsersToDataTable(this.Title, users);
            return new DataTable();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this._site != null)
                this._site.Dispose();
        }
    }
}
