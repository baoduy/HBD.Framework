using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using HBD.Framework.Core;

namespace HBD.Framework.AD
{
    public abstract class ADManagerbase : IDisposable
    {
        public string DirectoryPath { get; private set; }
        public string DomainName { get; private set; }
        public string ManagerAccount { get; private set; }
        public string Password { get; private set; }

        public ADManagerbase(string directoryPath, string domainName, string managerAccount, string password)
        {
            this.DirectoryPath = directoryPath;
            this.DomainName = domainName;
            this.ManagerAccount = managerAccount;
            this.Password = password;

            Guard.ArgumentNotNull(this.DirectoryPath, "DirectoryPath");
            Guard.ArgumentNotNull(this.DomainName, "DomainName");
        }

        DirectoryEntry _directory = null;
        public DirectoryEntry Directory
        {
            get
            {
                if (DirectoryEntry.Exists(this.DirectoryPath))
                    this._directory = new DirectoryEntry(this.DirectoryPath, this.ManagerAccount, this.Password, AuthenticationTypes.Secure);
                else throw new ArgumentException(string.Format("AD {0} is not existed.", this.DirectoryPath));
                return this._directory;
            }
        }

        protected string GetFilter(FilterType type, string name)
        {
            string filter = string.Empty;

            switch (type)
            {
                case FilterType.UserByLoginAccount:
                    filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountname={0}))"; break;
                case FilterType.UserByName:
                    filter = "(&(objectCategory=person)(objectClass=user)(cn={0}))"; break;
                case FilterType.OrganizationUnit:
                    filter = "(&(objectCategory=organizationalUnit)(ou={0}))"; break;
                case FilterType.Group:
                    filter = "(&(objectCategory=group)(objectClass=group)(cn={0}))"; break;
                case FilterType.Container:
                    filter = "(&(objectCategory=Container)(objectClass=Container)(cn={0}))"; break;
            }

            return string.Format(filter, Regex.Replace(name, ".*\\\\(.*)", "$1", RegexOptions.None));
        }

        public SearchResult Search(FilterType type, string name)
        {
            return this.Search(this.GetFilter(type, name));
        }
       
        public SearchResult Search(string filter)
        {
            using (DirectorySearcher deSearch = new DirectorySearcher())
            {
                deSearch.SearchRoot = this.Directory;

                deSearch.Filter = filter;
                deSearch.SearchScope = SearchScope.Subtree;
                deSearch.PageSize = 1;

                return deSearch.FindOne();
            }
        }

        public void Dispose()
        {
            if (this._directory != null)
            {
                try { this._directory.Dispose(); }
                finally { this._directory = null; }
            }
        }
    }
}
