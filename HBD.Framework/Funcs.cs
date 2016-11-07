using System;

namespace HBD.Framework
{
    public static class Funcs
    {
        /// <summary>
        /// Get value from the variable. Execute the Func to get the value if Variable is NULL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable"></param>
        /// <param name="loadFunc"></param>
        /// <returns></returns>
        public static T GetOrLoad<T>(ref T variable, Func<T> loadFunc) where T : class
        {
            if (variable != null) return variable;
            return variable = loadFunc.Invoke();
        }
    }
}