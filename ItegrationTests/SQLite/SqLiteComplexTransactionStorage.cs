﻿using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;
using FamilyMoneyLib.NetStandard.Storages.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.SQLite
{
    [TestClass]
    public class SqLiteComplexTransactionStorageTest
    {
        [TestMethod]
        public void CreateComplexTransactionTest()
        {
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);

            var newTransaction = storage.CreateTransaction(transaction);

            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));


            var complexTransaction = storage.GetAllTransactions().FirstOrDefault(x=>x.IsComplexTransaction);

            Assert.AreEqual(transaction.Name, complexTransaction?.Name);
            Assert.AreEqual(transaction.Category.Id, complexTransaction?.Category?.Id);
            Assert.AreEqual(transaction.Account.Id, complexTransaction?.Account?.Id);
            Assert.AreEqual(426.00m, complexTransaction?.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            ITransactionFactory transactionFactory = new RegularTransactionFactory();
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);


            var newTransaction = storage.CreateTransaction(transaction);


            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));
            var allTransactions = storage.GetAllTransactions();
            Assert.AreEqual(3, allTransactions.Count());
        }

        [TestMethod]
        public void DeleteComplexTransactionTest()
        {
            ITransactionFactory transactionFactory = new RegularTransactionFactory();
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var storage = new SqLiteTransactionStorage(transactionFactory,accountStorage,categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var newTransaction = storage.CreateTransaction(transaction);
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));




            storage.DeleteTransaction(newTransaction);


            var numberOfTransactions = storage.GetAllTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }

        [TestMethod]
        public void DeleteChildTransactionTest()
        {
            ITransactionFactory transactionFactory = new RegularTransactionFactory();
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var newTransaction = storage.CreateTransaction(transaction);
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));



            //storage.DeleteTransaction(childTransaction);
            storage.DeleteChildTransaction(transaction, childTransaction);


            var numberOfTransactions = storage.GetAllTransactions().Count();
            var numberOfComplex = storage.GetAllTransactions().Count(x=>x.IsComplexTransaction);
            var numberOfNoComplex = storage.GetAllTransactions().Count(x => !x.IsComplexTransaction);

            Assert.AreEqual(2, numberOfTransactions);
            Assert.AreEqual(1, numberOfComplex);
            Assert.AreEqual(1, numberOfNoComplex);
        }

        [TestMethod]
        public void DeleteLastChildTransactionTest()
        {
            ITransactionFactory transactionFactory = new RegularTransactionFactory();
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var newTransaction = storage.CreateTransaction(transaction);
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));


            storage.DeleteTransaction(childTransaction);


            var numberOfTransactions = storage.GetAllTransactions().Count();
            var numberOfComplex = storage.GetAllTransactions().Count(x => x.IsComplexTransaction);
            var numberOfNoComplex = storage.GetAllTransactions().Count(x => !x.IsComplexTransaction);

            Assert.AreEqual(1, numberOfTransactions);
            Assert.AreEqual(0, numberOfComplex);
            Assert.AreEqual(1, numberOfNoComplex);
        }

        [TestMethod]
        public void UpdateTransactionTest()
        {
            ITransactionFactory transactionFactory = new RegularTransactionFactory();
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            var newTransaction = storage.CreateTransaction(transaction);
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));
            childTransaction1.Name = "New Name";
            childTransaction1.Total = 515.03m;


            storage.UpdateTransaction(childTransaction1);


            var firstTransaction = storage.GetAllTransactions().First(x=>x.Id == childTransaction1.Id);
            Assert.AreEqual(childTransaction1.Name, firstTransaction.Name);
            Assert.AreEqual(childTransaction1.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(childTransaction1.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(childTransaction1.Total, firstTransaction.Total);
        }

        private ITransaction CreateTransaction(IAccountStorage accountManager, ICategoryStorage categoryManager, ITransactionFactory factory)
        {
        
            var transactionName = "Test Transaction";
            var transactionTotal = 213.00m;


            var account = accountManager.CreateAccount("Test account", "Account Description", "EUR");
            var category = categoryManager.CreateCategory("Sample category", "Category Description", 0, null);

            var transaction = factory.CreateTransaction(account, category, transactionName,transactionTotal,DateTime.Now,0,0.12m,null,null);

            return transaction;
        }
    }
}
