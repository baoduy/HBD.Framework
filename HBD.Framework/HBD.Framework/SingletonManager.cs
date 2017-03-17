using System;
using System.Collections.Concurrent;
using HBD.Framework.Core;

namespace HBD.Framework
{
    public static class SingletonManager
    {
        private static readonly ConcurrentDictionary<Type, ISingletonWrapper> Cache = new ConcurrentDictionary<Type, ISingletonWrapper>();
        /// <summary>
        /// Method will check if variable is NULL then execute factoryFunc to get new instance for variable.
        /// if not then just return it. This ensure that factoryFunc will be call once variable is NULL.
        /// NOTE: if factoryFunc return NULL at the first call then it will be call again and again unstill the return is not NULL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable"></param>
        /// <param name="factoryFunc"></param>
        /// <returns></returns>
        public static T GetOrLoad<T>(ref T variable, Func<T> factoryFunc)
        {
            if (variable != null) return variable;
            return variable = factoryFunc.Invoke();
        }

        /// <summary>
        /// Method will use SingletonWrapper to manage the loading for the instance.
        /// This ensure that factoryFunc will be call ONE TIME only.
        /// NOTE: if factoryFunc return NULL at the first call then the variable will be NULL forever.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factoryFunc"></param>
        /// <returns></returns>
        public static T GetOrLoadOne<T>(Func<T> factoryFunc) where T : class
        {
            var swrapper = Cache.GetOrAdd(typeof(T), new SingletonWrapper<T>(factoryFunc));
            return swrapper.Instance as T;
        }

        /// <summary>
        /// reset and reload the new instance for type T on next GetOrLoadOne.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Reset<T>()
        {
            ISingletonWrapper swrapper;
            if (Cache.TryGetValue(typeof(T), out swrapper))
                swrapper.Reset();
        }

        /// <summary>
        /// reset and reload the new instance for type T on next GetOrLoadOne.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Reset(object cachedObj)
        {
            Guard.ArgumentIsNotNull(cachedObj, nameof(cachedObj));

            ISingletonWrapper swrapper;
            if (Cache.TryGetValue(cachedObj.GetType(), out swrapper))
                swrapper.Reset();
        }

        /// <summary>
        /// Clear all item in cache.
        /// </summary>
        public static void Clear()
        {
            foreach (var wrapper in Cache)
                wrapper.Value.Dispose();
            Cache.Clear();
        }
    }
}
