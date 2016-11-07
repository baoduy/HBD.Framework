using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Framework.Tests
{
    [TestClass()]
    public class TextExtensionsTests
    {
        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyString_ExtractPatternTextTest()
        {
            var p1 = "".ExtractPatterns().ToList();
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_ExtractPatternTextTest()
        {
            ((string) null).ExtractPatterns();
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void BracketRegex_ExtractPatternTextTest()
        {
            var p = "[1][2]".ExtractPatterns().ToList();
            Assert.IsTrue(p.Count == 2);
            Assert.IsTrue(p[0].PatternValue == "[1]");
            Assert.IsTrue(p[1].PatternValue == "[2]");
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void AngledBracketRegex_ExtractPatternTextTest()
        {
            var p = "<1><2>".ExtractPatterns().ToList();
            Assert.IsTrue(p.Count == 2);
            Assert.IsTrue(p[0].PatternValue == "<1>");
            Assert.IsTrue(p[1].PatternValue == "<2>");
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ParenthesisRegex_ExtractPatternTextTest()
        {
            var p = "(1)(2)".ExtractPatterns().ToList();
            Assert.IsTrue(p.Count == 2);
            Assert.IsTrue(p[0].PatternValue == "(1)");
            Assert.IsTrue(p[1].PatternValue == "(2)");
        }
    }
}