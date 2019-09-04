using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Storages.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.SQLite
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
            var record = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Text_Entry", "\'Some Text \'")
            };

            sql.AddData(record);
            sql.AddData(record);
            sql.AddData(record);
            sql.AddData(record);
            sql.AddData(record);
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
            var record = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Text_Entry", "\'Some Text \'")
            };

            sql.AddData(record);
            sql.AddData(record);
            sql.AddData(record);
            sql.AddData(record);
            var lastRecord = sql.AddData(record);


            sql.DeleteRecordById(lastRecord);
            var lines = sql.SelectAll();


            sql.DeleteDatabase();
            Assert.AreEqual(4, lines.Count());

        }
    }
}
