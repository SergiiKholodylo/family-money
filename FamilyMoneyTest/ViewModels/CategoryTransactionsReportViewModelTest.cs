using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using FamilyMoneyLib.NetStandard.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]

    public class CategoryTransactionsReportViewModelTest
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
        public void CreateViewModel_DefaultParameters()
        {
            var viewModel = new CategoryTransactionsReportViewModel(
                _storages.AccountStorage,
                _storages.CategoryStorage,
                _storages.TransactionStorage);


            Assert.IsNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsFalse(viewModel.IncludeSubCategories);
            Assert.AreEqual(DateTime.Today.Date,viewModel.StartDate.Date);
            Assert.AreEqual(DateTime.Today.Date, viewModel.EndDate.Date);
        }
    }
}
