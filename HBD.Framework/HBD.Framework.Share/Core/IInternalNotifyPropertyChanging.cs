#region using

using System;
using HBD.Framework.EventArguments;

#endregion

namespace HBD.Framework.Core
{
    internal interface IInternalNotifyPropertyChanging
    {
        event EventHandler<CancelablePropertyChangingEventArgs> InternalPropertyChanging;
    }
}