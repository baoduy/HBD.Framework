#region using

using HBD.Framework.Core;
using System.Diagnostics;

#endregion

namespace HBD.Framework.Test.TestObjects
{
    [DebuggerDisplay("Id = {Id}")]
    public class NotifyPropertyChangedObject : NotifyPropertyChange
    {
        private int _id;
        private TestItem _item;
        private string _name;

        public int Id
        {
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