#region using

using HBD.Framework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.St.Core
{
    [TestClass]
    public class WildcardTest
    {
        [TestMethod]
        public void Test_IgnoreCase()
        {
            var w = new Wildcard("*Duy");

            Assert.IsTrue(w.IsMatch("Duy"));
            Assert.IsTrue(w.IsMatch("Hoang Duy"));

            Assert.IsTrue(w.IsMatch("duy"));
            Assert.IsTrue(w.IsMatch("Hoang duY"));

            Assert.IsFalse(w.IsMatch("Duy Hoang"));
        }

        [TestMethod]
        public void Test_NotIgnoreCase()
        {
            var w = new Wildcard("Hoang*Duy", false);

            Assert.IsTrue(w.IsMatch("Hoang Duy"));
            Assert.IsTrue(w.IsMatch("Hoang BaoDuy"));
            Assert.IsTrue(w.IsMatch("Hoang Bao Duy"));

            Assert.IsFalse(w.IsMatch("duy"));
            Assert.IsFalse(w.IsMatch("Hoang duY"));

            Assert.IsFalse(w.IsMatch("Duy Hoang"));
        }

        [TestMethod]
        public void Test_DoubleWildcard()
        {
            var w = new Wildcard("*.Controllers.*", false);

            Assert.IsTrue(w.IsMatch("Hoang.Controllers.Duy"));
            Assert.IsTrue(w.IsMatch("Hoang *.Controllers.Duy"));
            Assert.IsTrue(w.IsMatch("Hoang *.Controllers.* Duy"));

            Assert.IsFalse(w.IsMatch("duy"));
            Assert.IsFalse(w.IsMatch("Hoang duY"));
            Assert.IsFalse(w.IsMatch("Duy Hoang"));
        }
    }
}