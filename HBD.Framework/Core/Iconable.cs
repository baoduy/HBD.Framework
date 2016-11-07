namespace HBD.Framework.Core
{
    public abstract class Iconable : NotifyPropertyChange
    {
        private object _icon;

        public virtual object Icon
        {
            get { return _icon; }
            set { SetValue(ref _icon, value); }
        }
    }
}