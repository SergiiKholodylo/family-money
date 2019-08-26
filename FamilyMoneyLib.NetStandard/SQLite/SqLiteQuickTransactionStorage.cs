using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteQuickTransactionStorage:QuickTransactionStorageBase
    {
        private const string QuickTransactionTableStructure = "id INTEGER PRIMARY KEY," +
                                                     "accountId INTEGER ," +
                                                     "categoryId INTEGER ," +
                                                     "name TEXT , " +
                                                     "total NUMERIC , " +
                                                     "ownerId TEXT NOT NULL, " +
                                                     "baseId TEXT NOT NULL, " +
                                                     "weight NUMERIC, " +
                                                     "askForTotal INTEGER, " +
                                                     "askForWeight INTEGER";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "QuickTransactions",
            $"({QuickTransactionTableStructure})");

        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;

        public SqLiteQuickTransactionStorage(IQuickTransactionFactory quickTransactionFactory, 
            IAccountStorage accountStorage, ICategoryStorage categoryStorage) : 
            base(quickTransactionFactory)
        {
            _accountStorage = accountStorage;
            _categoryStorage = categoryStorage;
        }

        public override IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction)
        {
            _table.InitializeDatabase();
            quickTransaction.Id = _table.AddData(ObjectToIQuickTransactionConverter.ConvertToKeyPairList(quickTransaction));
            return quickTransaction;
        }

        public override void DeleteQuickTransaction(IQuickTransaction quickTransaction)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(quickTransaction.Id);
        }

        public override IEnumerable<IQuickTransaction> GetAllQuickTransactions()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll().ToArray();
            return lines.Select(x => ObjectToIQuickTransactionConverter.Convert(x, _quickTransactionFactory, _accountStorage, _categoryStorage)).ToArray();
        }

        public override void UpdateQuickTransaction(IQuickTransaction quickTransaction)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIQuickTransactionConverter.ConvertToKeyPairList(quickTransaction), quickTransaction.Id);
            
        }

        public override void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
        }
    }

    public static class ObjectToIQuickTransactionConverter
    {
        public static IEnumerable<KeyValuePair<string, object>> 
            ConvertToKeyPairList(IQuickTransaction quickTransaction)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                /*
                 *"id INTEGER PRIMARY KEY," +
                                                     "accountId INTEGER NOT NULL," +
                                                     "categoryId INTEGER NOT NULL," +
                                                     "name TEXT NOT NULL, " +
                                                     "total NUMERIC NOT NULL, " +
                                                     "ownerId TEXT NOT NULL, " +
                                                     "baseId TEXT NOT NULL, " +
                                                     "weight NUMERIC, " +
                                                     "askForTotal INTEGER, " +
                                                     "askForWeight INTEGER";
                 *
                 */
                new KeyValuePair<string, object>("accountId", quickTransaction.Account?.Id),
                new KeyValuePair<string, object>("categoryId", quickTransaction.Category?.Id),
                new KeyValuePair<string, object>("name", quickTransaction.Name),
                new KeyValuePair<string, object>("total", quickTransaction.Total),
                new KeyValuePair<string, object>("ownerId", quickTransaction.OwnerId),
                new KeyValuePair<string, object>("baseId", quickTransaction.BaseId),
                new KeyValuePair<string, object>("weight", quickTransaction.Weight),
                new KeyValuePair<string, object>("askForTotal", quickTransaction.AskForTotal),
                new KeyValuePair<string, object>("askForWeight", quickTransaction.AskForWeight),
            };
            return returnList;
        }

        public static IQuickTransaction Convert(IDictionary<string, object> line, 
            IQuickTransactionFactory quickTransactionFactory, IAccountStorage accountStorage, 
            ICategoryStorage categoryStorage)
        {
            var id = (long)line["id"];
            var accountId = (long)(line["accountId"] is DBNull?0L: line["accountId"]);
            var categoryId = (long)(line["categoryId"] is DBNull ? 0L : line["categoryId"]);
            var name = line["name"].ToString();
            var total = decimal.Parse(line["total"].ToString());
            var account = accountStorage.GetAllAccounts().FirstOrDefault(x => x?.Id == accountId);
            var category = categoryStorage.GetAllCategories().FirstOrDefault(x => x?.Id == categoryId);
            var weight = decimal.Parse(line["weight"].ToString());
            var askForTotal = (long) line["askForTotal"] > 0;
            var askForWeight = (long)line["askForWeight"] > 0;

            var transaction = quickTransactionFactory.CreateQuickTransaction(account, category, name, total, id, weight, askForTotal, askForWeight);

            transaction.Id = id;

            return transaction;

        }
    }
}
