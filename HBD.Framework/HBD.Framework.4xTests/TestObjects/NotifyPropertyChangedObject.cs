#region using

using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Test.TestObjects
{
    public class NotifyPropertyChangedObject : NotifyPropertyChange
    {
        private int _id;
        private TestItem _item;
        private string _name;

        public int Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public TestItem Item
        {
            get => _item;
            set => SetValue(ref _item, value);
        }
    }
}