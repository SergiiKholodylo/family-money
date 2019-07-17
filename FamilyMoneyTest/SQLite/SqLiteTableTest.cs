using System.Linq;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.SQLite
{
    [TestClass]
    public class SqLiteTableTest
    {
        [TestMethod]
        public void SqLiteInitDatabaseTest()
        {
            var sql = new SqLiteTable("familyMoney.db", "testBase",
                "(id INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)");


            sql.InitializeDatabase();


            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SqLiteDeleteDatabaseTest()
        {
            var sql = new SqLiteTable("familyMoney.db", "testBase",
                "(id INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)");
            sql.InitializeDatabase();


            sql.DeleteDatabase();


            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AddDataTest()
        {
            var sql = new SqLiteTable("familyMoney.db", "testBase",
                "(id INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)");
            
            sql.InitializeDatabase();
            sql.DeleteDatabase();
            sql.InitializeDatabase();

            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            var lines = sql.SelectAll();

            sql.DeleteDatabase();
            Assert.AreEqual(5,lines.Count());

        }


        [TestMethod]
        public void DeleteRecordByIdTest()
        {
            var sql = new SqLiteTable("familyMoney.db", "testBase",
                "(id INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)");

            sql.InitializeDatabase();
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            sql.AddData("NULL,'test'");
            var lastRecord = sql.AddData("NULL,'test'");


            sql.DeleteRecordById(lastRecord);
            var lines = sql.SelectAll();


            sql.DeleteDatabase();
            Assert.AreEqual(4, lines.Count());

        }
    }
}
