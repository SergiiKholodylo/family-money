using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.FilteredSources
{
    [TestClass]
    public class AccountTransactionFilteredSourceTest
    {
        private MemoryAccountStorage _accountStorage;
        private MemoryCategoryStorage _categoryStorage;
        private MemoryTransactionStorage _transactionStorage;
        private IAccount _account1;
        private IAccount _account2;

        [TestMethod]
        public void GetTransactionsTest()
        {
            var filteredSource = new AccountTransactionFilteredSource(_account1);

            var filteredTransactions = filteredSource.GetTransactions(_transactionStorage);


            Assert.AreEqual(6, filteredTransactions.Count());
            foreach (var transaction in filteredTransactions)
            {
                Assert.IsTrue(_account1.Equals(transaction.Account));
                Assert.IsTrue(!_account2.Equals(transaction.Account));
            }

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

            var category1 = _categoryStorage.CreateCategory("Category 1", "category Description", 0, null);
            var category2 = _categoryStorage.CreateCategory("Category 2", "category Description", 0, category1);
            var category3 = _categoryStorage.CreateCategory("Category 3", "category Description", 0, null);
            var category4 = _categoryStorage.CreateCategory("Category 4", "category Description", 0, category1);
            var category5 = _categoryStorage.CreateCategory("Category 5", "category Description", 0, null);

            _transactionStorage.CreateTransaction(_account1, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, category4, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, category3, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, category5, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account2, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, category2, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);
            _transactionStorage.CreateTransaction(_account1, category1, "Simple Transaction", 100, DateTime.Now, 0, 0, null, null);

        }
    }
}
