using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteAccountStorage : IAccountStorage
    {
        /*
         Account Table Structure

            id INTEGER PRIMARY KEY,
            timestamp TEXT NOT NULL, 
            name TEXT NOT NULL, 
            description TEXT NOT NULL, 
            currency TEXT NOT NULL
         */


        // Exception Microsoft.Data.Sqlite.SqliteException

        private const string AccountTableStructure = "id INTEGER PRIMARY KEY," +
                                                     "timestamp TEXT NOT NULL," +
                                                     "name TEXT NOT NULL, " +
                                                     "description TEXT NOT NULL," +
                                                     "currency TEXT NOT NULL";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Accounts",
            $"({AccountTableStructure})");

        public IAccount CreateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            account.Id = _table.AddData(ObjectToIAccountConverter.ConvertForInsertString(account));
            return account;
        }

        public  void DeleteAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(account.Id);
        }

        public  void UpdateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIAccountConverter.ConvertForUpdateString(account),account.Id);

        }

        public IEnumerable<IAccount> GetAllAccounts(IAccountFactory factory)
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll();

            return lines.Select(ObjectToIAccountConverter.Convert).ToList();
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToIAccountConverter
    {
        public static IAccount Convert(object[] line)
        {
            var factory = new RegularAccountFactory();
            var account = factory.CreateAccount(line[2].ToString(), line[3].ToString(), line[4].ToString());
            account.Id = (long) line[0];
            account.Timestamp = DateTime.Parse(line[1].ToString());

            return account;
        }

        public static string ConvertForUpdateString(IAccount account)
        {
            var sqlDataString =
                $"timestamp = '{account.Timestamp}',name = '{account.Name}', description = '{account.Description}',currency = '{account.Currency}'";
            return sqlDataString;
        }

        public static string ConvertForInsertString(IAccount account)
        {
            var sqlDataString =
                $"NULL,'{account.Timestamp}','{account.Name}', '{account.Description}','{account.Currency}'";
            return sqlDataString;
        }
    }
}
