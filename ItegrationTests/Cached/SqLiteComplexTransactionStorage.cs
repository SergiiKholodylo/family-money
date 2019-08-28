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
    public class SqLiteComplexTransactionStorageTest
    {
        private IAccountStorage accountStorage;
        private ICategoryStorage categoryStorage;
        private ITransactionFactory transactionFactory;
        private ITransactionStorage storage;
        private ITransaction transaction;
        private ITransaction childTransaction;
        private ITransaction childTransaction1;

        [TestInitialize]
        public void Setup()
        {
            accountStorage = new CachedAccountStorage(new SqLiteAccountStorage(new RegularAccountFactory()));
            categoryStorage = new CachedCategoryStorage(new SqLiteCategoryStorage(new RegularCategoryFactory()));
            transactionFactory = new RegularTransactionFactory();
            storage = new CachedTransactionStorage(new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage));
            categoryStorage.DeleteAllData();
            accountStorage.DeleteAllData();
            storage.DeleteAllData();
            transaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            childTransaction = CreateTransaction(accountStorage, categoryStorage, transactionFactory);
            childTransaction1 = CreateTransaction(accountStorage, categoryStorage, transactionFactory);

        }


        [TestMethod]
        public void CreateComplexTransactionTest()
        {

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

            var newTransaction = storage.CreateTransaction(transaction);


            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));
            var allTransactions = storage.GetAllTransactions();
            Assert.AreEqual(3, allTransactions.Count());
        }

        [TestMethod]
        public void DeleteComplexTransactionTest()
        {

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
            var newTransaction = storage.CreateTransaction(transaction);
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildTransaction(newTransaction, storage.CreateTransaction(childTransaction1));



            storage.DeleteTransaction(childTransaction);


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

            var result = factory.CreateTransaction(account, category, transactionName,transactionTotal,DateTime.Now,0,0.12m,null,null);

            return result;
        }
    }
}
