using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Dynamic.Tests
{
    [TestClass()]
    public class DynamicDictionaryTests
    {
        [TestMethod()]
        public void DynamicDictionaryTest()
        {
            dynamic obj = new DynamicDictionary(new { P = "A", K = "B" });
            Assert.AreEqual("A", obj.P);
            Assert.AreEqual("A", obj["p"]);

            obj.A = "AAA";
            Assert.AreEqual("AAA", obj.A);

            obj["B"] = "BBB";
            Assert.AreEqual("BBB", obj["b"]);

            dynamic b = new DynamicDictionary();
            Assert.IsNull(b["A"]);

            dynamic c = new DynamicDictionary(new object[] { });
            Assert.IsNull(c["A"]);
        }
    }
}