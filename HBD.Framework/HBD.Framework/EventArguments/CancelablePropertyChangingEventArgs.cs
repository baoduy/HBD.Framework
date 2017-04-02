#region using

using System.ComponentModel;

#endregion

namespace HBD.Framework.EventArguments
{
    public class CancelablePropertyChangingEventArgs : PropertyChangingEventArgs
    {
        public CancelablePropertyChangingEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue { get; }
        public object NewValue { get; }
        public bool Cancel { get; set; } = false;
    }
}