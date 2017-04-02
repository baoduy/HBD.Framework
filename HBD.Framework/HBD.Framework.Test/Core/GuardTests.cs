#region using

using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Core.Tests
{
    [TestClass]
    public class GuardTests
    {
        [TestMethod]
        [ExpectedException(typeof(NoNullAllowedException))]
        public void ValueIsNotNullTest()
        {
            Guard.ValueIsNotNull(null, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentIsNotNullTest()
        {
            Guard.ArgumentIsNotNull(null, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentIsTypeOfTest()
        {
            Guard.ArgumentIsTypeOf<string>(12, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldGreaterThanTest()
        {
            5.ShouldGreaterThan(10, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentIsTypeOfTest1()
        {
            Guard.ArgumentIsTypeOf(12, typeof(string), "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldNotBeEmptyTest()
        {
            Guard.ShouldNotBeEmpty(new string[] {}, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AllItemsShouldNotBeNullTest()
        {
            Guard.AllItemsShouldNotBeNull(new[] {1, 2, 3, (object) null, 4}, string.Empty);
        }
    }
}