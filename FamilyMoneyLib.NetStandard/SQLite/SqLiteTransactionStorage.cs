using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteTransactionStorage : ITransactionStorage
    {
        /*
         Account Table Structure

            id INTEGER PRIMARY KEY,
            timestamp TEXT NOT NULL, 
            accountId INTEGER NOT NULL,
            categoryId INTEGER NOT NULL,
            name TEXT NOT NULL, 
            total NUMERIC NOT NULL
         */


        // Exception Microsoft.Data.Sqlite.SqliteException

        private const string AccountTableStructure = "id INTEGER PRIMARY KEY," +
                                                     "timestamp TEXT NOT NULL," +
                                                     "accountId INTEGER NOT NULL,"+
                                                     "categoryId INTEGER NOT NULL,"+
                                                     "name TEXT NOT NULL, " +
                                                     "total NUMERIC NOT NULL";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Transactions",
            $"({AccountTableStructure})");

        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;

        public SqLiteTransactionStorage(IAccountStorage accountStorage, ICategoryStorage categoryStorage)
        {
            _accountStorage = accountStorage;
            _categoryStorage = categoryStorage;
        }

        public ITransaction CreateTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            transaction.Id = _table.AddData(ObjectToITransactionConverter.ConvertForInsertString(transaction));
            return transaction;
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(transaction.Id);
        }

        public void UpdateTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToITransactionConverter.ConvertForUpdateString(transaction), transaction.Id);
        }

        public IEnumerable<ITransaction> GetAllTransactions()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll();

            var response = lines.Select(x=>ObjectToITransactionConverter.Convert(x,_accountStorage,_categoryStorage)).ToList();

            return response;
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToITransactionConverter
    {
        public static string ConvertForInsertString(ITransaction transaction)
        {
            var sqlDataString =
                $"NULL,'{transaction.Timestamp}','{transaction.Account.Id}','{transaction.Category.Id}','{transaction.Name}', '{transaction.Total}'";
            return sqlDataString;
        }

        public static string ConvertForUpdateString(ITransaction transaction)
        {
            var sqlDataString =
                $"timestamp = '{transaction.Timestamp}',accountId = '{transaction.Account.Id}',categoryId = '{transaction.Category.Id}',name = '{transaction.Name}', total = '{transaction.Total}'";
            return sqlDataString;
        }

        public static ITransaction Convert(object[] line, IAccountStorage accountStorage, ICategoryStorage categoryStorage)
        {
            var factory = new RegularTransactionFactory();
            var id = (long) line[0];
            var timestamp = DateTime.Parse(line[1].ToString());
            var accountId = (long)(line[2]);
            var categoryId = (long)(line[3]);
            var name = line[4].ToString();
            var total = decimal.Parse(line[5].ToString());
            var account = accountStorage.GetAllAccounts().FirstOrDefault(x => x.Id == accountId);
            var category = categoryStorage.GetAllCategories().FirstOrDefault(x => x.Id == categoryId);
            var transaction = factory.CreateTransaction(account,category,name,total);
            transaction.Id = id;
            transaction.Timestamp = timestamp;

            return transaction;
        }
    }
}
