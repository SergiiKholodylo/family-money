using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class TransactionsViewModelTest
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
            var viewModel = new TransactionsViewModel(_storages);

            Assert.AreEqual(_account, viewModel.ActiveAccount);
            Assert.IsNotNull(viewModel.Transactions);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.AreEqual(1,viewModel.Transactions.Count);
        }

        [TestMethod]
        public void ActiveAccountChange()
        {
            var viewModel = new TransactionsViewModel(_storages);

            viewModel.ActiveAccount = _additionalAccount;

            Assert.AreEqual(_additionalAccount, viewModel.ActiveAccount);
            Assert.IsNotNull(viewModel.Transactions);
            Assert.IsNotNull(viewModel.ActiveAccount);
            Assert.AreEqual(0, viewModel.Transactions.Count);
        }

    }
}
