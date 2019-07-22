﻿using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Storages
{
    [TestClass]
    public class MemoryTransactionStorageTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var storage = new MemoryTransactionStorage();
            var transaction = CreateTransaction();


            var newTransaction = storage.CreateTransaction(transaction);


            Assert.AreEqual(transaction.Name, newTransaction.Name);
            Assert.AreEqual(transaction.Total, newTransaction.Total);
            Assert.AreEqual(transaction.Account, newTransaction.Account);
            Assert.AreEqual(transaction.Category, newTransaction.Category);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var storage = new MemoryTransactionStorage();
            var factory = new RegularTransactionFactory();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);


            var firstTransaction = storage.GetAllTransactions(factory).First();
            

            Assert.AreEqual(transaction.Name, firstTransaction.Name);
            Assert.AreEqual(transaction.Total, firstTransaction.Total);
            Assert.AreEqual(transaction.Account, firstTransaction.Account);
            Assert.AreEqual(transaction.Category, firstTransaction.Category);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var storage = new MemoryTransactionStorage();
            var factory = new RegularTransactionFactory();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);

            var numberOfTransactionsAfterCreate = storage.GetAllTransactions(factory).Count();
            storage.DeleteTransaction(transaction);


            var numberOfTransactions = storage.GetAllTransactions(factory).Count();


            Assert.AreEqual(0, numberOfTransactions);
            Assert.AreEqual(1, numberOfTransactionsAfterCreate);
        }

        [TestMethod]
        public void UpdateTransactionTest()
        {
            var storage = new MemoryTransactionStorage();
            var factory = new RegularTransactionFactory();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);
            transaction.Total = 120;


            storage.UpdateTransaction(transaction);
            var storedTransaction = storage.GetAllTransactions(factory).First();


            Assert.AreEqual(transaction.Total, storedTransaction.Total);

        }

        private ITransaction CreateTransaction()
        {
            var factory = new RegularTransactionFactory();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();

            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description");

            var transaction = factory.CreateTransaction(
                account, category, "Simple Transaction", 100
            );
            return transaction;
        }
    }
}
