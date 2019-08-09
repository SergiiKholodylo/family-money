using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Factories
{
    [TestClass]
    public class RegularQuickTransactionTest
    {
        [TestMethod]
        public void CreateQuickTransactionTest()
        {
            var factory = new RegularQuickTransactionFactory();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();

            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);

            var transaction = factory.CreateQuickTransaction(
                account, category, "Simple Transaction", 100,5,0,false,false
            );

            Assert.AreEqual(account, transaction.Account);
            Assert.AreEqual(category, transaction.Category);
            Assert.AreEqual("Simple Transaction", transaction.Name);
            Assert.AreEqual(100m, transaction.Total);
            Assert.AreEqual(5, transaction.Id);
            Assert.AreEqual(0, transaction.Weight);
            Assert.AreEqual(false, transaction.AskForTotal);
            Assert.AreEqual(false, transaction.AskForWeight);

        }
    }
}
