using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Factories
{
    [TestClass]
    public class RegularTransactionFactoryTest
    {
        [TestMethod]
        public void CreateTransactionTest()
        {
            var factory = new RegularTransactionFactory();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();

            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description");

            var transaction = factory.CreateTransaction(
                account, category,"Simple Transaction", 100
            );

            Assert.AreEqual(account, transaction.Account);
            Assert.AreEqual(category, transaction.Category);
        }
    }
}
