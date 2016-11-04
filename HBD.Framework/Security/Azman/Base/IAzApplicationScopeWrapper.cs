using AZROLESLib;
using HBD.Framework.Core;
using System;
using System.Reflection;

namespace HBD.Framework.Security.Azman.Base
{
    internal sealed class IAzApplicationScopeWrapper
    {
        private readonly IAzApplication _application;
        private readonly IAzScope _scope;

        public IAzApplicationScopeWrapper(IAzApplication application)
        {
            Guard.ArgumentIsNotNull(application, nameof(application));
            _application = application;
        }

        public IAzApplicationScopeWrapper(IAzScope scope)
        {
            Guard.ArgumentIsNotNull(scope, nameof(scope));
            _scope = scope;
        }

        public string Name
        {
            get { return _scope != null ? _scope.Name : _application.Name; }
            set
            {
                if (_scope != null) _scope.Name = value;
                else _application.Name = value;
            }
        }

        public string Description
        {
            get { return _scope != null ? _scope.Description : _application.Description; }
            set
            {
                if (_scope != null) _scope.Description = value;
                else _application.Description = value;
            }
        }

        public Type Type => _application?.GetType() ?? _scope.GetType();

        public IAzOperations Operations
        {
            get
            {
                if (_application != null) return _application.Operations;
                throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
            }
        }

        public IAzScopes Scopes
        {
            get
            {
                if (_application != null) return _application.Scopes;
                throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
            }
        }

        public IAzTasks Tasks => _application != null ? _application.Tasks : _scope.Tasks;

        public IAzRoles Roles => _application != null ? _application.Roles : _scope.Roles;

        public IAzApplicationGroups ApplicationGroups
            => _application != null ? _application.ApplicationGroups : _scope.ApplicationGroups;

        public void Submit()
        {
            if (_scope != null) _scope.Submit();
            else _application.Submit();
        }

        public IAzTask CreateTask(string name)
            => _scope != null ? _scope.CreateTask(name) : _application.CreateTask(name);

        public void DeleteTask(string name)
        {
            if (_application != null) _application.DeleteTask(name);
            else _scope.DeleteTask(name);
        }

        public IAzApplicationGroup CreateApplicationGroup(string name)
            => _scope != null ? _scope.CreateApplicationGroup(name) : _application.CreateApplicationGroup(name);

        public void DeleteApplicationGroup(string name)
        {
            if (_application != null) _application.DeleteApplicationGroup(name);
            else _scope.DeleteApplicationGroup(name);
        }

        public IAzOperation CreateOperation(string name)
        {
            if (_application != null) return _application.CreateOperation(name);
            throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
        }

        public void DeleteOperation(string name)
        {
            if (_application != null) _application.DeleteOperation(name);
            else throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
        }

        public IAzScope CreateScope(string name)
        {
            if (_application != null) return _application.CreateScope(name);
            throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
        }

        public void DeleteScope(string name)
        {
            if (_application != null) _application.DeleteScope(name);
            else throw new NotSupportedException($"{MethodBase.GetCurrentMethod().Name}_{this.Name}_{this.Type.Name}");
        }

        public IAzRole CreateRole(string name)
            => _scope != null ? _scope.CreateRole(name) : _application.CreateRole(name);

        public void DeleteRole(string name)
        {
            if (_application != null) _application.DeleteRole(name);
            else _scope.DeleteRole(name);
        }
    }
}