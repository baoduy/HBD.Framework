using AZROLESLib;
using HBD.Framework.Core;
using HBD.Framework.Security.Azman.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Security.Azman
{
    /// <summary>
    /// Connection string:
    /// - XML File Storage: msxml://[FilePath]
    /// - SQL Storage:
    /// Trusted:   mssql://Driver={SQL Server};Server={[Server Name]};/[Database Name]/[AzMan Store Name]
    /// Untrusted: mssql://Driver={SQL Server};Server={[Server Name]};Uid=[User
    ///            Id];Pwd=[Password];/[Database Name]/[AzMan Store Name]
    /// </summary>
    public sealed class AzManContext : IDisposable
    {
        private readonly IAzAuthorizationStore _azManStore;

        //private ConcurrentDictionary<string, IAzApplication> Applications { get; set; }

        public AzManContext(string connectionString) : this(GetConnectionStringBuilder(connectionString))
        {
        }

        public AzManContext(IAzConnectionStringBuilder connectionString)
        {
            Guard.ArgumentIsNotNull(connectionString, nameof(connectionString));
            ConnectionString = connectionString;

            _azManStore = new AzAuthorizationStore();
            _azManStore.Initialize(0, connectionString.ConnectionString);

            Applications = new AzItemCollection<AzApplication>(null, GetApplications);
        }

        public IAzConnectionStringBuilder ConnectionString { get; private set; }

        public void Dispose() => Save();

        private static IAzConnectionStringBuilder GetConnectionStringBuilder(string connectionString)
        {
            Guard.ArgumentIsNotNull(connectionString, nameof(connectionString));

            if (connectionString.StartsWithIgnoreCase("mssql"))
                return new AzSqlConnectionStringBuilder(connectionString);
            return new AzXmlConnectionStringBuilder(connectionString);
        }

        #region public methods

        public AzItemCollection<AzApplication> Applications { get; }

        public AzApplication CreateApplication(string name) => new AzApplication(this) { Name = name };

        public void Refresh() => _azManStore.UpdateCache();

        //public void DeleteApplication(string name)
        //    => DeleteApplication(Applications.FirstOrDefault(a => a.Name.IsEqualsIgnoreCase(name)));

        //public void DeleteApplication(AzApplication application)
        //{
        //    if (application == null) return;
        //    Applications.Remove(application);
        //    _azManStore.DeleteApplication(application.Name);
        //    HasChanged = true;
        //}

        public void Save()
        {
            _azManStore.Submit();
            HasChanged = false;
        }

        #endregion public methods

        #region private & internal methods

        //private IEnumerable<AzApplication> GetApplicationsFromXml()
        //{
        //    var conn = this.ConnectionString as AzXmlConnectionStringBuilder;
        //    if (conn == null) return null;

        // //De-serialize information from XML file. var xelement = XElement.Load(conn.FileName);

        //    return xelement.Elements("AzApplication").Select(e => new AzApplication(this)
        //    {
        //        Name = e.Attribute("Name").Value,
        //        Description = e.Attribute("Description").Value,
        //        ApplicationVersion = e.Attribute("ApplicationVersion").Value,
        //    });
        //}

        //private IEnumerable<AzApplication> GetApplicationsFromSql()
        //{
        //    var conn = this.ConnectionString as AzSqlConnectionStringBuilder;
        //    if (conn == null) yield break;

        // var sqlConstr = conn.ToSqlConnectionString(); //Get information from SQL

        // using (var con = new SqlQueryBuilderContext(sqlConstr)) { var select = con.CreateSelectQuery();

        // select.Fields("Az.Name", "Az.Description", "Az.ApplicationVersion") .From(t =>
        // t._("AzMan_AzApplication").As("Az").InnerJoin("AzMan_AzAuthorizationStore").As("S") .On(c
        // => c._("Az.StoreID") == c._("S.ID"))) .Where(t => t._("S.Name") == conn.AzManStoreName);

        //        using (var reader = con.ExecuteReader(select))
        //            while (reader.Read())
        //                yield return new AzApplication(this)
        //                {
        //                    Name = reader.GetValue<string>("Name"),
        //                    Description = reader.GetValue<string>("Description"),
        //                    ApplicationVersion = reader.GetValue<string>("ApplicationVersion"),
        //                };
        //    }
        //}

        internal bool HasChanged { get; set; }

        internal IAzApplication CreateAzApplication(string name, string description)
        {
            var item = _azManStore.CreateApplication(name);
            item.Description = description;
            item.Submit();
            return item;
        }

        internal void DeleteApplication(AzApplication application)
        {
            if (application == null) return;
            Applications.Remove(application);
            _azManStore.DeleteApplication(application.Name);
            HasChanged = true;
        }

        private IEnumerable<AzApplication> GetApplications()
            => _azManStore.Applications.OfType<IAzApplication>().Select(a => new AzApplication(this) { IAzItem = a });

        //internal IAzApplication OpenApplication(string appName) => _azManStore.OpenApplication(appName);
        //internal void CloseApplication(string appName) => _azManStore.CloseApplication(appName, 0x10);

        #endregion private & internal methods
    }
}