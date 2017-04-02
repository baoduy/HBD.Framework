#region using

using System;

#endregion

namespace HBD.Framework.Core
{
    public interface IServiceInstanceManager<in TInterface> : IDisposable where TInterface : class
    {
        /// <summary>
        ///     Restore to default service. Custom service will be Disposed.
        /// </summary>
        void RestoreDefaultService();

        /// <summary>
        ///     Set custom Service for Current instance. The default service won't be use anymore.
        /// </summary>
        /// <param name="customService"></param>
        void SetCustomeService(TInterface customService);
    }
}