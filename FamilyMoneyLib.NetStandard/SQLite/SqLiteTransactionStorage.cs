using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteTransactionStorage : TransactionStorageBase
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
                                                     "total NUMERIC NOT NULL, " +
                                                     "ownerId TEXT NOT NULL, " +
                                                     "baseId TEXT NOT NULL, "+
                                                     "weight NUMERIC,"+
                                                     "productId INTEGER";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Transactions",
            $"({AccountTableStructure})");

        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;



        public SqLiteTransactionStorage(ITransactionFactory transactionFactory,SqLiteAccountStorage accountStorage, SqLiteCategoryStorage categoryStorage, RegularAccountFactory accountFactory, RegularCategoryFactory categoryFactory):base(transactionFactory)
        {
            _accountStorage = accountStorage;
            _categoryStorage = categoryStorage;
        }

        public SqLiteTransactionStorage(ITransactionFactory transactionFactory) : base(transactionFactory)
        {
        }

        public override ITransaction CreateTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            transaction.Id = _table.AddData(ObjectToITransactionConverter.ConvertToKeyPairList(transaction));
            return transaction;
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(transaction.Id);
        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll();

            var response = lines.Select(x => ObjectToITransactionConverter.Convert(x, TransactionFactory, _accountStorage, _categoryStorage)).OrderByDescending(x => x.Timestamp).ToList();

            return response;
        }

        public override void UpdateTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToITransactionConverter.ConvertToKeyPairList(transaction), transaction.Id);
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToITransactionConverter
    {
        public static List<KeyValuePair<string, object>> ConvertToKeyPairList(ITransaction transaction)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("timestamp", transaction.Timestamp),
                new KeyValuePair<string, object>("accountId", transaction.Account.Id),
                new KeyValuePair<string, object>("categoryId", transaction.Category.Id),
                new KeyValuePair<string, object>("name", transaction.Name),
                new KeyValuePair<string, object>("total", transaction.Total),
                new KeyValuePair<string, object>("ownerId", transaction.OwnerId),
                new KeyValuePair<string, object>("baseId", transaction.BaseId),
                new KeyValuePair<string, object>("weight", transaction.Weight),
                new KeyValuePair<string, object>("productId", transaction.Product?.Id),
            };
            return returnList;
        }

        public static ITransaction Convert(object[] line, ITransactionFactory transactionFactory,
            IAccountStorage accountStorage, ICategoryStorage categoryStorage)
        {
            var id = (long) line[0];
            var timestamp = DateTime.Parse(line[1].ToString());
            var accountId = (long)(line[2]);
            var categoryId = (long)(line[3]);
            var name = line[4].ToString();
            var total = decimal.Parse(line[5].ToString());
            var account = accountStorage.GetAllAccounts().FirstOrDefault(x => x.Id == accountId);
            var category = categoryStorage.GetAllCategories().FirstOrDefault(x => x.Id == categoryId);
            //var weight = line.
            var transaction = transactionFactory.CreateTransaction(account,category,name,total);
            transaction.Id = id;
            transaction.Timestamp = timestamp;

            return transaction;
        }
    }
}
