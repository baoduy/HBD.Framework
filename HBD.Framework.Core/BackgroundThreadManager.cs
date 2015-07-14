using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HBD.Framework.Log;
using System.ComponentModel;

namespace HBD.Framework.Core
{
    public static class BackgroundThreadManager
    {
        static volatile int index = 0;
        public static Thread StartThread<TResult>(Func<TResult> func, Action<TResult> callBack)
        {
            var t = new Thread(() =>
            {
                TResult result = default(TResult);
                try
                { result = func.Invoke(); }
                catch (Exception ex) { LogManager.Write(ex); }
                finally
                { callBack.Invoke(result); }

            }) { IsBackground = true, Name = string.Format("Thread {0}", ++index) };
            t.Start();
            return t;
        }

        public static Thread StartThread<T, TResult>(Func<T, TResult> func, T parameters, Action<TResult> callBack)
        {
            var t = new Thread(() =>
            {
                TResult result = default(TResult);
                try
                { result = func.Invoke(parameters); }
                catch (Exception ex) { LogManager.Write(ex); }
                finally
                { callBack.Invoke(result); }

            }) { IsBackground = true, Name = string.Format("Thread {0}", ++index) };

            t.Start();
            return t;
        }

        public static Thread StartThread(Action action)
        {
            var t = new Thread(action.Invoke) { IsBackground = true, Name = string.Format("Thread {0}", ++index) };
            t.Start();
            return t;
        }


        public static BackgroundWorker StartBackgroundWorker<TResult>(Func<TResult> func, Action<TResult> callBack)
        {
            var t = new BackgroundWorker();
            t.DoWork += (sender, e) =>
            {
                TResult result = default(TResult);
                try
                { result = func.Invoke(); }
                catch (Exception ex) { LogManager.Write(ex); }
                finally
                { callBack.Invoke(result); }
            };
            return t;
        }

        public static BackgroundWorker StartBackgroundWorker<T, TResult>(Func<T, TResult> func, T parameters, Action<TResult> callBack)
        {
            var t = new BackgroundWorker();
            t.DoWork += (sender, e) =>
            {
                TResult result = default(TResult);
                try
                { result = func.Invoke(parameters); }
                catch (Exception ex) { LogManager.Write(ex); }
                finally
                { callBack.Invoke(result); }
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
