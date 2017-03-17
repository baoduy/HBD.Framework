#region

using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzMember : AzItem
    {
        public AzMember()
        {
            AssignedRoles = new AzItemCollection<AzRole>(null, GetAssignedRoles, () => Name.IsNotNull());
            AssignedRoles.CollectionChanged += TheValue_HasChanged;
        }

        private AzScope Scope => FindParrent<AzScope>();
        private AzGroup Group => Parent as AzGroup;

        public AzItemCollection<AzRole> AssignedRoles { get; }

        public string NameWithoutDomain => UserPrincipalHelper.GetUserNameWithoutDomain(this.Name);

        internal IReadOnlyList<string> AssignRoles
            => Scope?.RoleAssignments.Where(r => r.Members.Contains(Name)).Select(r => r.Name).ToArray();

        private IEnumerable<AzRole> GetAssignedRoles() => Scope?.AllRoles.Where(r => AssignRoles.Contains(r.Name));

        public override void Validate()
        {
            base.Validate();

            if (!AssignedRoles.Any())
                throw new NullReferenceException($"Member {Name} must have at least one Role.");

            if (!UserPrincipalHelper.IsUserExisted(Name))
                throw new InstanceNotFoundException(Name);
        }

        protected override void OnSaving()
        {
            //Update Assignments
            var scope = Scope;
            Guard.ValueIsNotNull(scope, nameof(Scope));

            var currents = AssignRoles;
            var lastest = AssignedRoles.Select(o => o.Name).ToList();

            //Get New Items
            foreach (var o in lastest.Where(o => !currents.Contains(o)))
                Scope.GetOrCreateAzRoleAssignment(o).Members.Add(Name);

            //Get Deleted Ones
            foreach (var o in currents.Where(o => !lastest.Contains(o)))
                Scope.GetOrCreateAzRoleAssignment(o).Members.Remove(Name);
        }

        protected override void OnDeleting() => Group?.DeleteMember(Name);
    }
}