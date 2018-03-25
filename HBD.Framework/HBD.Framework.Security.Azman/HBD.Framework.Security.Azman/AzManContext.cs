#region

using AZROLESLib;
using HBD.Framework.Core;
using HBD.Framework.Security.Azman.Base;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace HBD.Framework.Security.Azman
{
    /// <summary>
    ///     Connection string:
    ///     - XML File Storage: msxml://[FilePath]
    ///     - SQL Storage:
    ///     Trusted:   mssql://Driver={SQL Server};Server={[Server Name]};/[Database Name]/[AzMan Store Name]
    ///     Untrusted: mssql://Driver={SQL Server};Server={[Server Name]};Uid=[User
    ///     Id];Pwd=[Password];/[Database Name]/[AzMan Store Name]
    /// </summary>
    public sealed class AzManContext : IDisposable
    {
        private readonly IAzAuthorizationStore _azManStore;

        //private ConcurrentDictionary<string, IAzApplication> Applications { get; set; }

        public AzManContext(string nameOrconnectionString) : this(GetConnectionStringBuilder(nameOrconnectionString))
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

        private static IAzConnectionStringBuilder GetConnectionStringBuilder(string nameOrconnectionString)
        {
            Guard.ArgumentIsNotNull(nameOrconnectionString, nameof(nameOrconnectionString));

            nameOrconnectionString =
                Configuration.ConfigurationManager.GetConnectionString(nameOrconnectionString);

            if (nameOrconnectionString.StartsWithIgnoreCase("mssql"))
                return new AzSqlConnectionStringBuilder(nameOrconnectionString);
            return new AzXmlConnectionStringBuilder(nameOrconnectionString);
        }

        #region public methods

        public AzItemCollection<AzApplication> Applications { get; }

        public AzApplication CreateApplication(string name) => new AzApplication(this) { Name = name };

        public void Refresh() => _azManStore.UpdateCache();

        public AzApplication GetApplication(string applicationName)
            => new AzApplication(this, _azManStore.OpenApplication(applicationName));

        public void Save()
        {
            _azManStore.Submit();
            HasChanged = false;
        }

        #endregion public methods

        #region private & internal methods

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

        #endregion private & internal methods
    }
}