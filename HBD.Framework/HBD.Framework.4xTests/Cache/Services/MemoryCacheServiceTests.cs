#region using

using System;
using HBD.Framework.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Framework.Caching.Services;
using HBD.Framework.Caching;

#endregion

namespace HBD.Framework.Test.Cache
{
    [TestClass]
    public class MemoryCacheServiceTests
    {
        private MemoryCacheService _service;

        [TestInitialize]
        public void Initializer()
        {
            _service = new MemoryCacheService();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _service.Dispose();
        }

        private HasAttributeTestClass3 AddToCache(string key)
        {
            var obj = new HasAttributeTestClass3 {Prop3 = "Testing Adding"};
            _service.AddOrUpdate(key, obj);
            return obj;
        }

        private HasAttributeTestClass3 UpdateToCache(string key)
        {
            var obj = new HasAttributeTestClass3 {Prop3 = "Testing Updating"};
            _service.AddOrUpdate(key, obj);
            return obj;
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void Check_ExpiredTime()
        {
            CacheManager.Default.ExpirationTime = new TimeSpan(0, 0, 1);
            Assert.IsTrue(CacheManager.Default.ExpirationTime == new TimeSpan(0, 0, 1));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void Verify_CacheManager()
        {
            Assert.IsNotNull(CacheManager.Default);
            Assert.IsInstanceOfType(CacheManager.Default, typeof(MemoryCacheService));
        }


        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void AddOrUpdateTest()
        {
            //Add
            const string key = "test";
            var obj = AddToCache(key);

            var cached = _service.Get(key);
            Assert.IsNotNull(cached);
            Assert.IsTrue(_service.Get<HasAttributeTestClass3>(key).Prop3 == obj.Prop3);
            Assert.IsNull(_service.Get<HasAttributeTestClass2>(key));

            //Update
            obj = UpdateToCache(key);
            _service.AddOrUpdate(key, obj);

            cached = _service.Get(key);
            Assert.IsNotNull(cached);
            Assert.IsTrue(_service.Get<HasAttributeTestClass3>(key).Prop3 == obj.Prop3);
            Assert.IsNull(_service.Get<HasAttributeTestClass2>(key));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest()
        {
            _service.Dispose();
            var obj = AddToCache("test");
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void RemoveTest()
        {
            const string key = "test";
            var obj = AddToCache(key);
            Assert.IsTrue(_service.IsContains(key));
            _service.Remove(key);
            Assert.IsFalse(_service.IsContains(key));
        }
    }
}