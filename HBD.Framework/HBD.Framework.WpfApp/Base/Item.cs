using HBD.Framework.Core;

namespace HBD.Framework.WpfApp.Base
{
    public class Item: NotifyPropertyChange
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => base.SetValue(ref _name, value);
        }

        private int _index;
        public int Index
        {
            get => _index;
            set => base.SetValue(ref _index, value);
        }
    }
}
