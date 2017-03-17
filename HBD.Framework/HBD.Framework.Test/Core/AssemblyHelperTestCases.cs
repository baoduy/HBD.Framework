#region

using System;
using HBD.Framework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Core
{
    [TestClass]
    public class AssemblyHelperTestCases
    {
        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_AssemblyHelper_LoadType()
        {
            var typeName = "HBD.Framework.Collections.SimpleMonitor";
            var type = AssemblyHelper.GetType(typeName);

            Assert.IsNotNull(type);
            Assert.IsTrue(type.FullName == typeName);
        }

        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_AssemblyHelper_LoadAssebmly()
        {
            var assName = "HBD.Framework";
            var assembly = AssemblyHelper.GetAssembly(assName);

            Assert.IsNotNull(assembly);
            Assert.IsNotNull(assembly.FullName == assName);
        }

        [TestMethod]
        [TestCategory("Fw.Core.AssemblyHelper")]
        public void Can_LoadAssebmly_FromFiles()
        {
            var t = Type.GetType("HBD.Framework.Collections.SimpleMonitor");
            Assert.IsNull(t);
            var t1 = AssemblyHelper.GetType("HBD.Framework.Collections.SimpleMonitor");
            Assert.IsNotNull(t1);
        }
    }
}