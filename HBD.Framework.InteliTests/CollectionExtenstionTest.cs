using HBD.Framework;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// <copyright file="CollectionExtenstionTest.cs" company="Microsoft">
//     Copyright © Microsoft 2015
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace HBD.Framework.Tests
{
    [TestClass]
    [PexClass(typeof(CollectionExtenstion))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class CollectionExtenstionTest
    {
        [PexGenericArguments(typeof(int))]
        [PexMethod]
        [PexAllowedException(typeof(NotSupportedException))]
        public void AddRange<T>(IList<T> @this, IEnumerable<T> collection)
        {
            CollectionExtenstion.AddRange<T>(@this, collection);
            // TODO: add assertions to method CollectionExtenstionTest.AddRange(IList`1<!!0>, IEnumerable`1<!!0>)
        }

        [PexGenericArguments(typeof(int))]
        [PexMethod]
        public void EnqueueArrange<T>(Queue<T> @this, IEnumerable<T> items)
        {
            CollectionExtenstion.EnqueueArrange<T>(@this, items);
            // TODO: add assertions to method CollectionExtenstionTest.EnqueueArrange(Queue`1<!!0>, IEnumerable`1<!!0>)
        }

        [PexGenericArguments(typeof(int))]
        [PexMethod]
        [PexAllowedException(typeof(NotSupportedException))]
        public void RemoveRange<T>(IList<T> @this, IEnumerable<T> collection)
        {
            CollectionExtenstion.RemoveRange<T>(@this, collection);
            // TODO: add assertions to method CollectionExtenstionTest.RemoveRange(IList`1<!!0>, IEnumerable`1<!!0>)
        }

        [PexMethod]
        public bool IsNullOrEmpty(IEnumerable @this)
        {
            bool result = CollectionExtenstion.IsEmpty(@this);
            return result;
            // TODO: add assertions to method CollectionExtenstionTest.IsNullOrEmpty(IEnumerable)
        }
    }
}