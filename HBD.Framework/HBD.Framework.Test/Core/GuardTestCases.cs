#region

using System;
using HBD.Framework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Core
{
    [TestClass]
    public class GuardTestCases
    {
        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        public void Test_ArgumentNotNull()
        {
            try
            {
                Guard.ArgumentIsNotNull(null, "object");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
                Assert.IsTrue(ex.Message.Contains("object"));
            }

            Guard.ArgumentIsNotNull(new object(), "object");
            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ArgumentIs_Type()
        {
            Guard.ArgumentIsTypeOf(new object(), typeof(string), "object");
        }

        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ArgumentIs_Generic()
        {
            Guard.ArgumentIsTypeOf<string>(new object(), "object");
        }

        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        [ExpectedException(typeof(ArgumentException))]
        public void MustGreaterThan_5With6_Exception()
        {
            5.ShouldGreaterThan(6, string.Empty);
        }

        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        [ExpectedException(typeof(ArgumentException))]
        public void MustGreaterThan_5With5_Exception()
        {
            5.ShouldGreaterThan(5, string.Empty);
        }

        [TestMethod]
        [TestCategory("Fw.Core.Guard")]
        public void MustGreaterThan_5With4_NoException()
        {
            5.ShouldGreaterThan(4, string.Empty);
        }
    }
}