using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml.Linq;
using HBD.Framework.Data.XML;
using HBD.Framework.Core;
using HBD.Framework.Data.Utilities;

namespace HBD.Framework.Data.Sharepoint
{
    public abstract class SPAdapterBase : DataConverterBase, ISPAdapter
    {
        public SPAdapterBase(string siteURL)
        {
            this.SiteURL = siteURL;
        }

        private string[] _listTitles = null;
        private string[] _siteGroups = null;
        private IDictionary<string, string> _subSites = null;

        public string SiteURL { get; private set; }
        public abstract string Title { get; }
        public virtual string[] ListTitles
        {
            get
            {
                if (this._listTitles == null)
                    this._listTitles = this.GetListTitles();
                return _listTitles;
            }
        }
        public virtual string[] SiteGroups
        {
            get
            {
                if (_siteGroups == null)
                    _siteGroups = this.GetSiteGroups();
                return _siteGroups;
            }
        }
        public virtual IDictionary<string, string> SubSites
        {
            get
            {
                if (_subSites == null)
                    _subSites = this.GetSubSites();
                return _subSites;
            }
        }
        public abstract bool CanConnect();
        protected abstract string[] GetListTitles();
        protected abstract string[] GetSiteGroups();
        protected abstract IDictionary<string, string> GetSubSites();
        public abstract string[] GetViewTitles(string listTitle);
        
        #region ExportData
        public virtual string ExportListToXML(string listTitle, string folderName)
        {
            Guard.ArgumentNotNull(folderName, "folderName");
            Guard.ArgumentNotNull(listTitle, "listTitle");

            if (!folderName.EndsWith("\\"))
                folderName += "\\";

            var data = this.ToDataTable(listTitle);
            if (data != null)
            {
                var fileName = string.Format("{0}{1}_{2:yyyy.MM.dd hhmmss}.xml", folderName, data.TableName, DateTime.Now);
                using (var adapter = new XMLAdapter(fileName))
                {
                    adapter.WriteFile(data);
                }
            }
            return string.Empty;
        }
        #endregion

        public abstract DataTable ToDataTable(string listTitle, string viewTitle = null);
        public abstract DataTable ToDataTable(string listTitle, IFilterClause filterClause, params string[] fields);
        public abstract Dictionary<string, object> GetItemByID(string listTitle, int itemID);
        public abstract bool Update(string listTitle, int itemID, string fieldName, object value);

        public abstract DataTable AllUsersToDataTable(string groupName);
        public abstract DataTable AllUsersToDataTable();
        public abstract string[] GetUserNames(string groupName);
        public abstract string[] GetAllUserNames();
    }
}
