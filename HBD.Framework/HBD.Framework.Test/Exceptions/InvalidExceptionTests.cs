#region using

using HBD.Framework.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Exceptions
{
    [TestClass]
    public class InvalidExceptionTests
    {
        [TestMethod]
        [TestCategory("Fw.Exceptions")]
        public void InvalidExceptionsTest()
        {
            var priObj = new PrivateType(typeof(InvalidException));
            Assert.AreEqual("A is invalid.", priObj.InvokeStatic("BuildMessage", "A"));
            Assert.AreEqual("A and B are invalid.", priObj.InvokeStatic("BuildMessage", "A", "B"));
            Assert.AreEqual("A, B and C are invalid.", priObj.InvokeStatic("BuildMessage", "A", "B", "C"));
            Assert.AreEqual("Value is invalid.", priObj.InvokeStatic("BuildMessage"));
        }
    }
}