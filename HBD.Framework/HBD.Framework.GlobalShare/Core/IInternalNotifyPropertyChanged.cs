#region using

using System.ComponentModel;

#endregion

namespace HBD.Framework.Core
{
    internal interface IInternalNotifyPropertyChanged
    {
        event PropertyChangedEventHandler InternalPropertyChanged;
    }
}