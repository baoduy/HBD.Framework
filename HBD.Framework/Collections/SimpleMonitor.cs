using System;

namespace HBD.Framework.Collections
{
    [Serializable]
    public sealed class SimpleMonitor : IDisposable
    {
        private int _busyCount = 0;

        public bool Busy => this._busyCount > 0;

        public void Enter() => this._busyCount += 1;

        public void Dispose() => this._busyCount -= 1;
    }
}