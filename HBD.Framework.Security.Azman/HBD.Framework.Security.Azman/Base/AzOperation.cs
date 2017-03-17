#region

using AZROLESLib;
using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Security.Azman.Base
{
    public sealed class AzOperation : AzItem
    {
        private int _id;
        private IAzOperation Operation => IAzItem as IAzOperation;
        private AzApplication Application => Parent as AzApplication;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                HasChanged = true;
            }
        }

        protected override void SetValueToProperties()
        {
            base.SetValueToProperties();
            _id = IAzItem.OperationID;
        }

        protected override void OnDeleting() => Application?.DeleteOperation(this);

        public override void Validate()
        {
            base.Validate();
            Id.ShouldGreaterThan(0, nameof(Id));
        }

        protected override void OnSaving()
        {
            if (IsNew)
                IAzItem = Application.CreateOperation(Id, Name, Description);
            else
            {
                Operation.Name = Name;
                Operation.Description = Description;
                Operation.OperationID = Id;
                Operation.Submit();
            }
        }
    }
}