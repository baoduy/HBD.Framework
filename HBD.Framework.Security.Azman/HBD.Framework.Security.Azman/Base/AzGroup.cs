#region

using System.Collections.Generic;
using System.Linq;
using AZROLESLib;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzGroup : AzItem
    {
        public AzGroup()
        {
            Members = new AzItemCollection<AzMember>(this, GetMembers, () => !IsNew);
        }

        private IAzApplicationGroup Group => IAzItem as IAzApplicationGroup;
        private AzScope Scope => Parent as AzScope;

        public AzItemCollection<AzMember> Members { get; }

        private IEnumerable<AzMember> GetMembers()
        {
            var members = Group?.MembersName as object[];
            return members?.OfType<string>().Select(m => new AzMember {Name = m, IAzItem = m});
        }

        protected override void OnSaving()
        {
            if (IsNew)
            {
                if (Members.Count <= 0) return;
                IAzItem = Scope.CreateGroup(Name, Description);
            }
            else
            {
                Group.Name = Name;
                Group.Description = Description;
            }

            if (Members.Count <= 0)
            {
                Delete();
                return;
            }

            //Update Members to Group
            var currents = GetMembers().Select(m => m.NameWithoutDomain).ToList();
            var lastest = Members.Select(o => o.NameWithoutDomain).ToList();

            //Get New Items
            foreach (var o in lastest.Where(o => !currents.Contains(o)))
                AddMember(o);

            //Get Deleted Ones
            foreach (var o in currents.Where(o => !lastest.Contains(o)))
                DeleteMember(o);

            //Update Members to Role Assignments
            Members.ForEach(m => m.Save());

            Group.Submit();
        }

        protected override void OnDeleting()
        {
            //Remove all Role Assigned for Members.
            foreach (var member in Members)
                foreach (
                    var assignment in
                    Scope.RoleAssignments.Where(
                        assignment => assignment.Members.Any(m => m.EndsWith(member.NameWithoutDomain))))
                    assignment.Members.Remove(member.Name);

            Scope.DeleteGroup(this);
        }

        #region Private Methods

        internal void AddMember(string name) => Group.AddMemberName(name);

        internal void DeleteMember(string name) => Group?.DeleteMemberName(name);

        #endregion Private Methods
    }
}