using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Reports
{
    [TestClass]
    public class Report1Test
    {
        [TestMethod]
        public void ExecuteTest()
        {
            var accountStorage = new MemoryAccountStorage(new RegularAccountFactory());
            var categoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());

            var category1 = categoryStorage.CreateCategory("Category 1", "", 0, null);
            var category2 = categoryStorage.CreateCategory("Category 2", "", 0, category1);
            var category3 = categoryStorage.CreateCategory("Category 3", "", 0, category2);
            var category31 = categoryStorage.CreateCategory("Category 3-1", "", 0, category3);
            var category32 = categoryStorage.CreateCategory("Category 3-2", "", 0, category3);
            var category4 = categoryStorage.CreateCategory("Category 4", "", 0, category1);
            var category5 = categoryStorage.CreateCategory("Category 5", "", 0, category1);
            var account = accountStorage.CreateAccount("Main account", "", "UAH");
            var account2 = accountStorage.CreateAccount("Reserve account", "", "TUR");


            var transaction1 = CreateTransaction(transactionStorage,account,category31);
            CreateTransaction(transactionStorage, account, category31);
            CreateTransaction(transactionStorage, account, category32);
            CreateTransaction(transactionStorage, account, category31);
            CreateTransaction(transactionStorage, account, category5);
            CreateTransaction(transactionStorage, account, category31);
            CreateTransaction(transactionStorage, account, category32);
            CreateTransaction(transactionStorage, account, category32);


            CreateTransaction(transactionStorage, account2, category32);
            CreateTransaction(transactionStorage, account2, category31);
            CreateTransaction(transactionStorage, account2, category4);


            var report1 = new Report1(transactionStorage,categoryStorage);
            var list = report1.Execute(new AllTransactionFilteredSource()).GroupBy(x => x.Key.Account);
        }


        [TestMethod]
        public void CategoryAccountPairCompareTest()
        {
            var accountStorage = new MemoryAccountStorage(new RegularAccountFactory());
            var categoryStorage = new MemoryCategoryStorage(new RegularCategoryFactory());
            var category1 = categoryStorage.CreateCategory("Category 1", "", 0, null);
            var category2 = categoryStorage.CreateCategory("Category 2", "", 0, category1);
            var account = accountStorage.CreateAccount("Main account", "", "UAH");
            var account2 = accountStorage.CreateAccount("Reserve account", "", "TUR");
            


            var mainPair = new Report1.CategoryAccountPair(account, category1);
            var samePair = new Report1.CategoryAccountPair(account, category1);
            var diffPair = new Report1.CategoryAccountPair(account2, category1);
            var anotherDiffPair = new Report1.CategoryAccountPair(account2, category2);


            Assert.IsTrue(mainPair.Equals(samePair));
            Assert.IsFalse(mainPair.Equals(diffPair));
            Assert.IsFalse(samePair.Equals(diffPair));
            Assert.IsFalse(samePair.Equals(anotherDiffPair));

        }

        private ITransaction CreateTransaction(ITransactionStorage storage, IAccount account, ICategory category)
        { 
            return storage.CreateTransaction(account, category, "Simple Transaction", 100,DateTime.Now,0,0,null,null);
        }
    }
}
