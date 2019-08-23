using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    public class SqLiteAccountStorage : AccountStorageBase,IAccountStorage
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

        private bool _isDirty = true;
        private IEnumerable<IAccount> _cache;

        public SqLiteAccountStorage(IAccountFactory factory) : base(factory)
        {

        }
        


        public override IAccount CreateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            account.Id = _table.AddData(ObjectToIAccountConverter.ConvertToKeyPairList(account));
            _isDirty = true;
            return account;
        }

        public override void DeleteAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(account.Id);
            _isDirty = true;
        }

        public override void UpdateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIAccountConverter.ConvertToKeyPairList(account),account.Id);
            _isDirty = true;

        }

        public override IEnumerable<IAccount> GetAllAccounts()
        {
            if (!_isDirty) return _cache;

            _table.InitializeDatabase();
            var lines = _table.SelectAll();
            _cache = lines.Select(objects => ObjectToIAccountConverter.Convert(objects, AccountFactory)).ToList();
            _isDirty = false;
            return _cache;
        }

        public void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
            _isDirty = true;
        }
    }

    public static class ObjectToIAccountConverter
    {
        public static IAccount Convert(IDictionary<string, object> line, IAccountFactory accountFactory)
        {
            var name = line["name"].ToString();
            var description = line["description"].ToString();
            var currency = line["currency"].ToString();
            var account = accountFactory.CreateAccount(name,description,currency);
            account.Id = (long) line["id"];
            account.Timestamp = DateTime.Parse(line["timestamp"].ToString());

            return account;
        }

        public static IEnumerable<KeyValuePair<string, object>> ConvertToKeyPairList(IAccount account)
        {
            var returnList = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("timestamp", account.Timestamp),
                new KeyValuePair<string, object>("name", account.Name),
                new KeyValuePair<string, object>("description", account.Description),
                new KeyValuePair<string, object>("currency", account.Currency)
            };
            return returnList;
        }
    }
}
