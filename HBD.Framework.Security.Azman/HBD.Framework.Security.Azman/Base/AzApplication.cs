#region

using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using AZROLESLib;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzApplication : AzScope
    {
        private readonly AzManContext _context;

        internal AzApplication(AzManContext context, IAzApplication app = null)
        {
            Guard.ArgumentIsNotNull(context, nameof(context));
            _context = context;

            IAzItem = app;

            Operations = new AzItemCollection<AzOperation>(this, GetOperations, () => !IsNew);
            Operations.CollectionChanged += TheValue_HasChanged;

            Scopes = new AzItemCollection<AzScope>(this, GetScopes, () => !IsNew);
            Scopes.CollectionChanged += TheValue_HasChanged;
        }

        public override AzItemCollection<AzOperation> Operations { get; }
        public AzItemCollection<AzScope> Scopes { get; }

        public override AzUserInfo GetUserInfo(string userName)
        {
            var userInfo = base.GetUserInfo(userName);
            userInfo.UserScopeInfos = this.Scopes.Select(s => s.GetUserInfo(userName)).Where(u=>!u.IsEmpty()).ToReadOnly();
            return userInfo;
        }

        public AzUserInfo GetUserInfo(string userName,string scope)
        {
            Guard.ArgumentIsNotNull(userName,nameof(userName));
            Guard.ArgumentIsNotNull(scope,nameof(scope));

            var sc = this.Scopes.FirstOrDefault(s => s.Name.EqualsIgnoreCase(scope));
            if(sc==null)throw new InstanceNotFoundException(scope);

            return sc.GetUserInfo(userName);
        }

        protected override void OnSaving()
        {
            if (IsNew)
                IAzItem = _context.CreateAzApplication(Name, Description);

            Operations.ForEach(o => o.Save());
            base.OnSaving();
            Scopes.ForEach(o => o.Save());

            IAzItem.Submit();
        }

        protected override void OnDeleting() => _context.DeleteApplication(this);

        //IAzClientContext InitializeClientContextFromName(string ClientName, string DomainName = "", object varReserved = null);
        //IAzClientContext InitializeClientContextFromStringSid(string SidString, int lOptions, object varReserved);
        //IAzClientContext InitializeClientContextFromToken(ulong ullTokenHandle, object varReserved);

        #region Private methods

        internal IAzOperation CreateOperation(int id, string name, string description)
        {
            var item = IAzItem.CreateOperation(name);
            item.Description = description;
            item.OperationID = id;
            item.Submit();
            return item;
        }

        internal void DeleteOperation(AzOperation operation)
        {
            if (operation == null) return;
            Operations.Remove(operation);
            IAzScope.DeleteOperation(operation.Name);
            HasChanged = true;
        }

        internal IAzScope CreateScope(string name, string description = null)
        {
            var iaz = IAzScope.CreateScope(name);
            iaz.Description = description;
            iaz.Submit();
            return iaz;
        }

        internal void DeleteScope(AzScope scope)
        {
            if (scope == null) return;
            Scopes.Remove(scope);
            IAzScope.DeleteScope(scope.Name);
            HasChanged = true;
        }

        private IEnumerable<AzOperation> GetOperations()
            => IAzScope?.Operations.OfType<IAzOperation>().Select(o => new AzOperation { IAzItem = o });

        private IEnumerable<AzScope> GetScopes()
            => IAzScope?.Scopes.OfType<IAzScope>().Select(s => new AzScope { IAzItem = s });

        #endregion Private methods
    }
}