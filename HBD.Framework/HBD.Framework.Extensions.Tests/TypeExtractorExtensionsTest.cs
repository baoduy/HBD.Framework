using System;
using System.ComponentModel;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using HBD.Framework.Extensions;
using HBD.Framework.Extensions.Core;
using HBD.Framework.Extensions.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.EntityFrameworkCore.Extensions.Tests
{
    [TestClass]
    public class TestTypeExtractorExtensions
    {
        #region Public Methods

        [TestMethod]
        [Benchmark]
        public void Test_Abstract()
        {
            typeof(TestEnum).Assembly.Extract().Abstract()
                .Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_HasAttribute()
        {
            typeof(TestEnum).Assembly.Extract().HasAttribute<DisplayNameAttribute>()
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_Interface()
        {
            typeof(TestEnum).Assembly.Extract().IsInstanceOf<ITem>()
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_Nested()
        {
            typeof(TestEnum).Assembly.Extract().Nested()
                .Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_NotClass()
        {
            typeof(TestEnum).Assembly.Extract().NotClass()
                .Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_NotEnum()
        {
            typeof(TestEnum).Assembly.Extract().NotEnum()
                .Should().HaveCountGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void TestExtract()
        {
            typeof(TestEnum).Assembly.Extract().Public().Class().Count()
                .Should().BeGreaterOrEqualTo(3);
        }

        [TestMethod]
        [Benchmark]
        public void TestExtract_GenericClass()
        {
            var list = typeof(TestEnum).Assembly.ScanGenericClassesWithFilter("Generic").ToList();
            list.Any().Should().BeTrue();
        }

        [TestMethod]
        [Benchmark]
        public void TestExtract_NotInstanceOf()
        {
            var list = typeof(TestEnum).Assembly.Extract().Class().NotInstanceOf(typeof(ITem)).ToList();
            list.Contains(typeof(TestItem)).Should().BeFalse();
            list.Contains(typeof(TestItem2)).Should().BeFalse();
        }

        [TestMethod]
        [Benchmark]
        public void TestScanClassesFromWithFilter()
        {
            typeof(TestEnum).Assembly.ScanClassesWithFilter("Item")
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void TestScanClassesImplementOf()
        {
            typeof(TestEnum).Assembly.ScanClassesImplementOf(typeof(IDisposable))
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void TestScanClassesImplementOf_Generic()
        {
            typeof(TestEnum).Assembly.ScanClassesImplementOf<GenericClassItem<TestItem>>()
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void TestScanPublicClassesFromWithFilter()
        {
            typeof(TestEnum).Assembly.ScanPublicClassesWithFilter("ConfigItem")
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void TestScanPublicClassesImplementOf()
        {
            typeof(TestEnum).Assembly.ScanPublicClassesImplementOf<IConfigItem>()
                .Count().Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        [Benchmark]
        public void Test_Duplicate_Assemblies()
        {
            new[] { typeof(ITypeExtractor).Assembly, typeof(ITypeExtractor).Assembly }
                .Extract().IsInstanceOf<ITypeExtractor>()
                 .Count().Should().BeGreaterOrEqualTo(1);
        }
        #endregion Public Methods
    }
}