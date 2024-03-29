﻿using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Storages.SQLite
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

        public SqLiteAccountStorage(IAccountFactory factory) : base(factory)
        {

        }
        


        public override IAccount CreateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            if(account.Id==0)
                account.Id = _table.AddData(ObjectToIAccountConverter.ConvertToKeyPairList(account));
            else
                _table.AddData(ObjectToIAccountConverter.ConvertToKeyPairListWithId(account));
            return account;
        }

        public override void DeleteAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.DeleteRecordById(account.Id);
            
        }

        public override void UpdateAccount(IAccount account)
        {
            _table.InitializeDatabase();
            _table.UpdateData(ObjectToIAccountConverter.ConvertToKeyPairList(account),account.Id);
        }

        public override IEnumerable<IAccount> GetAllAccounts()
        {
            

            _table.InitializeDatabase();
            var lines = _table.SelectAll();
            return lines.Select(objects => ObjectToIAccountConverter.Convert(objects, AccountFactory)).ToList();
            
        }

        public override void DeleteAllData()
        {
            _table.InitializeDatabase();
            _table.DeleteDatabase();
            
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

        public static IEnumerable<KeyValuePair<string, object>> ConvertToKeyPairListWithId(IAccount account)
        {
            var list = ConvertToKeyPairList(account).ToList();
            list.Add(new KeyValuePair<string, object>("id", account.Id));
            return list;
        }
    }
}
