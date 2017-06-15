#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace HBD.Framework.Core
{
#if !NETSTANDARD1_6
    public class TypeFinder : IEnumerable<Type>
    {
        private Type[] _inheritedFrom;
        private Type[] _notIn;

        protected internal TypeFinder()
        {
        }

        public IEnumerator<Type> GetEnumerator()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes());

            if (_inheritedFrom != null && _inheritedFrom.Length > 0)
                types = types.Where(t => _inheritedFrom.All(i => i.IsAssignableFrom(t)));

            if (_notIn != null && _notIn.Length > 0)
                types = types.Where(t => _notIn.All(i => i != t));

            return types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public TypeFinder InheritedFrom(params Type[] types)
        {
            _inheritedFrom = types;
            return this;
        }

        public TypeFinder NotIn(params Type[] types)
        {
            _notIn = types;
            return this;
        }
    }
#endif
}