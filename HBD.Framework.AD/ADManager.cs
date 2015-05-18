using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Text.RegularExpressions;
using HBD.Framework.Core;
using HBD.Framework.Log;

namespace HBD.Framework.AD
{
    public class ADManager : ADManagerbase
    {
        public ADManager(string directoryPath, string domainName, string managerAccount, string password)
            : base(directoryPath, domainName, managerAccount, password) { }

        public DirectoryEntry GetAccount(string accountName)
        {
            var result = this.Search(FilterType.UserByLoginAccount, accountName);
            if (result == null)
                result = this.Search(FilterType.UserByName, accountName);

            if (result != null)
                return result.GetDirectoryEntry();
            return null;
        }

        public DirectoryEntry GetContainer(string containerName)
        {
            var result = this.Search(FilterType.Container, containerName);
            if (result != null)
                return result.GetDirectoryEntry();
            return null;
        }

        public DirectoryEntry GetOrganizationUnit(string OUName)
        {
            var result = this.Search(FilterType.OrganizationUnit, OUName);
            if (result != null)
                return result.GetDirectoryEntry();
            return null;
        }

        public DirectoryEntry GetOrgUnitOrContainer(string name)
        {
            var containter = this.GetContainer(name);
            if (containter == null)
                containter = this.GetOrganizationUnit(name);
            return containter;
        }

        public Status GetAccountStatus(SearchResult result)
        {
            if (result == null)
                return Status.Unknow;

            using (var entry = result.GetDirectoryEntry())
            {
                return this.GetAccountStatus(entry);
            }
        }

        public Status GetAccountStatus(DirectoryEntry entry)
        {
            if (entry == null)
                return Status.Unknow;

            if (entry.NativeGuid == null)
                return Status.Disabled;

            int flags = (int)entry.Properties[Constants.AccountProperties.UserAccountControl].Value;
            return Convert.ToBoolean(flags & 0x0002) ? Status.Disabled : Status.Active;
        }

        public bool IsAccountExisted(string accountName)
        {
            return this.GetAccount(accountName) != null;
        }

        public void CreateGroup(string groupName)
        {
            using (DirectoryEntry group = this.Directory.Children.Add("CN=" + groupName, "GROUP"))
            {
                group.Properties[Constants.AccountProperties.UserLogonName_preWin2000].Value = groupName;
                group.CommitChanges();
            }
        }

        public Guid CreateAccount(string OU_Container, string accountName, string firstName, string lastName, string password)
        {
            if (this.GetAccount(accountName) != null)
                throw new ArgumentException(string.Format("Account {0} is existed.", accountName));

            //Default container is USERS
            if (string.IsNullOrEmpty(OU_Container))
                OU_Container = "USERS";

            var container = this.GetContainer(OU_Container);
            if (container == null)
                container = this.GetOrganizationUnit(OU_Container);

            Guard.ArgumentNotNull(container, "Organization Unit or Container");

            try
            {
                using (DirectoryEntry user = container.Children.Add(string.Format("CN={0}", accountName), "USER"))
                {
                    user.Properties[Constants.AccountProperties.UserLogonName].Value = accountName;
                    user.Properties[Constants.AccountProperties.UserLogonName_preWin2000].Value = accountName;

                    user.CommitChanges();
                    this.Directory.CommitChanges();

                    user.Properties[Constants.AccountProperties.FirstName].Value = firstName;
                    user.Properties[Constants.AccountProperties.LastName].Value = lastName;

                    user.Invoke("SetPassword", new object[] { password });
                    this.EnableAccountWithoutCommit(user);

                    user.CommitChanges();
                    return user.Guid;
                }
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                return Guid.Empty;
            }
        }

        public void RemaneUserAccount(string oldAccountName, string newAccountName)
        {
            var user = this.GetAccount(oldAccountName);
            if (user != null)
            {
                user.Properties[Constants.AccountProperties.UserLogonName].Value = newAccountName;
                user.CommitChanges();
                user.Close();
                user.Dispose();
            }
        }

        public bool EnableAccount(string accountName)
        {
            var user = this.GetAccount(accountName);
            if (user == null)
                return false;

            this.EnableAccountWithoutCommit(user);

            user.CommitChanges();
            user.Close();
            user.Dispose();

            return true;
        }

        public bool EnableAccountWithoutCommit(DirectoryEntry account)
        {
            if (account == null)
                return false;

            int val = (int)account.Properties[Constants.AccountProperties.UserAccountControl].Value;
            account.Properties[Constants.AccountProperties.UserAccountControl].Value = val & ~0x2;

            return true;
        }

        public bool DisableAccount(string accountName)
        {
            var user = this.GetAccount(accountName);
            if (user == null)
                return false;

            int val = (int)user.Properties[Constants.AccountProperties.UserAccountControl].Value;
            user.Properties[Constants.AccountProperties.UserAccountControl].Value =
                val | (int)Constants.AccountPropertyValues.ACCOUNTDISABLE;

            user.CommitChanges();
            user.Close();
            user.Dispose();

            return true;
        }

        public bool MoveAccountTo(string accountName, string containerName)
        {
            var account = this.GetAccount(accountName);
            var container = this.GetOrgUnitOrContainer(containerName);

            return this.MoveTo(account, container);
        }

        protected bool MoveTo(DirectoryEntry entry, DirectoryEntry containEntry)
        {
            if (entry == null || containEntry == null)
                return false;

            entry.MoveTo(containEntry);
            return true;
        }

        public bool Unlock(string accountName)
        {
            var user = this.GetAccount(accountName);
            if (user == null)
                return false;

            user.Properties[Constants.AccountProperties.LockoutTime].Value = 0; //unlock account
            user.CommitChanges(); //may not be needed but adding it anyways
            user.Close();
            user.Dispose();

            return true;
        }
    }
}
