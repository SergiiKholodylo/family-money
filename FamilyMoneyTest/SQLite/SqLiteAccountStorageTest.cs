using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.SQLite
{
    [TestClass]
    public class SqLiteTransactionStorageTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var accountStorage = new SqLiteAccountStorage();
            var categoryStorage = new SqLiteCategoryStorage();
            var storage = new SqLiteTransactionStorage(accountStorage, categoryStorage);
            var transaction = CreateTransaction();


            var newTransaction = storage.CreateTransaction(transaction);


            Assert.AreEqual(transaction.Name, newTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, newTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, newTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, newTransaction.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var accountStorage = new SqLiteAccountStorage();
            var categoryStorage = new SqLiteCategoryStorage();
            var storage = new SqLiteTransactionStorage(accountStorage, categoryStorage);
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);

            var firstTransaction = storage.GetAllTransactions().Last();

            Assert.AreEqual(transaction.Name, firstTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, firstTransaction.Total);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var accountStorage = new SqLiteAccountStorage();
            var categoryStorage = new SqLiteCategoryStorage();
            var storage = new SqLiteTransactionStorage(accountStorage, categoryStorage);
            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);


            
            storage.DeleteTransaction(transaction);


            var numberOfTransactions = storage.GetAllTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }


        [TestMethod]
        public void UpdateTransactionTest()
        {
            var accountStorage = new SqLiteAccountStorage();
            var categoryStorage = new SqLiteCategoryStorage();
            var storage = new SqLiteTransactionStorage(accountStorage, categoryStorage);
            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);

            transaction.Name = "New Name";
            transaction.Total = 515.03m;


            storage.UpdateTransaction(transaction);


            var firstTransaction = storage.GetAllTransactions().First();
            Assert.AreEqual(transaction.Name, firstTransaction.Name);
            Assert.AreEqual(transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(transaction.Total, firstTransaction.Total);
        }

        private ITransaction CreateTransaction()
        {
            var accountManager = new AccountManager(new RegularAccountFactory(), new SqLiteAccountStorage());
            var categoryManager = new CategoryManager(new RegularCategoryFactory(), new SqLiteCategoryStorage());

            var factory = new RegularTransactionFactory();
            
            var transactionName = "Test Transaction";
            var transactionTotal = 213.00m;


            var account = accountManager.CreateAccount("Test account", "Account Description", "EUR");
            var category = categoryManager.CreateCategory("Sample category", "Category Description");

            var transaction = factory.CreateTransaction(account, category, transactionName,transactionTotal);

            return transaction;
        }
    }
}
