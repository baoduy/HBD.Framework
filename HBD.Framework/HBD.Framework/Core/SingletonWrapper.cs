using System;

namespace HBD.Framework.Core
{
    internal interface ISingletonWrapper : IDisposable
    {
        object Instance { get; }
        void Reset();
    }

    /// <summary>
    /// Support to load instance of class 1 time only.
    /// </summary>
    public class SingletonWrapper<T> : ISingletonWrapper, IDisposable
    {
        private readonly Func<T> _factoryFunc;
        private bool _isLoaded = false;
        private bool _isDisposed = false;
        private T _instance;
        public SingletonWrapper(Func<T> factoryFunc)
        {
            Guard.ArgumentIsNotNull(factoryFunc, nameof(factoryFunc));
            _factoryFunc = factoryFunc;
        }

        public virtual T Instance
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException($"SingletonWrapper of {typeof(T).FullName}", $"SingletonWrapper of {typeof(T).FullName} is Disposed.");

                if (_isLoaded) return _instance;

                _isLoaded = true;
                TryDisposeInstance();
                return _instance = _factoryFunc.Invoke();
            }
        }

        object ISingletonWrapper.Instance => Instance;

        /// <summary>
        /// Reset and load instance again on next accessing.
        /// </summary>
        public virtual void Reset() => _isLoaded = false;

        public void Dispose()
        {
            if (_instance == null || _isDisposed) return;
            _isDisposed = true;

            TryDisposeInstance();
        }

        private void TryDisposeInstance()
        {
            var dis = _instance as IDisposable;
            dis?.Dispose();
        }
    }
}
