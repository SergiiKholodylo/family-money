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
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage, accountFactory, categoryFactory);
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
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage, accountFactory, categoryFactory);
            storage.DeleteAllData();
            var transaction = CreateTransaction();
            storage.CreateTransaction(transaction);

            var firstTransaction = storage.GetAllTransactions().First();

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
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage, accountFactory, categoryFactory);

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
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionFactory = new RegularTransactionFactory();
            var storage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage, accountFactory, categoryFactory);

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
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountManager = new AccountManager(accountFactory, new SqLiteAccountStorage(accountFactory));
            var categoryManager = new CategoryManager(categoryFactory, new SqLiteCategoryStorage(categoryFactory));

            var factory = new RegularTransactionFactory();
            
            var transactionName = "Test Transaction";
            var transactionTotal = 213.00m;


            var account = accountManager.CreateAccount("Test account", "Account Description", "EUR");
            var category = categoryManager.CreateCategory("Sample category", "Category Description", 0, null);

            var transaction = factory.CreateTransaction(account, category, transactionName,transactionTotal);

            return transaction;
        }
    }
}
