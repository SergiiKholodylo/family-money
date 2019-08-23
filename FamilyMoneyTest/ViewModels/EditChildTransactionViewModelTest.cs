using System;
using System.Linq;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class EditChildTransactionViewModelTest
    {
        private FamilyMoneyLib.NetStandard.Storages.Storages _storages;
        private IAccount _account;
        private ICategory _category;
        private ITransaction _transaction;
        private ITransaction _childTransaction;
        private ICategory _childCategory;
        private IAccount _fakeAccount;

        [TestInitialize]
        public void Setup()
        {

            _storages = new FamilyMoneyLib.NetStandard.Storages.Storages
            {
                AccountStorage = new MemoryAccountStorage(new RegularAccountFactory()),
                CategoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory()),
                TransactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory())
            };
            _account = _storages.AccountStorage.CreateAccount("Account", "Description", "UAH");
            _fakeAccount = _storages.AccountStorage.CreateAccount("Fake Account", "Description", "UAH");
            _category = _storages.CategoryStorage.CreateCategory("Main Category", "Category Description", 0, null);
            _childCategory = _storages.CategoryStorage.CreateCategory("Main Category", "Category Description", 0,_category);
            _transaction = _storages.TransactionStorage.CreateTransaction(_account, _category, "Test Transaction", 23m,
                DateTime.Now, 0, 0, null, null);

            _childTransaction = _storages.TransactionStorage.CreateTransaction(_fakeAccount, _childCategory, "Test Child Transaction", 23m,
                DateTime.Now, 0, 0, null, null);

        }

        [TestMethod]
        public void ConstructorTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages,_transaction,null);


            Assert.IsNotNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);
            Assert.AreEqual(DateTime.Today,viewModel.Timestamp.Date);
            Assert.AreEqual(_transaction, viewModel.ParentTransaction);
        }

        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void ConstructorWithNoParentTransactionTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, null,  null);


            Assert.IsNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);
            Assert.AreEqual(DateTime.Today, viewModel.Timestamp.Date);
        }


        [TestMethod]
        public void CreateChildTransactionNewTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, _transaction, null);
            viewModel.CreateChildTransaction();

            Assert.AreEqual(_transaction.Account,viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);

        }

        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void UpdateChildTransactionNewTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, _transaction,  null);
            viewModel.UpdateChildTransaction();

            Assert.IsNotNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);

        }

        [TestMethod]
        public void InitialValuesFillingTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, _transaction,  _childTransaction);
            
            Assert.AreEqual(_transaction.Account, viewModel.Account);
            Assert.AreEqual(_transaction.Timestamp, viewModel.Timestamp);
            Assert.AreEqual(String.Empty, viewModel.ErrorString);
        }

        [TestMethod]
        public void CreateChildTransactionTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, _transaction, null)
            {
                Category = _childCategory,
                Name = "Child Transaction",
                Total = 22.17m,
                Weight = 0.917m
            };
            viewModel.CreateChildTransaction();

            var savedTransaction = _storages.TransactionStorage.GetAllTransactions().FirstOrDefault(x=>x.Name.Equals("Child Transaction"));

            Assert.IsNotNull(savedTransaction);
            Assert.AreEqual(_transaction.Account, savedTransaction.Account);
            Assert.AreEqual(0.917m, savedTransaction.Weight);
            Assert.AreEqual(22.17m, savedTransaction.Total);
            Assert.AreEqual(_childCategory, savedTransaction.Category);
        }

        [TestMethod]
        public void UpdateChildTransactionTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, _transaction, _childTransaction)
            {
                Category = _childCategory,
                Name = "Child Transaction",
                Total = 22.17m,
                Weight = 0.917m
            };
            viewModel.UpdateChildTransaction();

            var savedTransaction = _storages.TransactionStorage.GetAllTransactions().FirstOrDefault(x => x.Id == _childTransaction.Id);

            Assert.IsNotNull(savedTransaction);
            Assert.AreEqual(_transaction.Account, savedTransaction.Account);
            Assert.AreEqual(0.917m, savedTransaction.Weight);
            Assert.AreEqual(22.17m, savedTransaction.Total);
            Assert.AreEqual(_childCategory, savedTransaction.Category);
        }
    }
}
