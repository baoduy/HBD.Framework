using HBD.Framework.Data.Sharepoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HBD.Framework.Data.Utilities;
using System.Data;
using HBD.Framework.Data.Sharepoint.Client2010;
using Microsoft.SharePoint.Client;

namespace HBD.Test.Framework.Data.Sharepoint
{


    /// <summary>
    ///This is a test class for SPClientAdapterTest and is intended
    ///to contain all SPClientAdapterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SPClientAdapterTest
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

        const string siteURL = "http://cnbconnectuat/";
        const string listTitle = "Staff Directory";

        /// <summary>
        ///A test for SPClientAdapter Constructor
        ///</summary>
        [TestMethod()]
        public void SPClientAdapterConstructorTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ConvertToDataTable
        ///</summary>
        [TestMethod()]
        public void ConvertToDataTableTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            IFilterClause filterClause = FilterManager.Contains("Personal_x0020_Name", "Hoang"); // TODO: Initialize to an appropriate value
            string[] fields = new string[] { "ID", "Personal_x0020_Name" }; // TODO: Initialize to an appropriate value
            DataTable actual;
            actual = target.ToDataTable(listTitle, filterClause, fields);
            Assert.AreEqual(actual.TableName, listTitle);
            Assert.IsTrue(actual.Rows.Count == 1);
            Assert.IsTrue(actual.Columns.Contains(fields[0]) && actual.Columns.Contains(fields[1]));
        }

        /// <summary>
        ///A test for ConvertToDataTable
        ///</summary>
        [TestMethod()]
        public void ConvertToDataTableTest1()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            string viewTitle = target.GetViewTitles(listTitle)[0]; // TODO: Initialize to an appropriate value
            DataTable actual;
            actual = target.ToDataTable(listTitle, viewTitle);
            Assert.AreEqual(actual.TableName, listTitle);
            Assert.IsTrue(actual.Rows.Count > 0);
        }

        /// <summary>
        ///A test for ConvertToDataTable
        ///</summary>
        [TestMethod()]
        public void ConvertToDataTableTest2()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            DataTable actual;
            actual = target.ToDataTable(listTitle);
            Assert.AreEqual(actual.TableName, listTitle);
            Assert.IsTrue(actual.Rows.Count > 0);
        }

        /// <summary>
        ///A test for GetAccounts
        ///</summary>
        [TestMethod()]
        public void GetAccountsTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            string groupName = target.SiteGroups[0]; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetUserNames(groupName);
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for GetAllAccounts
        ///</summary>
        [TestMethod()]
        public void GetAllAccountsTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetAllUserNames();
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for GetList
        ///</summary>
        [TestMethod()]
        public void GetListTest()
        {
            // TODO: Initialize to an appropriate value
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            List actual;
            actual = target.GetList(listTitle);
            Assert.AreEqual(listTitle, actual.Title);
        }

        /// <summary>
        ///A test for GetListTitles
        ///</summary>
        [TestMethod()]
        [DeploymentItem("HBD.Framework.Data.Sharepoint.dll")]
        public void GetListTitlesTest()
        {
            SPClientAdapter_Accessor target = new SPClientAdapter_Accessor(siteURL); // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetListTitles();
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for GetSiteGroups
        ///</summary>
        [TestMethod()]
        [DeploymentItem("HBD.Framework.Data.Sharepoint.dll")]
        public void GetSiteGroupsTest()
        {
            SPClientAdapter_Accessor target = new SPClientAdapter_Accessor(siteURL); // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetSiteGroups();
            Assert.IsTrue(actual.Length>0);
        }

        /// <summary>
        ///A test for GetViewTitles
        ///</summary>
        [TestMethod()]
        public void GetViewTitlesTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.GetViewTitles(listTitle);
            Assert.IsTrue(actual != null);
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            SPClientAdapter target = new SPClientAdapter(siteURL); // TODO: Initialize to an appropriate value
            int itemID = 2000; // TODO: Initialize to an appropriate value
            string fieldName = "Personal_x0020_Name"; // TODO: Initialize to an appropriate value
            object value = "Hoang"; // TODO: Initialize to an appropriate value
            var actual = target.Update(listTitle, itemID, fieldName, value);
            var item = target.GetItemByID(listTitle, itemID);
            Assert.IsTrue( actual);
            Assert.IsNotNull(item);
            Assert.IsTrue(item[fieldName].Equals(value));
        }
    }
}
