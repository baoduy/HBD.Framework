#region using

using System;

#endregion

namespace HBD.Framework.Collections
{
    public sealed class SimpleMonitor : IDisposable
    {
        private int _busyCount;

        public bool Busy => _busyCount > 0;

        public void Dispose() => _busyCount -= 1;

        public void Enter() => _busyCount += 1;
    }
}