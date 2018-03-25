#region

using AZROLESLib;
using HBD.Framework.Core;
using HBD.Framework.Exceptions;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public class AzScope : AzItem
    {
        private IReadOnlyList<AzGroup> _allGroup;

        private IReadOnlyList<AzRole> _allRoles;

        public AzScope()
        {
            Roles = new AzItemCollection<AzRole>(this, GetRoles, () => !IsNew);
            Roles.CollectionChanged += TheValue_HasChanged;

            Groups = new AzItemCollection<AzGroup>(this, GetGroups, () => !IsNew);
            Groups.CollectionChanged += TheValue_HasChanged;

            RoleAssignments = new AzItemCollection<AzRoleAssignment>(this, GetRoleAssignments, () => !IsNew);
        }

        private AzApplication Application => Parent as AzApplication;

        // ReSharper disable once InconsistentNaming
        internal AzApplicationScopeWrapper IAzScope => IAzItem as AzApplicationScopeWrapper;

        internal override dynamic IAzItem
        {
            get { return base.IAzItem; }
            set
            {
                if (value == null)
                    base.IAzItem = null;
                else
                {
                    var scope = value as IAzScope;
                    base.IAzItem = scope != null
                        ? new AzApplicationScopeWrapper(scope)
                        : new AzApplicationScopeWrapper((IAzApplication)value);
                }
            }
        }

        /// <summary>
        ///     Application Operations.
        /// </summary>
        public virtual AzItemCollection<AzOperation> Operations => Application.Operations;

        /// <summary>
        ///     The list of Roles from this Scope and Application level.
        /// </summary>
        internal IReadOnlyList<AzRole> AllRoles
        {
            get
            {
                if (_allRoles != null) return _allRoles;
                return
                    _allRoles = (Application == null ? Roles.ToReadOnly() : Roles.Union(Application.Roles).ToReadOnly());
            }
        }

        /// <summary>
        ///     The list of Groups from this Scope and Application level.
        /// </summary>
        internal IReadOnlyList<AzGroup> AllGroups
        {
            get
            {
                if (_allGroup != null) return _allGroup;
                return
                    _allGroup =
                        Application == null ? Groups.ToReadOnly() : Groups.Union(Application.Groups).ToReadOnly();
            }
        }

        public AzItemCollection<AzRole> Roles { get; }
        public AzItemCollection<AzGroup> Groups { get; }

        public virtual AzUserInfo GetUserInfo(string userName)
        {
            userName = UserPrincipalHelper.GetUserNameWithoutDomain(userName);
            Guard.ArgumentIsNotNull(userName, nameof(userName));

            var operations = new List<AzOperation>();

            var roles = this.RoleAssignments.Where(r => r.Members.AnyIgnoreCase(userName)).Select(r => r.Name).ToList();

            foreach (var role in this.Roles.Where(r => roles.Contains(r.Name)))
                operations.AddRange(role.AssignedOperations);

            return new AzUserInfo(this.Name, userName)
            {
                Operations = operations.Select(o => o.Name).ToArray(),
                Roles = roles.ToArray(),
                Groups = this.Groups.Where(g => g.Members.Any(m => m.NameWithoutDomain.EqualsIgnoreCase(userName))).Select(g => g.Name).ToArray(),
            };
        }

        public override void Validate()
        {
            base.Validate();

            var dupRole = Roles.FirstOrDefault(i => AllRoles.Any(r => r != i && r.Name.EqualsIgnoreCase(i.Name)));
            if (dupRole != null)
                throw new DuplicatedException(dupRole.Name);

            var dupGroup = Groups.FirstOrDefault(i => AllGroups.Any(r => r != i && r.Name.EqualsIgnoreCase(i.Name)));
            if (dupRole != null)
                throw new DuplicatedException(dupRole.Name);
        }

        protected override void OnSaving()
        {
            if (IsNew)
                IAzItem = Application.CreateScope(Name, Description);
            else
            {
                IAzScope.Name = Name;
                IAzScope.Description = Description;
            }

            Roles.ForEach(r => r.Save());
            Groups.ForEach(g => g.Save());
            RoleAssignments.ForEach(r => r.Save());

            IAzScope.Submit();
        }

        protected override void OnDeleting() => Application?.DeleteScope(this);

        #region Private Methods

        internal IAzTask CreateRole(string name, string description = null)
        {
            var iaz = IAzScope.CreateTask(name);
            iaz.IsRoleDefinition = 1;
            iaz.Description = description;
            iaz.Submit();

            return iaz;
        }

        internal void DeleteRole(AzRole role)
        {
            if (role == null) return;
            Roles.Remove(role);
            IAzScope.DeleteTask(role.Name);
            HasChanged = true;
        }

        internal IAzApplicationGroup CreateGroup(string name, string description = null)
        {
            var iaz = IAzScope.CreateApplicationGroup(name);
            iaz.Description = description;
            iaz.Submit();

            return iaz;
        }

        internal void DeleteGroup(AzGroup group)
        {
            if (group == null) return;
            Groups.Remove(group);
            IAzScope.DeleteApplicationGroup(group.Name);
            HasChanged = true;
        }

        internal AzItemCollection<AzRoleAssignment> RoleAssignments { get; }

        internal AzRoleAssignment GetOrCreateAzRoleAssignment(string name)
        {
            var role = RoleAssignments[name];
            if (role != null) return role;

            var azRole = IAzScope.CreateRole(name);
            azRole.Submit();

            role = new AzRoleAssignment { IAzItem = azRole };
            RoleAssignments.Add(role);

            HasChanged = true;
            return role;
        }

        internal void DeleteRoleAssignment(AzRoleAssignment roleAssignment)
        {
            if (roleAssignment == null) return;
            RoleAssignments.Remove(roleAssignment);
            IAzScope.DeleteRole(roleAssignment.Name);
            HasChanged = true;
        }

        internal IEnumerable<AzRole> GetRoles()
            =>
            IAzScope?.Tasks.OfType<IAzTask>()
                .Where(r => r.IsRoleDefinition == 1)
                .Select(r => new AzRole { IAzItem = r });

        internal IEnumerable<AzGroup> GetGroups()
            => IAzScope?.ApplicationGroups.OfType<IAzApplicationGroup>().Select(g => new AzGroup { IAzItem = g });

        internal IEnumerable<AzRoleAssignment> GetRoleAssignments()
            => IAzScope?.Roles.OfType<IAzRole>().Select(r => new AzRoleAssignment { IAzItem = r });

        #endregion Private Methods
    }
}