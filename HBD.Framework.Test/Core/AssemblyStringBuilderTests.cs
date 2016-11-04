using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HBD.Framework.Core.Tests
{
    [TestClass()]
    public class AssemblyStringBuilderTests
    {
        [TestMethod()]
        [TestCategory("Fw.Core")]
        public void ParseTest()
        {
            Assert.AreEqual("ABC.AAA", AssemblyStringBuilder.Parse("ABC.AAA").FullTypeName);
            Assert.AreEqual("ABC", AssemblyStringBuilder.Parse("ABC.AAA,ABC").AssemblyFileName);
        }

        [TestMethod()]
        [TestCategory("Fw.Core")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_ArgumentNullException_Test()
        {
            AssemblyStringBuilder.Parse("");
        }

        [TestMethod()]
        [TestCategory("Fw.Core")]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_ArgumentException_Test()
        {
            AssemblyStringBuilder.Parse("ABC.AAA,ABC,CCC");
        }

        [TestMethod()]
        [TestCategory("Fw.Core")]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_ArgumentException_Test2()
        {
            AssemblyStringBuilder.Parse(",");
        }

        [TestMethod()]
        [TestCategory("Fw.Core")]
        public void Implicit_String_Test()
        {
            AssemblyStringBuilder a = "ABC.AAA,ABC";
            Assert.AreEqual("ABC.AAA",a.FullTypeName);
            Assert.AreEqual("ABC", a.AssemblyFileName);
        }

        [TestMethod()]
        [TestCategory("Fw.Core")]
        public void Implicit_AssemblyStringBuilder_Test()
        {
            var a = AssemblyStringBuilder.Parse("ABC.AAA,ABC");
            string s = a;

            Assert.AreEqual("ABC.AAA,ABC", s);
        }
    }
}