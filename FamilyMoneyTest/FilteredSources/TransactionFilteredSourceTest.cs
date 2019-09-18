using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilteredSources
{
    [TestClass]
    public class TransactionFilteredSourceTest
    {
        private MemoryAccountStorage _accountStorage;
        private MemoryCategoryStorage _categoryStorage;
        private MemoryTransactionStorage _transactionStorage;
        private IAccount _account1;
        private IAccount _account2;
        private ICategory _category1;
        private ICategory _category2;
        private ICategory _category3;
        private ICategory _category4;
        private ICategory _category5;

        [TestMethod]
        public void TestWithoutFilter()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.MinValue, DateTime.Now);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(11, filteredTransactions.Count());
        }

        [TestMethod]
        public void TestTodayFilter()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.Now.Date, DateTime.Now);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(6, filteredTransactions.Count());
        }

        [TestMethod]
        public void TestYesterdayFilter()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.Now.AddDays(-1).Date, DateTime.Now.Date);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(5, filteredTransactions.Count());
        }

        [TestMethod]
        public void TestWithAccountFilter()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.MinValue, DateTime.Now,_account2);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(5, filteredTransactions.Count());
        }

        [TestMethod]
        public void TestWithCategoryFilter()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.MinValue, DateTime.Now, null, _category5);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(1, filteredTransactions.Count());
        }

        [TestMethod]
        public void TestWithCategoryFilterWithSubcategories()
        {
            var filteredSource = new TransactionFilteredSource(DateTime.MinValue, DateTime.Now, null, _category1, true);


            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage).ToArray();


            Assert.AreEqual(8, filteredTransactions.Count());
        }


        [TestInitialize]
        public void Setup()
        {
            _accountStorage = new MemoryAccountStorage(new RegularAccountFactory());
            _categoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory());
            _transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());

            _transactionStorage.DeleteAllData();

            _account1 = _accountStorage.CreateAccount("Main Account", "Description", "UAH");
            _account2 = _accountStorage.CreateAccount("Reserve Account", "Description", "UAH");

            _category1 = _categoryStorage.CreateCategory("Category 1", "category Description", 0, null);
            _category2 = _categoryStorage.CreateCategory("Category 2", "category Description", 0, _category1);
            _category3 = _categoryStorage.CreateCategory("Category 3", "category Description", 0, null);
            _category4 = _categoryStorage.CreateCategory("Category 4", "category Description", 0, _category1);
            _category5 = _categoryStorage.CreateCategory("Category 5", "category Description", 0, _category4);

            _transactionStorage.CreateTransaction(_account1, _category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, _category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, _category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, _category4, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, _category1, "Simple Transaction", 100, DateTime.Now.AddDays(-1), 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, _category3, "Simple Transaction", 100, DateTime.Now.AddDays(-1), 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, _category3, "Simple Transaction", 100, DateTime.Now.AddDays(-1), 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, _category5, "Simple Transaction", 100, DateTime.Now.AddDays(-1), 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, _category1, "Simple Transaction", 100, DateTime.Now.AddDays(-1), 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, _category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, _category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);

        }
    }
}
