﻿using System;
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
                                                     "weight NUMERIC, "+
                                                     "productId INTEGER, "+
                                                     "isComplexTransaction INTEGER NOT NULL, " +
                                                     "parentId INTEGER";

        private readonly SqLiteTable _table = new SqLiteTable("familyMoney.db", "Transactions",
            $"({AccountTableStructure})");

        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;



        public SqLiteTransactionStorage(ITransactionFactory transactionFactory, 
            IAccountStorage accountStorage,
            ICategoryStorage categoryStorage):base(transactionFactory)
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
            foreach (var childrenTransaction in transaction.ChildrenTransactions)
            {
                _table.AddData(ObjectToITransactionConverter.ConvertToKeyPairList(childrenTransaction));
            }

            return transaction;
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            var allData = GetAllTransactions();
            var children = allData.Where(x => x.ParentTransaction?.Id == transaction.Id);
            foreach (var child in children)
            {
                _table.DeleteRecordById(child.Id);
            }
            _table.DeleteRecordById(transaction.Id);
        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            _table.InitializeDatabase();
            var lines = _table.SelectAll().ToArray();

            var response = lines.Select(x => ObjectToITransactionConverter.Convert(x, TransactionFactory, _accountStorage, _categoryStorage)).OrderByDescending(x => x.Timestamp).ToArray();
            foreach (var line in lines)
            {
                ObjectToITransactionConverter.UpdateParents(line, response);
            }
            return response;
        }

        public override void UpdateTransaction(ITransaction transaction)
        {
            _table.InitializeDatabase();
            var allData = GetAllTransactions();
            //var children = allData.Where(x => x.ParentTransaction?.Id == transaction.Id);
            //foreach (var child in children)
            //{
            //    _table.DeleteRecordById(child.Id);
            //}

            _table.UpdateData(ObjectToITransactionConverter.ConvertToKeyPairList(transaction), transaction.Id);
            //foreach (var childrenTransaction in transaction.ChildrenTransactions)
            //{
            //    _table.AddData(ObjectToITransactionConverter.ConvertToKeyPairList(childrenTransaction));
            //}
        }

        public override void AddChildrenTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAllChildrenTransactions()
        {
            throw new NotImplementedException();
        }

        public override void DeleteChildrenTransaction(ITransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void AddChildrenTransaction(ITransaction parent, ITransaction child)
        {
            var transaction = (Transaction) parent;
            child.ParentTransaction = parent;
            transaction.AddChildrenTransaction(child);
            UpdateTransaction(child);
            UpdateTransaction(transaction);
        }

        public void DeleteAllChildrenTransactions(Transaction parent)
        {
            parent.DeleteAllChildrenTransactions();
        }

        public void DeleteChildrenTransaction(Transaction parent, Transaction child)
        {
            parent.DeleteChildrenTransaction(child);
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
                new KeyValuePair<string, object>("accountId", transaction.Account?.Id),
                new KeyValuePair<string, object>("categoryId", transaction.Category?.Id),
                new KeyValuePair<string, object>("name", transaction.Name),
                new KeyValuePair<string, object>("total", transaction.Total),
                new KeyValuePair<string, object>("ownerId", transaction.OwnerId),
                new KeyValuePair<string, object>("baseId", transaction.BaseId),
                new KeyValuePair<string, object>("weight", transaction.Weight),
                new KeyValuePair<string, object>("productId", transaction.Product?.Id),
                new KeyValuePair<string, object>("isComplexTransaction", transaction.IsComplexTransaction),
                new KeyValuePair<string, object>("parentId", transaction.ParentTransaction?.Id),
            };
            return returnList;
        }

        public static ITransaction Convert(IDictionary<string, object> line, ITransactionFactory transactionFactory,
            IAccountStorage accountStorage, ICategoryStorage categoryStorage)
        {
            var id = (long) line["id"];
            var timestamp = DateTime.Parse(line["timestamp"].ToString());
            var accountId = (long)(line["accountId"]);
            var categoryId = (long)(line["categoryId"]);
            var name = line["name"].ToString();
            var total = decimal.Parse(line["total"].ToString());
            var account = accountStorage.GetAllAccounts().FirstOrDefault(x => x.Id == accountId);
            var category = categoryStorage.GetAllCategories().FirstOrDefault(x => x.Id == categoryId);
            var weight = decimal.Parse( line["weight"].ToString());
            var productId = (line["productId"] is System.DBNull)? 0: (long)line["productId"];//Add Product Storage
            var parentId = (line["parentId"] is System.DBNull) ? 0 : (long)line["parentId"];
            var isComplexTransaction = (long) line["isComplexTransaction"] > 0;

            var transaction = (Transaction)transactionFactory.CreateTransaction(account,category,name,total,timestamp, id,weight,null,null);

            transaction.Id = id;
            transaction.Timestamp = timestamp;

            return transaction;
        }

        public static void UpdateParents(IDictionary<string, object> line, ITransaction[] withNoParents)
        {
            var id = (long)line["id"];
            var parentId = line["parentId"];
            if (parentId is System.DBNull) return;
            var transaction = withNoParents.FirstOrDefault(x => x.Id == id);
            var parentTransaction = (Transaction)withNoParents.FirstOrDefault(x => x.Id == (long)parentId);
            if (transaction != null)
            {
                transaction.ParentTransaction = parentTransaction;
                parentTransaction?.AddChildrenTransaction(transaction);
            }
               
        }
    }
}
