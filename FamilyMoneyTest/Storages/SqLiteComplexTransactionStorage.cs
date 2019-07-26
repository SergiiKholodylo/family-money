using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Storages
{
    [TestClass]
    public class MemoryComplexTransactionStorageTest
    {
        [TestMethod]
        public void CreateComplexTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new MemoryAccountStorage(accountFactory);
            var categoryStorage = new MemoryCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);
            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage,categoryStorage);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage);

            var newTransaction = storage.CreateTransaction(transaction);

            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction1));


            var complexTransaction = storage.GetAllTransactions().FirstOrDefault(x=>x.IsComplexTransaction);

            Assert.AreEqual(transaction.Name, complexTransaction?.Name);
            Assert.AreEqual(transaction.Category.Id, complexTransaction?.Category?.Id);
            Assert.AreEqual(transaction.Account.Id, complexTransaction?.Account?.Id);
            Assert.AreEqual(transaction.Total, complexTransaction?.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();

            var accountStorage = new MemoryAccountStorage(accountFactory);
            var categoryStorage = new MemoryCategoryStorage(categoryFactory);
            var storage = new SqLiteTransactionStorage(
                new RegularTransactionFactory(), 
                accountStorage, 
                categoryStorage);

            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage);

            var newTransaction = storage.CreateTransaction(transaction);

            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction1));

            var allTransactions = storage.GetAllTransactions();

            Assert.AreEqual(3, allTransactions.Count());
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new MemoryAccountStorage(accountFactory);
            var categoryStorage = new MemoryCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);

            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage);

            var newTransaction = storage.CreateTransaction(transaction);

            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction1));




            storage.DeleteTransaction(newTransaction);


            var numberOfTransactions = storage.GetAllTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }


        [TestMethod]
        public void UpdateTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new MemoryAccountStorage(accountFactory);
            var categoryStorage = new MemoryCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);

            storage.DeleteAllData();
            var transaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction = CreateTransaction(accountStorage, categoryStorage);
            var childTransaction1 = CreateTransaction(accountStorage, categoryStorage);

            var newTransaction = storage.CreateTransaction(transaction);

            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction));
            storage.AddChildrenTransaction(newTransaction, storage.CreateTransaction(childTransaction1));

            childTransaction1.Name = "New Name";
            childTransaction1.Total = 515.03m;


            storage.UpdateTransaction(childTransaction1);


            var firstTransaction = storage.GetAllTransactions().First(x=>x.Id == childTransaction1.Id);
            Assert.AreEqual(childTransaction1.Name, firstTransaction.Name);
            Assert.AreEqual(childTransaction1.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(childTransaction1.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(childTransaction1.Total, firstTransaction.Total);
        }

        private ITransaction CreateTransaction(IAccountStorage accountStorage, ICategoryStorage categoryStorage)
        {
            var factory = new RegularTransactionFactory();
            
            var transactionName = "Test Transaction";
            var transactionTotal = 213.00m;


            var account = accountStorage.CreateAccount("Test account", "Account Description", "EUR");
            var category = categoryStorage.CreateCategory("Sample category", "Category Description", 0, null);

            var transaction = factory.CreateTransaction(account, category, transactionName,transactionTotal,DateTime.Now,0,0.12m,null,null);

            return transaction;
        }
    }
}
