using HBD.Framework.Collections;
using System;
using System.Collections.Specialized;

namespace HBD.Framework
{
    public static class SimpleMonitorExtensions
    {
        public static IDisposable BlockReentrancy(this SimpleMonitor @this)
        {
            @this.Enter();
            return @this;
        }

        public static void CheckReentrancy(this SimpleMonitor @this, NotifyCollectionChangedEventHandler eventHandler)
        {
            if (@this.Busy && eventHandler?.GetInvocationList().Length > 1)
                throw new InvalidOperationException("CheckReentrancy");
        }
    }
}