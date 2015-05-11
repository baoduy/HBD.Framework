using HBD.Framework.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HBD.Framework.Data.Utilities;
using System.Reflection;
using System.Collections.Generic;

namespace HBD.Test.Framework.Extension
{


    /// <summary>
    ///This is a test class for ExtensionTest and is intended
    ///to contain all ExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest1()
        {
            object objA = "123"; // TODO: Initialize to an appropriate value
            object objB = "123"; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest2()
        {
            object objA = 123; // TODO: Initialize to an appropriate value
            object objB = 123; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest3()
        {
            object objA = "123"; // TODO: Initialize to an appropriate value
            object objB = "124"; // TODO: Initialize to an appropriate value
            int expected = -1; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest4()
        {
            object objA = 123; // TODO: Initialize to an appropriate value
            object objB = 124; // TODO: Initialize to an appropriate value
            int expected = -1; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest5()
        {
            object objA = "123"; // TODO: Initialize to an appropriate value
            object objB = "122"; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        public void CompareTest6()
        {
            object objA = 123; // TODO: Initialize to an appropriate value
            object objB = 122; // TODO: Initialize to an appropriate value
            int expected = 1; // TODO: Initialize to an appropriate value
            int actual;
            actual = objA.Compare(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateInstance
        ///</summary>
        [TestMethod()]
        public void CreateInstanceTest()
        {
            Type type = typeof(Object); // TODO: Initialize to an appropriate value
            object actual;
            actual = type.CreateInstance();
            Assert.IsTrue(actual is Object);
        }

        /// <summary>
        ///A test for GetBasicCharacters
        ///</summary>
        [TestMethod()]
        public void GetBasicCharactersTest()
        {
            string value = "abc#123$%^&*()okp\\//__(){}"; // TODO: Initialize to an appropriate value
            string expected = "abc123okp"; // TODO: Initialize to an appropriate value
            string actual;
            actual = value.GetAlphabetCharacters();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsContains
        ///</summary>
        [TestMethod()]
        public void IsContainsTest1()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "123"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsContains(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsContains
        ///</summary>
        [TestMethod()]
        public void IsContainsTest2()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "78"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsContains(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsContains
        ///</summary>
        [TestMethod()]
        public void IsContainsTest3()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "abc"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsContains(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEndsWith
        ///</summary>
        [TestMethod()]
        public void IsEndsWithTest1()
        {
            object objA = 1234567890; // TODO: Initialize to an appropriate value
            object objB = 90; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEndsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEndsWith
        ///</summary>
        [TestMethod()]
        public void IsEndsWithTest2()
        {
            object objA = 1234567890; // TODO: Initialize to an appropriate value
            object objB = 91; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEndsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEndsWith
        ///</summary>
        [TestMethod()]
        public void IsEndsWithTest3()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "90"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEndsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEndsWith
        ///</summary>
        [TestMethod()]
        public void IsEndsWithTest4()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "91"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEndsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEndsWith
        ///</summary>
        [TestMethod()]
        public void IsEndsWithTest5()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = 90; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEndsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEquals
        ///</summary>
        [TestMethod()]
        public void IsEqualsTest1()
        {
            object objA = "abc"; // TODO: Initialize to an appropriate value
            object objB = "abc"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEquals(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEquals
        ///</summary>
        [TestMethod()]
        public void IsEqualsTest2()
        {
            object objA = "abc"; // TODO: Initialize to an appropriate value
            object objB = "acb"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEquals(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEquals
        ///</summary>
        [TestMethod()]
        public void IsEqualsTest3()
        {
            object objA = 123; // TODO: Initialize to an appropriate value
            object objB = 123; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEquals(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsEquals
        ///</summary>
        [TestMethod()]
        public void IsEqualsTest4()
        {
            object objA = 145; // TODO: Initialize to an appropriate value
            object objB = 144; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsEquals(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsIn
        ///</summary>
        [TestMethod()]
        public void IsInTest1()
        {
            object objA = 12; // TODO: Initialize to an appropriate value
            object objB = new List<int>() { 1, 2, 3, 4, 11, 12, 13 }; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsIn(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsIn
        ///</summary>
        [TestMethod()]
        public void IsInTest2()
        {
            object objA = "2"; // TODO: Initialize to an appropriate value
            object objB = new List<string>() { "1", "2", "3", "4", "11", "12", "13" }; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsIn(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsIn
        ///</summary>
        [TestMethod()]
        public void IsInTest3()
        {
            object objA = 0; // TODO: Initialize to an appropriate value
            object objB = new List<int>() { 1, 2, 3, 4, 11, 12, 13 }; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsIn(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsIn
        ///</summary>
        [TestMethod()]
        public void IsInTest4()
        {
            object objA = "22"; // TODO: Initialize to an appropriate value
            object objB = new List<string>() { "1", "2", "3", "4", "11", "12", "13" }; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsIn(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStartsWith
        ///</summary>
        [TestMethod()]
        public void IsStartsWithTest1()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = 123; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsStartsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStartsWith
        ///</summary>
        [TestMethod()]
        public void IsStartsWithTest2()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "123"; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsStartsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStartsWith
        ///</summary>
        [TestMethod()]
        public void IsStartsWithTest3()
        {
            object objA = 1234567890; // TODO: Initialize to an appropriate value
            object objB = 123; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsStartsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStartsWith
        ///</summary>
        [TestMethod()]
        public void IsStartsWithTest4()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = 111; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsStartsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStartsWith
        ///</summary>
        [TestMethod()]
        public void IsStartsWithTest5()
        {
            object objA = "1234567890"; // TODO: Initialize to an appropriate value
            object objB = "111"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = objA.IsStartsWith(objB);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValueType
        ///</summary>
        [TestMethod()]
        public void IsValueTypeTest1()
        {
            object obj = 123; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = obj.IsStringOrValueType();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValueType
        ///</summary>
        [TestMethod()]
        public void IsValueTypeTest2()
        {
            object obj = "123"; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = obj.IsStringOrValueType();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValueType
        ///</summary>
        [TestMethod()]
        public void IsValueTypeTest3()
        {
            object obj = '2'; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = obj.IsStringOrValueType();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsValueType
        ///</summary>
        [TestMethod()]
        public void IsValueTypeTest4()
        {
            object obj = FilterManager.CreateFilterClause("A", CompareOperation.Equals, null); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = obj.IsStringOrValueType();
            Assert.AreEqual(expected, actual);
        }
    }
}
