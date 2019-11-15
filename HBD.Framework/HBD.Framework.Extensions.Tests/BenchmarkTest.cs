using HBD.EntityFrameworkCore.Extensions.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BenchmarkDotNet.Running;

namespace HBD.Framework.Extensions.Tests
{
    [TestClass]
    public class BenchmarkTest
    {
        #region Public Methods

        
        [TestMethod]
        public void Test_TypeExtractor()
        {
            var summary = BenchmarkRunner.Run<TestTypeExtractorExtensions>();
        }

        #endregion Public Methods
    }
}