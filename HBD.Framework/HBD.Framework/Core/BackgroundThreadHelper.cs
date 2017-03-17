#region

using System;
using System.ComponentModel;
using System.Threading;

#endregion

namespace HBD.Framework.Core
{
    public static class BackgroundThreadHelper
    {
        private static volatile int _index;

        public static Thread StartThread<TResult>(Func<TResult> func, Action<TResult> callBack)
        {
            var t = new Thread(() =>
            {
                var result = default(TResult);
                try
                {
                    result = func.Invoke();
                }
                finally
                {
                    callBack.Invoke(result);
                }
            })
            {
                IsBackground = true,
                Name = $"Thread {++_index}"
            };
            t.Start();
            return t;
        }

        public static Thread StartThread<T, TResult>(Func<T, TResult> func, T parameters, Action<TResult> callBack)
        {
            var t = new Thread(() =>
            {
                var result = default(TResult);
                try
                {
                    result = func.Invoke(parameters);
                }
                finally
                {
                    callBack.Invoke(result);
                }
            })
            {
                IsBackground = true,
                Name = $"Thread {++_index}"
            };

            t.Start();
            return t;
        }

        public static Thread StartThread(Action action)
        {
            var t = new Thread(action.Invoke) {IsBackground = true, Name = $"Thread {++_index}"};
            t.Start();
            return t;
        }

        public static BackgroundWorker StartBackgroundWorker<TResult>(Func<TResult> func, Action<TResult> callBack)
        {
            var t = new BackgroundWorker();
            t.DoWork += (sender, e) =>
            {
                var result = default(TResult);
                try
                {
                    result = func.Invoke();
                }
                finally
                {
                    callBack.Invoke(result);
                }
            };
            return t;
        }

        public static BackgroundWorker StartBackgroundWorker<T, TResult>(Func<T, TResult> func, T parameters,
            Action<TResult> callBack)
        {
            var t = new BackgroundWorker();
            t.DoWork += (sender, e) =>
            {
                var result = default(TResult);
                try
                {
                    result = func.Invoke(parameters);
                }
                finally
                {
                    callBack.Invoke(result);
                }
            };

            return t;
        }

        public static BackgroundWorker StartBackgroundWorker(Action action)
        {
            var t = new BackgroundWorker();
            t.DoWork += (sender, e) => { action.Invoke(); };
            return t;
        }
    }
}