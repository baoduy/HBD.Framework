#region using

using System;
using System.Collections.Generic;

#endregion

namespace HBD.Framework.Core
{

#if !NETSTANDARD1_6
    public static class AssemblyHelper
    {
        /// <summary>
        ///     Find all Types in current Domain that inherited from inheritedTypes.
        /// </summary>
        /// <returns></returns>
        public static TypeFinder FindAllTypes() => new TypeFinder();

        /// <summary>
        ///     Find all Types in current Domain that inherited from T.
        /// </summary>
        /// <typeparam name="T">inherited type</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindAllTypes<T>() => FindAllTypes().DeliveredFrom(typeof(T));
    }

#endif
}