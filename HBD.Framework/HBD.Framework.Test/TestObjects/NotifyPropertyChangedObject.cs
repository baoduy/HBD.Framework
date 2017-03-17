#region

using HBD.Framework.Core;

#endregion

namespace HBD.Framework.Test.TestObjects
{
    public class NotifyPropertyChangedObject : NotifyPropertyChange
    {
        private TestItem _item;
        private string _name;
        private int _id;

        public int Id {
            get { return _id; }
            set { SetValue(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public TestItem Item
        {
            get { return _item; }

            set { SetValue(ref _item, value); }
        }
    }
}