﻿using HBD.Framework.Extensions.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HBD.Framework.Extensions.Internal
{
    internal class TypeExtractor : ITypeExtractor
    {
        #region Private Fields

        private IQueryable<Type> _query;

        #endregion Private Fields

        #region Public Constructors

        public TypeExtractor(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length <= 0)
                throw new ArgumentNullException(nameof(assemblies));

            _query = assemblies.SelectMany(a => a.GetTypes()).AsQueryable();
        }

        #endregion Public Constructors

        #region Public Methods

        public ITypeExtractor Abstract() => Where(t => t.IsAbstract);

        public ITypeExtractor Class() => Where(t => t.IsClass);

        public ITypeExtractor Enum() => Where(t => t.IsEnum);

        public ITypeExtractor Generic() => Where(t => t.IsGenericType);

        public IEnumerator<Type> GetEnumerator() => _query.Distinct().GetEnumerator();

        public ITypeExtractor HasAttribute<TAttribute>() where TAttribute : Attribute
            => HasAttribute(typeof(TAttribute));

        public ITypeExtractor HasAttribute(Type attributeType)
            => Where(t => t.GetCustomAttribute(attributeType) != null);

        public ITypeExtractor Interface() => Where(t => t.IsInterface);

        public ITypeExtractor IsInstanceOf(Type type)
        {
            if (type.IsGenericType && type.IsInterface)
                return Where(t => t.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == type || type.IsAssignableFrom(y)));
            return Where(t => type.IsAssignableFrom(t));
        }

        public ITypeExtractor IsInstanceOf<T>() => IsInstanceOf(typeof(T));

        public ITypeExtractor Nested() => Where(t => t.IsNested);

        public ITypeExtractor NotAbstract() => Where(t => !t.IsAbstract);

        public ITypeExtractor NotClass() => Where(t => !t.IsClass);

        public ITypeExtractor NotEnum() => Where(t => !t.IsEnum);

        public ITypeExtractor NotGeneric() => Where(t => !t.IsGenericType);

        public ITypeExtractor NotInstanceOf(Type type)
        {
            if (type.IsGenericType && type.IsInterface)
                return Where(t => !t.GetInterfaces().Any(y =>
                    y.IsGenericType && y.GetGenericTypeDefinition() == type || type.IsAssignableFrom(y)));
            return Where(t => !type.IsAssignableFrom(t));
        }

        public ITypeExtractor NotInstanceOf<T>() => NotInstanceOf(typeof(T));

        public ITypeExtractor NotInterface() => Where(t => !t.IsInterface);

        public ITypeExtractor NotNested() => Where(t => !t.IsNested);

        public ITypeExtractor NotPublic() => Where(t => !t.IsPublic);

        public ITypeExtractor Public() => Where(t => t.IsPublic);

        public ITypeExtractor Where(Expression<Func<Type, bool>> predicate)
        {
            if (predicate != null)
                _query = _query.Where(predicate);
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Public Methods
    }
}