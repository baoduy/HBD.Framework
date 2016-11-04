using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework;
using System.Linq;
using System;

namespace HBD.Framework.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void IsInTest()
        {
            Assert.IsFalse("".IsAnyOf("123", "456"));
            Assert.IsFalse("123".IsAnyOf());
            Assert.IsFalse("abc".IsAnyOf("ABC", "AAA"));
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void IsInIgnoreCaseTest()
        {
            Assert.IsTrue("abc".IsAnyOfIgnoreCase("ABC", "AAA"));
            Assert.IsFalse("a".IsAnyOfIgnoreCase("ABC", "AAA"));
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void GetMd5HashCodeTest()
        {
            Assert.IsTrue("".GetMd5HashCode() == "");
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void SplitWordsTest()
        {
            Assert.AreEqual("HelloWorld".SplitWords(), "Hello World");
            Assert.AreEqual("Hello World".SplitWords(), "Hello World");
            Assert.AreEqual("Hello".SplitWords(), "Hello");

            Assert.AreEqual("Hello123ABC".SplitWords(), "Hello 123 ABC");
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ExtractAngledBracket_Test()
        {
            Assert.IsTrue(!"<>".ExtractAngledBrackets().Any());
            Assert.IsTrue("<A>".ExtractAngledBrackets().Count() == 1);
            Assert.IsTrue("<A> <B>".ExtractAngledBrackets().Count() == 2);
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ExtractBracket_Test()
        {
            Assert.IsTrue(!"[]".ExtractBrackets().Any());
            Assert.IsTrue("[A]".ExtractBrackets().Count() == 1);
            Assert.IsTrue("[A] [B]".ExtractBrackets().Count() == 2);
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ExtractParenthesis_Test()
        {
            Assert.IsTrue(!"()".ExtractParenthesis().Any());
            Assert.IsTrue("(A)".ExtractParenthesis().Count() == 1);
            Assert.IsTrue("(A) (B)".ExtractParenthesis().Count() == 2);
        }

        [TestMethod()]
        [TestCategory("Fw.Extensions")]
        public void ExtractBracesTest()
        {
            Assert.IsTrue(!"{}".ExtractBraces().Any());
            Assert.IsTrue("{A}".ExtractBraces().Count() == 1);
            Assert.IsTrue("{A} {B}".ExtractBraces().Count() == 2);
        }

        [TestMethod()]
        public void ReplaceIgnoreCaseTest()
        {
            Assert.AreEqual("Hoang Bao Duy".ReplaceIgnoreCase("duy", "Test"), "Hoang Bao Test");
            Assert.AreEqual("Hoang Bao Duy".ReplaceIgnoreCase("BAO", "TEST"), "Hoang TEST Duy");
            Assert.IsNull(((string)null).ReplaceIgnoreCase("BAO", "TEST"));
        }

        [TestMethod()]
        public void ContainsIgnoreCaseTest()
        {
            Assert.IsTrue(new[] { "123", "ABC", "aaa" }.ContainsIgnoreCase("AAA"));
            Assert.IsFalse(new string[] { }.ContainsIgnoreCase("AAA"));
        }

        [TestMethod()]
        public void SingleString_ContainsIgnoreCaseTest()
        {
            Assert.IsTrue("Hoang Bao Duy".ContainsIgnoreCase("bao"));
            Assert.IsFalse("Hoang Bao Duy".ContainsIgnoreCase("aaa"));
        }

        [TestMethod()]
        public void ContainsAnyTest()
        {
            Assert.IsTrue("Hoang Bao Duy".ContainsAny(new[] { "Bao" }));
            Assert.IsFalse("Hoang Bao Duy".ContainsAny(new[] { "duy" }));
        }

        [TestMethod()]
        public void ContainsAnyIgnoreCaseTest()
        {
            Assert.IsTrue("Hoang Bao Duy".ContainsAnyIgnoreCase(new[] { "bao" }));
            Assert.IsFalse("Hoang Bao Duy".ContainsAnyIgnoreCase(new[] { "aaa" }));
        }

        [TestMethod()]
        public void ExtractDatesTest()
        {
            Assert.AreEqual("Testing Date Time 2016/07/08".ExtractDates("yyyy/MM/dd").First(),new DateTime(2016,07,08));
            Assert.AreEqual("Testing Date Time 08/Jul/16".ExtractDates("dd/MMM/yy").First(), new DateTime(2016, 07, 08));

            Assert.AreEqual("Testing Date Time 2016/07/08 12:24:00".ExtractDates("yyyy/MM/dd hh:mm:ss").First(), new DateTime(2016, 07, 08,0,24,0));
            Assert.AreEqual("Testing Date Time 08/Jul/16_002412".ExtractDates("dd/MMM/yy_hhmmss").First(), new DateTime(2016, 07, 08,0,24,12));
        }
    }
}