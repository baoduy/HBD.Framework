#region

using System.Collections.Generic;
using System.Linq;
using AZROLESLib;
using HBD.Framework.Collections;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    internal sealed class AzRoleAssignment : AzItem
    {
        public AzRoleAssignment()
        {
            Members = new LazyList<string>(GetMember, () => !IsNew);
            Members.CollectionChanged += TheValue_HasChanged;
        }

        private IAzRole IazRole => IAzItem as IAzRole;
        private AzScope Scope => Parent as AzScope;

        public LazyList<string> Members { get; }

        private IEnumerable<string> GetMember()
        {
            var members = IazRole.MembersName as object[];
            return members?.OfType<string>().Select(UserPrincipalHelper.GetUserNameWithoutDomain);
        }

        protected override void OnSaving()
        {
            //Delete the empty Role Assignment.
            //if (this.Members.Count <= 0)
            //{
            //    this.Delete();
            //    return;
            //}

            //Update Assignments
            var scope = Scope;
            Guard.ValueIsNotNull(scope, nameof(Scope));

            var currents = GetMember().ToList();
            var lastest = Members;

            //Get New Items
            foreach (var o in lastest.Where(o => !currents.Contains(o)))
                IazRole.AddMemberName(o);

            //Get Deleted Ones
            foreach (var o in currents.Where(o => !lastest.Contains(o)))
                IazRole.DeleteMemberName(o);

            IazRole.Submit();
        }

        protected override void OnDeleting() => Scope?.DeleteRoleAssignment(this);
    }
}