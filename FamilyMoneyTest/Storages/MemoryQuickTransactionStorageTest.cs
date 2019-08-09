using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Storages
{

    [TestClass]
    public class MemoryQuickTransactionStorageTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var factory = new RegularQuickTransactionFactory();
            var storage = new MemoryQuickTransactionStorage(factory);

            var transaction = CreateTransaction();


            var newTransaction = storage.CreateQuickTransaction(transaction);


            Assert.AreEqual(transaction.Account, newTransaction.Account);
            Assert.AreEqual(transaction.Category, newTransaction.Category);
            Assert.AreEqual("Simple Transaction", newTransaction.Name);
            Assert.AreEqual(100m, newTransaction.Total);
            Assert.AreEqual(5, newTransaction.Id);
            Assert.AreEqual(0, newTransaction.Weight);
            Assert.AreEqual(false, newTransaction.AskForTotal);
            Assert.AreEqual(false, newTransaction.AskForWeight);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            var factory = new RegularQuickTransactionFactory();
            var storage = new MemoryQuickTransactionStorage(factory);
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);


            var newTransaction = storage.GetAllQuickTransactions().First();


            Assert.AreEqual(transaction.Account, newTransaction.Account);
            Assert.AreEqual(transaction.Category, newTransaction.Category);
            Assert.AreEqual("Simple Transaction", newTransaction.Name);
            Assert.AreEqual(100m, newTransaction.Total);
            Assert.AreEqual(5, newTransaction.Id);
            Assert.AreEqual(0, newTransaction.Weight);
            Assert.AreEqual(false, newTransaction.AskForTotal);
            Assert.AreEqual(false, newTransaction.AskForWeight);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            var factory = new RegularQuickTransactionFactory();
            var storage = new MemoryQuickTransactionStorage(factory);
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);

            var numberOfTransactionsAfterCreate = storage.GetAllQuickTransactions().Count();
            storage.DeleteQuickTransaction(transaction);


            var numberOfTransactions = storage.GetAllQuickTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
            Assert.AreEqual(1, numberOfTransactionsAfterCreate);
        }

        [TestMethod]
        public void UpdateTransactionTest()
        {
            var factory = new RegularQuickTransactionFactory();
            var storage = new MemoryQuickTransactionStorage(factory);
            var transaction = CreateTransaction();
            storage.CreateQuickTransaction(transaction);
            transaction.Total = 120;


            storage.UpdateQuickTransaction(transaction);
            var storedTransaction = storage.GetAllQuickTransactions().First();


            Assert.AreEqual(transaction.Total, storedTransaction.Total);

        }

        private IQuickTransaction CreateTransaction()
        {
            var factory = new RegularQuickTransactionFactory();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();

            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);

            var transaction = factory.CreateQuickTransaction(
                account, category, "Simple Transaction", 100, 5, 0, false, false
            );
            return transaction;
        }
    }
}
