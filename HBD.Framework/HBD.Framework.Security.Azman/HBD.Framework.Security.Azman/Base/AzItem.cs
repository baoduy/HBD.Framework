#region

using System;
using System.Diagnostics;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    [DebuggerDisplay("Name = {" + nameof(Name) + "}")]
    public abstract class AzItem : IEquatable<AzItem>, IValidationable
    {
        private dynamic _azItem;
        private string _description;
        private string _name;

        internal virtual dynamic IAzItem
        {
            get { return _azItem; }
            set
            {
                if (_azItem == value) return;

                _azItem = value;

                if (_azItem == null) return;
                SetValueToProperties();
            }
        }

        internal AzItem Parent { get; set; }

        internal bool HasChanged { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                HasChanged = true;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                HasChanged = true;
            }
        }

        public bool IsNew => IAzItem == null;
        public bool IsDeleted { get; private set; }

        public virtual bool Equals(AzItem other) => Name == other?.Name;

        public virtual void Validate() => Guard.ValueIsNotNull(Name, nameof(Name));

        public T FindParrent<T>() where T : AzItem
        {
            var current = Parent;
            while (current != null)
            {
                if (current is T)
                    return (T) current;
                current = current.Parent;
            }
            return default(T);
        }

        public void Save()
        {
            if (IsDeleted)
                throw new Exception("Cannot save deleted item.");

            if (!HasChanged) return;

            Validate();
            OnSaving();
            HasChanged = false;
        }

        protected abstract void OnSaving();

        protected abstract void OnDeleting();

        public void Delete()
        {
            if (IsDeleted) return;
            IsDeleted = true;
            OnDeleting();
        }

        protected virtual void SetValueToProperties()
        {
            if (IAzItem is string) return;

            _name = IAzItem.Name;
            _description = IAzItem.Description;
        }

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.HasChanged = true;
        protected virtual void TheValue_HasChanged(object sender, EventArgs e) => HasChanged = true;
    }
}