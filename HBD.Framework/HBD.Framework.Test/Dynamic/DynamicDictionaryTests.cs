#region using

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Dynamic.Tests
{
    [TestClass]
    public class DynamicDictionaryTests
    {
        [TestMethod]
        public void DynamicDictionaryTest()
        {
            dynamic obj = new DynamicDictionary(new {P = "A", K = "B"});
            Assert.AreEqual("A", obj.P);
            Assert.AreEqual("A", obj["p"]);

            obj.A = "AAA";
            Assert.AreEqual("AAA", obj.A);

            obj["B"] = "BBB";
            Assert.AreEqual("BBB", obj["b"]);

            dynamic b = new DynamicDictionary();
            Assert.IsNull(b["A"]);

            dynamic c = new DynamicDictionary();
            Assert.IsNull(c["A"]);
        }
    }
}