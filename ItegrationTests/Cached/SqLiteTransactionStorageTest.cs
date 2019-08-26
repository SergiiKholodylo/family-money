using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.CachedStorage;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Cached
{
    [TestClass]
    public class SqLiteTransactionStorageTest
    {
        private IAccountStorage _accountStorage;
        private ICategoryStorage _categoryStorage;
        private ITransactionStorage _storage;
        private ITransaction _transaction;

        [TestInitialize]
        public void Setup()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            _accountStorage = new CachedAccountStorage(new SqLiteAccountStorage(accountFactory));
            _categoryStorage = new CachedCategoryStorage(new SqLiteCategoryStorage(categoryFactory));
            var transactionFactory = new RegularTransactionFactory();
            _storage = new CachedTransactionStorage(new SqLiteTransactionStorage(transactionFactory, _accountStorage, _categoryStorage));
            _storage.DeleteAllData();
            CreateTransaction();

        }

        [TestMethod]
        public void CreateTransactionTest()
        {


            var newTransaction = _storage.CreateTransaction(_transaction);


            Assert.AreEqual(_transaction.Name, newTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, newTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, newTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, newTransaction.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            _storage.CreateTransaction(_transaction);

            var firstTransaction = _storage.GetAllTransactions().First();

            Assert.AreEqual(_transaction.Name, firstTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, firstTransaction.Total);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            //_storage.CreateTransaction(_transaction);


            
            _storage.DeleteTransaction(_transaction);


            var numberOfTransactions = _storage.GetAllTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }


        [TestMethod]
        public void UpdateTransactionTest()
        {
            //_storage.CreateTransaction(_transaction);

            _transaction.Name = "New Name";
            _transaction.Total = 515.03m;


            _storage.UpdateTransaction(_transaction);


            var firstTransaction = _storage.GetAllTransactions().First();
            Assert.AreEqual(_transaction.Name, firstTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, firstTransaction.Total);
        }

        private void CreateTransaction()
        {
            var transactionName = "Test Transaction";
            var transactionTotal = 213.00m;


            var account = _accountStorage.CreateAccount("Test account", "Account Description", "EUR");
            var category = _categoryStorage.CreateCategory("Sample category", "Category Description", 0, null);

            _transaction = _storage.CreateTransaction(account, category, transactionName,transactionTotal,DateTime.Now,0,0.12m,null,null);

 }
    }
}
