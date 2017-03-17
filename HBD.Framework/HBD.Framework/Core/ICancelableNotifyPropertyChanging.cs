#region

using System;
using HBD.Framework.EventArguments;

#endregion

namespace HBD.Framework.Core
{
    public interface ICancelableNotifyPropertyChanging
    {
        event EventHandler<CancelablePropertyChangingEventArgs> PropertyChanging;
    }
}