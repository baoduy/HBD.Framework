#region using

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Text
{
    [TestClass]
    public class TextTestCases
    {
        [TestMethod]
        [TestCategory("Fw.Test")]
        public void Test_ExtractTextByPatterns()
        {
            var patterns = "<hoang> [bao] (duy) steven".ExtractPatterns().ToList();

            Assert.IsTrue(patterns.Count == 3);
            Assert.IsTrue(patterns[0].Value == "hoang" && patterns[0].PatternValue == "<hoang>");
            Assert.IsTrue(patterns[1].Value == "bao" && patterns[1].PatternValue == "[bao]");
            Assert.IsTrue(patterns[2].Value == "duy" && patterns[2].PatternValue == "(duy)");

            patterns = "<hoang bao] (duy".ExtractPatterns().ToList();
            Assert.IsTrue(patterns.Count == 0);
        }
    }
}