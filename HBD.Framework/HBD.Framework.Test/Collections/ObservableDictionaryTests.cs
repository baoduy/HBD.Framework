#region

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Collections.Tests
{
    [TestClass]
    public class ObservableDictionaryTests
    {
        [TestMethod]
        public void ObservableDictionary_NonParameter_Contructor_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;
            var coll = new ObservableDictionary<string, object>();

            coll.CollectionChanged += (s, e) =>
            {
                if (e.NewItems?.Cast<KeyValuePair<string, object>>().Count() == 1)
                    isChangedCalled++;
            };
            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            coll.Add("1", new object());

            //There are 4 properties will be called:[Count,Indexer,KeysName and ValuesName]
            Assert.IsTrue(isPropertyCalled == 4);
            Assert.IsTrue(isChangedCalled == 1);
        }

        [TestMethod]
        public void ObservableDictionary_Dictionary_Contructor_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.CollectionChanged += (s, e) =>
            {
                if (e.NewItems?.Cast<KeyValuePair<string, object>>().Count() == 1)
                    isChangedCalled++;
            };
            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            coll.Add("2", new object());

            Assert.IsTrue(isPropertyCalled == 4);
            Assert.IsTrue(isChangedCalled == 1);
            Assert.IsTrue(coll.Count == 2);
        }

        [TestMethod]
        public void ObservableDictionary_Replace_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;

            var dic = new Dictionary<string, object>
            {
                ["1"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Replace
                    && e.NewItems?.Cast<KeyValuePair<string, object>>().Count() == 1
                    && e.OldItems?.Cast<KeyValuePair<string, object>>().Count() == 1)

                    isChangedCalled++;
            };
            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            coll["1"] = new object();

            //There are 4 properties will be called:[Count,Indexer,KeysName and ValuesName]
            Assert.IsTrue(isPropertyCalled == 4);
            Assert.IsTrue(isChangedCalled == 1);
        }

        [TestMethod]
        public void ObservableDictionary_Replace_NewKeyValue_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;

            var dic = new Dictionary<string, object>
            {
                ["1"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add
                    && e.NewItems?.Cast<KeyValuePair<string, object>>().Count() == 1)

                    isChangedCalled++;
            };
            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            coll["2"] = new object();

            //There are 4 properties will be called:[Count,Indexer,KeysName and ValuesName]
            Assert.IsTrue(isPropertyCalled == 4);
            Assert.IsTrue(isChangedCalled == 1);
        }

        [TestMethod]
        public void ObservableDictionary_Remove_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;

            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove
                    && e.OldItems?.Cast<KeyValuePair<string, object>>().Count() == 1)

                    isChangedCalled++;
            };
            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            Assert.IsTrue(coll.Remove("2"));

            //There are 4 properties will be called:[Count,Indexer,KeysName and ValuesName]
            Assert.IsTrue(isPropertyCalled == 4);
            Assert.IsTrue(isChangedCalled == 1);
        }

        [TestMethod]
        public void ObservableDictionary_Remove_NewKey_Test()
        {
            var isPropertyCalled = 0;
            var isChangedCalled = 0;

            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.CollectionChanged += (s, e) => isChangedCalled++;

            coll.PropertyChanged += (s, e) => isPropertyCalled++;

            Assert.IsFalse(coll.Remove("4"));

            //There are 4 properties will be called:[Count,Indexer,KeysName and ValuesName]
            Assert.IsTrue(isPropertyCalled == 0);
            Assert.IsTrue(isChangedCalled == 0);
        }

        [TestMethod]
        public void ContainsKeyTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            Assert.IsTrue(coll.ContainsKey("1"));
        }

        [TestMethod]
        public void RemoveTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.Remove("1");
            Assert.IsFalse(coll.ContainsKey("1"));
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            object val = null;
            coll.TryGetValue("2", out val);

            Assert.IsNotNull(val);

            val = null;
            coll.Remove("3");
            coll.TryGetValue("3", out val);

            Assert.IsNull(val);
        }

        [TestMethod]
        public void ClearTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            coll.Clear();

            Assert.IsTrue(coll.Count == 0);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            Assert.IsTrue(coll.Contains(coll.First()));
            Assert.IsTrue(coll.Contains(coll.Last()));
        }

        [TestMethod]
        public void CopyToTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);
            var array = new KeyValuePair<string, object>[coll.Count];
            coll.CopyTo(array, 0);

            Assert.IsTrue(array.All(i => i.Key.IsNotNull() && i.Value.IsNotNull()));
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            var dic = new Dictionary<string, object>
            {
                ["1"] = new object(),
                ["2"] = new object(),
                ["3"] = new object()
            };
            var coll = new ObservableDictionary<string, object>(dic);

            Assert.IsTrue(coll.All(i => i.Key.IsNotNull() && i.Value.IsNotNull()));
        }
    }
}