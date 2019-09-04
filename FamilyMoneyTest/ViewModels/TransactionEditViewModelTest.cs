using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class TransactionEditViewModelTest
    {
        private FamilyMoneyLib.NetStandard.Storages.Storages _storages;
        private IAccount _account;
        private ICategory _category;
        private ITransaction _transaction;
        private IAccount _additionalAccount;

        [TestInitialize]
        public void Setup()
        {
            _storages = new FamilyMoneyLib.NetStandard.Storages.Storages
            {
                AccountStorage = new MemoryAccountStorage(new RegularAccountFactory()),
                CategoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory()),
                TransactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory())
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
            
            var viewModel = new TransactionEditViewModel(_storages, null);


            Assert.IsNotNull(viewModel.ChildrenTransactions);
            Assert.AreEqual(0,viewModel.Total);
            Assert.AreEqual(0, viewModel.Weight);
            Assert.IsFalse(viewModel.IsComplexTransaction);
            Assert.IsFalse(viewModel.IsExistingTransaction);
            Assert.IsNull(viewModel.Transaction);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Transactions);
        }

        [TestMethod]
        public void ConstructorWithTransactionTest()
        {

            var viewModel = new TransactionEditViewModel(_storages, _transaction);


            Assert.IsNotNull(viewModel.ChildrenTransactions);
            Assert.AreEqual(22m, viewModel.Total);
            Assert.AreEqual(0.451m, viewModel.Weight);
            Assert.IsFalse(viewModel.IsComplexTransaction);
            Assert.IsTrue(viewModel.IsExistingTransaction);
            Assert.IsNotNull(viewModel.Transaction);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Transactions);
            Assert.AreEqual(_category, viewModel.Category);
            Assert.AreEqual(_account, viewModel.Account);
            Assert.AreEqual(_transaction.Name, viewModel.Name);
        }

    }
}
