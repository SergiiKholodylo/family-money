using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class TransactionViewModelBaseTest
    {
        private FamilyMoneyLib.NetStandard.Storages.Storages _storages;
        private IAccount _account;
        private ICategory _category;
        private ITransaction _transaction;
        private IAccount _additionalAccount;

        [TestInitialize]
        public void Setup()
        {
            var transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());
            _storages = new FamilyMoneyLib.NetStandard.Storages.Storages
            {
                AccountStorage = new MemoryAccountStorage(new RegularAccountFactory()),
                CategoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory()),
                TransactionStorage = transactionStorage,
                BarCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(), transactionStorage)
            };

            _account = _storages.AccountStorage.CreateAccount("Main Account", "Description", "UAH");
            _additionalAccount = _storages.AccountStorage.CreateAccount("Main Account", "Description", "UAH");
            _category = _storages.CategoryStorage.CreateCategory("Main Category", "Description", 0, null);
            _transaction = _storages.TransactionStorage.CreateTransaction(_account, _category,
                "Test", 22m, DateTime.Now, 0, 0.451m, null, null);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var viewModel = new TransactionViewModelBase(_storages);


            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.Transactions);
            Assert.IsNotNull(viewModel.ChildrenTransactions);
            Assert.IsNotNull(viewModel.Timestamp);
            Assert.IsNotNull(viewModel.Time);
            Assert.IsNotNull(viewModel.Date);
            Assert.IsNull(viewModel.Transaction);
        }

        [TestMethod]
        public void CreateTransactionTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Category = _category,
                Account = _additionalAccount,
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m
            };

            viewModel.CreateTransaction();
            var newTransaction = 
                _storages.TransactionStorage.GetAllTransactions().
                    FirstOrDefault(x => x.Account.Equals( _additionalAccount) );


            Assert.IsNotNull(newTransaction);
            Assert.AreEqual(67.99m, newTransaction.Total);
            Assert.AreEqual(1.09m, newTransaction.Weight);
            Assert.AreEqual("Test transaction",newTransaction.Name);
            Assert.AreEqual(_category, newTransaction.Category);
        }


        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void CreateTransactionExceptionTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m
            };

            viewModel.CreateTransaction();
            var newTransaction =
                _storages.TransactionStorage.GetAllTransactions().
                    FirstOrDefault(x => x.Account.Equals(_additionalAccount));


            Assert.IsTrue(false);
        }


        [TestMethod]
        public void CreateBarCodeWithTransactionWithoutCodeTest()
        {
            var viewModel = new TransactionViewModelBase(_storages);

            viewModel.CreateBarCodeWithTransaction();


            Assert.IsNull(viewModel.BarCode);
            Assert.AreEqual(0, _storages.BarCodeStorage.GetAllBarCodes().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void CreateBarCodeWithTransactionWithCodeExceptionTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Category = _category,
                Account = _additionalAccount,
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m,
                BarCode = new BarCode("777777777777")

            };


            viewModel.CreateBarCodeWithTransaction();


            Assert.IsTrue(false);

        }

        [TestMethod]

        public void CreateBarCodeWithTransactionWithCodeTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Category = _category,
                Account = _additionalAccount,
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m,
                BarCode = new BarCode("777777777777")

            };

            viewModel.CreateTransaction();
            var barCode = _storages.BarCodeStorage.GetAllBarCodes().FirstOrDefault();

            Assert.IsNotNull(barCode);
            Assert.IsNotNull(barCode.Transaction);
            Assert.AreEqual(1, _storages.BarCodeStorage.GetAllBarCodes().Count());


        }


        [TestMethod]
        public void UpdateTransactionTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Category = _category,
                Account = _additionalAccount,
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m
            };

            viewModel.CreateTransaction();

            viewModel.Total = 88.99m;
            viewModel.Weight = 0.99m;
            viewModel.Name = "New";

            viewModel.UpdateTransaction();

            var newTransaction =
                _storages.TransactionStorage.GetAllTransactions().
                    FirstOrDefault(x => x.Account.Equals(_additionalAccount));


            Assert.IsNotNull(newTransaction);
            Assert.AreEqual(88.99m, newTransaction.Total);
            Assert.AreEqual(0.99m, newTransaction.Weight);
            Assert.AreEqual("New", newTransaction.Name);
            Assert.AreEqual(_category, newTransaction.Category);
        }


        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void UpdateTransactionExceptionTest()
        {
            var viewModel = new TransactionViewModelBase(_storages)
            {
                Name = "Test transaction",
                Total = 67.99m,
                Weight = 1.09m
            };

            viewModel.UpdateTransaction();
            var newTransaction =
                _storages.TransactionStorage.GetAllTransactions().
                    FirstOrDefault(x => x.Account.Equals(_additionalAccount));


            Assert.IsTrue(false);
        }
    }
}
