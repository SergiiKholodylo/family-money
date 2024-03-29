﻿using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.SQLite
{
    [TestClass]
    public class SqLiteQuickTransactionStorageTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularQuickTransactionFactory();
            var storage = new SqLiteQuickTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            var transaction = CreateTransaction();


            var newTransaction = storage.CreateQuickTransaction(transaction);


            Assert.AreEqual(transaction.Name, newTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, newTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, newTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, newTransaction.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularQuickTransactionFactory();
            var storage = new SqLiteQuickTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);

            var firstTransaction = storage.GetAllQuickTransactions().First();

            Assert.AreEqual(transaction.Name, firstTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, firstTransaction.Total);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularQuickTransactionFactory();
            var storage = new SqLiteQuickTransactionStorage(transactionFactory, accountStorage, categoryStorage);

            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);


            
            storage.DeleteQuickTransaction(transaction);


            var numberOfTransactions = storage.GetAllQuickTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }


        [TestMethod]
        public void UpdateTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularQuickTransactionFactory();
            var storage = new SqLiteQuickTransactionStorage(transactionFactory, accountStorage, categoryStorage);

            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);

            transaction.Name = "New Name";
            transaction.Total = 515.03m;


            storage.UpdateQuickTransaction(transaction);


            var firstTransaction = storage.GetAllQuickTransactions().First();
            Assert.AreEqual(transaction.Name, firstTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, firstTransaction.Total);
        }

        private IQuickTransaction CreateTransaction()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountManager =  new SqLiteAccountStorage(accountFactory);
            var categoryManager =  new SqLiteCategoryStorage(categoryFactory);

            var factory = new RegularQuickTransactionFactory();
            

            var account = accountManager.CreateAccount("Test account", "Account Description", "EUR");
            var category = categoryManager.CreateCategory("Sample category", "Category Description", 0, null);

            
            var transaction = factory.CreateQuickTransaction(
                account, category, "Simple Transaction", 100, 5, 0, false, false);

            return transaction;
        }
    }
}
