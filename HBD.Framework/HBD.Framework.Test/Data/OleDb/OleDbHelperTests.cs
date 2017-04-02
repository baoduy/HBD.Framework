#region using

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Framework.Data.OleDb.Tests
{
    [TestClass]
    public class OleDbHelperTests
    {
        /// <summary>
        ///     Running with 64x if PC is 64x.
        /// </summary>
        [TestMethod]
        [TestCategory("Fw.Data.OleDb")]
        public void Can_ReadExcel_To_DataTable_Test()
        {
            const string connString =
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=TestData\\2015 Weekly Calendar.xlsx;Extended Properties = \"Excel 12.0 Xml;HDR=NO\";";
            using (var conn = new OleDbAdapter(connString))
            {
                var dt = conn.ExecuteTable("SELECT * FROM [2015 Weekly Calendar$]");
                Assert.IsNotNull(dt);
            }
        }
    }
}