﻿#region using

using System;
using System.Threading;
using HBD.Framework.Cache;
using HBD.Framework.Cache.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Test.Cache
{
    [TestClass]
    public class CacheManagerTests
    {
        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Instance()
        {
            Assert.IsNotNull(CacheManager.Default);
            Assert.IsInstanceOfType(CacheManager.Default, typeof(MemoryCacheService));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Region_Instance()
        {
            CacheManager.Default.AddOrUpdate("123", new object(), "A");
            Assert.IsNull(CacheManager.Default.Get("123"));
            Assert.IsNull(CacheManager.Default.Get("123", "B"));
            Assert.IsNotNull(CacheManager.Default.Get("123", "A"));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Expiry_Instance()
        {
            //Cache 5 secs
            CacheManager.Default.AddOrUpdate("123", new object(), new TimeSpan(0, 0, 5));
            Assert.IsNotNull(CacheManager.Default.Get("123"));

            //Delay 6 secs Cache item should be null.
            Thread.Sleep(new TimeSpan(0, 0, 6));
            Assert.IsNull(CacheManager.Default.Get("123"));
        }
    }
}