using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Report1ViewModelTest
    {
        private MemoryAccountStorage accountStorage;
        private MemoryCategoryStorage categoryStorage;
        private MemoryTransactionStorage transactionStorage;


        [TestInitialize]
        public void Setup()
        {
             accountStorage = new MemoryAccountStorage(new RegularAccountFactory());
             categoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory());
             transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());

             transactionStorage.DeleteAllData();

             var account1 = accountStorage.CreateAccount("Main Account", "Description", "UAH");
             var account2 = accountStorage.CreateAccount("Reserve Account", "Description", "UAH");

             var category1 = categoryStorage.CreateCategory("Category 1", "category Description", 0, null);
             var category2 = categoryStorage.CreateCategory("Category 2", "category Description", 0, category1);
             var category3 = categoryStorage.CreateCategory("Category 3", "category Description", 0, null);
             var category4 = categoryStorage.CreateCategory("Category 4", "category Description", 0, category1);
             var category5 = categoryStorage.CreateCategory("Category 5", "category Description", 0, null);

             transactionStorage.CreateTransaction(account1, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account1, category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account1, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account1, category4, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account2, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account2, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account2, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account2, category5, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account2, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account1, category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
             transactionStorage.CreateTransaction(account1, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);

        }

        [TestMethod]
        public void TestConstructor()
        {
            var viewModel = new Report1ViewModel(accountStorage,categoryStorage, transactionStorage);

            var counts = viewModel.Accounts.Count;
            viewModel.Account = viewModel.Accounts.FirstOrDefault();
            viewModel.Execute();

            Assert.AreEqual(2,counts);
        }

            
    }
    
}
