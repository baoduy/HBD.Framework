using HBD.Framework.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
namespace HBD.Framework.Data.Sharepoint
{
    public interface ISPAdapter : IDataConverter, IDisposable
    {
        DataTable ToDataTable(string listTitle, string viewTitle = null);
        DataTable ToDataTable(string listTitle, IFilterClause filterClause, params string[] fields);
        /// <summary>
        /// Get Users in group as DataTable
        /// </summary>
        /// <param name="groupName">Group Name</param>
        /// <returns>DatatTable of users</returns>
        DataTable AllUsersToDataTable(string groupName);
        
        /// <summary>
        /// Get all Users in Site to DataTable
        /// </summary>
        /// <returns></returns>
        DataTable AllUsersToDataTable();
        /// <summary>
        /// Get Login name of users in group
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <returns>login names of users</returns>
        string[] GetUserNames(string groupName);
        /// <summary>
        /// Get all login names in site
        /// </summary>
        /// <returns>login names if users</returns>
        string[] GetAllUserNames();
        bool CanConnect();
        string ExportListToXML(string listTitle, string folderName);

        string[] GetViewTitles(string listTitle);
        string[] ListTitles { get; }
        IDictionary<string, string> SubSites { get; }
        string[] SiteGroups { get; }
        string SiteURL { get; }
        string Title { get; }
        bool Update(string listTitle, int itemID, string fieldName, object value);
        Dictionary<string, object> GetItemByID(string listTitle, int itemID);
    }
}
