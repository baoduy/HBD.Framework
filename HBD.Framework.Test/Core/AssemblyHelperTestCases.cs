using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Core;

namespace HBD.Framework.Test.Core
{
    [TestClass]
    public class AssemblyHelperTestCases
    {
        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_AssemblyHelper_LoadType()
        {
            var typeName = "HBD.Framework.TestObjects.Class1";
            var type = AssemblyHelper.GetType(typeName);

            Assert.IsNotNull(type);
            Assert.IsTrue(type.FullName == typeName);
        }

        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_AssemblyHelper_LoadAssebmly()
        {
            var assName="HBD.Framework.TestObjects";
            var assembly = AssemblyHelper.GetAssembly(assName);

            Assert.IsNotNull(assembly);
            Assert.IsNotNull(assembly.FullName == assName);
        }

        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_LoadAssebmly_FromFiles()
        {
            var t = Type.GetType("HBD.Framework.TestObjects.Class1");
            Assert.IsNull(t);
            var t1 = AssemblyHelper.GetType("HBD.Framework.TestObjects.Class1");
            Assert.IsNotNull(t1);
        }
    }
}
