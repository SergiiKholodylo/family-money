using System.Linq;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Managers
{
    [TestClass]
    public class TransactionManagerTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var factory = new RegularTransactionFactory();
            var storage = new MemoryTransactionStorage(factory);
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);
            var manager = new TransactionManager(factory, storage);


            var storedTransaction = manager.CreateTransaction(account, category, "Simple Transaction", 100);


            Assert.AreEqual(account, storedTransaction.Account);
            Assert.AreEqual(category, storedTransaction.Category);
            Assert.AreEqual(100,storedTransaction.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var factory = new RegularTransactionFactory();
            var storage = new MemoryTransactionStorage(factory);
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);
            var manager = new TransactionManager(factory, storage);
            manager.CreateTransaction(account, category, "Simple Transaction", 100);


            var storedTransaction = manager.GetAllTransactions().First();


            Assert.AreEqual(account, storedTransaction.Account);
            Assert.AreEqual(category, storedTransaction.Category);
            Assert.AreEqual(100, storedTransaction.Total);

        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var factory = new RegularTransactionFactory();
            var storage = new MemoryTransactionStorage(factory);
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);
            var manager = new TransactionManager(factory, storage);
            var storedTransaction = manager.CreateTransaction(account, category, "Simple Transaction", 100);


            manager.DeleteTransaction(storedTransaction);
            var numberOfTransaction = manager.GetAllTransactions().Count();


            Assert.AreEqual(0, numberOfTransaction);

        }

        [TestMethod]
        public void UpdateTransactionTest()
        {
            var newTransactionAmount = 250m;
            var factory = new RegularTransactionFactory();
            var storage = new MemoryTransactionStorage(factory);

            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);
            var manager = new TransactionManager(factory, storage);
            var storedTransaction = manager.CreateTransaction(account, category, "Simple Transaction", 100);
            storedTransaction.Total = newTransactionAmount;


            storage.UpdateTransaction(storedTransaction);
            var firstTransaction = manager.GetAllTransactions().First();


            Assert.AreEqual(newTransactionAmount,firstTransaction.Total);
        }
    }
}
