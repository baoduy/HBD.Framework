using AZROLESLib;
using HBD.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzRole : AzItem
    {
        public AzRole()
        {
            //Set Parent is null to prevent ListCollection changes item's Parent.
            AssignedOperations = new AzItemCollection<AzOperation>(null, GetAssignedOperations, () => !IsNew);
            AssignedOperations.CollectionChanged += TheValue_HasChanged;

            AssignedRoles = new AzItemCollection<AzRole>(null, GetAssignedRoles, () => !IsNew);
            AssignedRoles.CollectionChanged += TheValue_HasChanged;
        }

        private AzScope Scope => FindParrent<AzScope>();
        private IAzTask IAzTask => IAzItem as IAzTask;

        internal bool IsRoleDefinition => true;

        /// <summary>
        ///     Assigned Operation. Ensure the operation had been ready in Application before assign.
        /// </summary>
        public AzItemCollection<AzOperation> AssignedOperations { get; }

        /// <summary>
        ///     Assigned Role. Ensure the role had been ready in Application before assign.
        ///     Cycle role assignment is illegible.
        /// </summary>
        public AzItemCollection<AzRole> AssignedRoles { get; }

        private IList<string> AssignOpes => (IAzTask.Operations as object[])?.OfType<string>().ToArray();
        private IList<string> AssignRoles => (IAzTask.Tasks as object[])?.OfType<string>().ToArray();

        public override void Validate()
        {
            base.Validate();

            if (!AssignedOperations.Any() && !AssignedRoles.Any())
                throw new ObjectNullException($"Role {Name} must have at least one Operation or Role.");

            var notFoundOp = AssignedOperations.FirstOrDefault(i => !Scope.Operations.Contains(i));
            if (notFoundOp != null)
                throw new ObjectNotFoundException(notFoundOp.Name, nameof(Scope.Operations));

            var notFoundRole = AssignedRoles.FirstOrDefault(i => !Scope.AllRoles.Contains(i));
            if (notFoundRole != null)
                throw new ObjectNotFoundException(notFoundRole.Name, nameof(Scope.Roles));

            var cycleAssignedRole = AssignedRoles.FirstOrDefault(i => i == this);
            if (cycleAssignedRole != null)
                throw new NotSupportedException($"Cycle assignment role {cycleAssignedRole.Name}");
        }

        protected override void OnSaving()
        {
            if (IsNew)
                IAzItem = Scope.CreateRole(Name, Description);
            else
            {
                IAzTask.Name = Name;
                IAzTask.Description = Description;
                IAzTask.IsRoleDefinition = IsRoleDefinition ? 1 : 0;
            }

            //Save the Assignments.
            SaveAssignedOperations();
            SaveAssignedRoles();

            IAzTask.Submit();
        }

        protected override void OnDeleting() => Scope?.DeleteRole(this);

        #region private methods

        private IEnumerable<AzOperation> GetAssignedOperations()
            => Scope?.Operations.Where(o => AssignOpes?.Contains(o.Name) == true);

        private IEnumerable<AzRole> GetAssignedRoles()
            => Scope?.AllRoles.Where(o => AssignRoles?.Contains(o.Name) == true && o != this);

        private void SaveAssignedOperations()
        {
            var currents = AssignOpes;
            var lastest = AssignedOperations.Select(o => o.Name).ToList();

            //Get New Items
            foreach (var o in lastest.Where(o => !currents.Contains(o)))
                IAzTask.AddOperation(o);

            //Get Deleted Ones
            foreach (var o in currents.Where(o => !lastest.Contains(o)))
                IAzTask.DeleteOperation(o);
        }

        private void SaveAssignedRoles()
        {
            var currents = AssignRoles;
            var lastest = AssignedRoles.Select(o => o.Name).ToList();

            //Get New Items
            foreach (var o in lastest.Where(o => !currents.Contains(o)))
                IAzTask.AddTask(o);

            //Get Deleted Ones
            foreach (var o in currents.Where(o => !lastest.Contains(o)))
                IAzTask.DeleteTask(o);
        }

        #endregion private methods
    }
}