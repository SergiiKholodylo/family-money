using System;
using System.Collections.Generic;
using System.Text;
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
            _category = _storages.CategoryStorage.CreateCategory("Main Category", "Category Description", 0, null);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages,null,null,null);


            Assert.IsNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);
            Assert.AreEqual(DateTime.Today,viewModel.Timestamp.Date);
        }


        [TestMethod]
        public void CreateChildTransactionNewTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, null, null, null);
            viewModel.CreateChildTransaction();

            Assert.IsNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);

        }

        [TestMethod]
        [ExpectedException(typeof(ViewModelException))]
        public void UpdateChildTransactionNewTest()
        {
            var viewModel = new EditChildTransactionViewModel(_storages, null, null, null);
            viewModel.UpdateChildTransaction();

            Assert.IsNull(viewModel.Account);
            Assert.IsNull(viewModel.Category);
            Assert.IsNotNull(viewModel.Accounts);
            Assert.IsNotNull(viewModel.Categories);

        }

    }
}
