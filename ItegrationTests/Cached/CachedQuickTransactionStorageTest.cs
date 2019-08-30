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
    public class CachedQuickTransactionStorageTest
    {
        private IAccountFactory _accountFactory;
        private ICategoryFactory _categoryFactory;
        private IAccountStorage _accountStorage;
        private ICategoryStorage _categoryStorage;
        private IQuickTransactionFactory _transactionFactory;
        private IQuickTransactionStorage _storage;
        private IQuickTransaction _transaction;

        [TestInitialize]
        public void Setup()
        {
            _accountFactory = new RegularAccountFactory();
            _categoryFactory = new RegularCategoryFactory();
            _accountStorage = new CachedAccountStorage(new SqLiteAccountStorage(_accountFactory));
            _categoryStorage = new CachedCategoryStorage(new SqLiteCategoryStorage(_categoryFactory));
            _transactionFactory = new RegularQuickTransactionFactory();
            _storage = new CachedQuickTransactionStorage(new SqLiteQuickTransactionStorage(_transactionFactory, _accountStorage, _categoryStorage));
            _storage.DeleteAllData();

            _transaction = CreateTransaction();

        }

        [TestMethod]
        public void CreateTransactionTest()
        {


            var newTransaction = _storage.CreateQuickTransaction(_transaction);


            Assert.AreEqual(_transaction.Name, newTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, newTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, newTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, newTransaction.Total);
        }

        [TestMethod]
        public void GetAllTransactionsTest()
        {
            _storage.CreateQuickTransaction(_transaction);

            var firstTransaction = _storage.GetAllQuickTransactions().First();

            Assert.AreEqual(_transaction.Name, firstTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, firstTransaction.Total);
        }

        [TestMethod]
        public void DeleteTransactionTest()
        {
            
            _storage.DeleteQuickTransaction(_transaction);


            var numberOfTransactions = _storage.GetAllQuickTransactions().Count();


            Assert.AreEqual(0, numberOfTransactions);
        }


        [TestMethod]
        public void UpdateTransactionTest()
        {

            _transaction.Name = "New Name";
            _transaction.Total = 515.03m;


            _storage.UpdateQuickTransaction(_transaction);


            var firstTransaction = _storage.GetAllQuickTransactions().First();
            Assert.AreEqual(_transaction.Name, firstTransaction.Name);
            Assert.AreEqual(_transaction.Category.Id, firstTransaction.Category.Id);
            Assert.AreEqual(_transaction.Account.Id, firstTransaction.Account.Id);
            Assert.AreEqual(_transaction.Total, firstTransaction.Total);
        }

        private IQuickTransaction CreateTransaction()
        {
            var account = _accountStorage.CreateAccount("Test account", "Account Description", "EUR");
            var category = _categoryStorage.CreateCategory("Sample category", "Category Description", 0, null);

            
            var transaction = _storage.CreateQuickTransaction(
                account, category, "Simple Transaction", 100, 5, 0, false, false);

            return transaction;
        }
    }
}
