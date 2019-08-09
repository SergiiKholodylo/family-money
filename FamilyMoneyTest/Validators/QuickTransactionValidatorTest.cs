using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Validators
{
    [TestClass]
    public class QuickTransactionValidatorTest
    {
        [TestMethod]
        public void ToTransactionIncompleteCategoryTransactionTest()
        {
            var accountFactory = new RegularAccountFactory();
            var quickTransaction = new QuickTransaction
            {
                Account = accountFactory.CreateAccount(
                    "TestAccount","Description","USD"),
                Name = "TestName",
                AskForWeight = false,
                AskForTotal = false,
                Total = 2.34m,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsTrue(result, "Interaction required");
        }

        [TestMethod]
        public void ToTransactionIncompleteAccountTransactionTest()
        {
            var categoryFactory = new RegularCategoryFactory();
            var quickTransaction = new QuickTransaction
            {
                Category = categoryFactory.CreateCategory("TestAccount", "Description",0L,null),
                Name = "TestName",
                AskForWeight = false,
                AskForTotal = false,
                Total = 2.34m,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsTrue(result, "Interaction required");
        }

        [TestMethod]
        public void ToTransactionIncompleteTotalTransactionTest()
        {
            var categoryFactory = new RegularCategoryFactory();
            var accountFactory = new RegularAccountFactory();

            var quickTransaction = new QuickTransaction
            {
                Account = accountFactory.CreateAccount(
                    "TestAccount", "Description", "USD"),
                Category = categoryFactory.CreateCategory("TestAccount", "Description", 0L, null),
                Name = "TestName",
                AskForWeight = false,
                AskForTotal = false,
                Total = 0,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsTrue(result, "Interaction required");
        }

        [TestMethod]
        public void ToTransactionIncompleteAskForTotalTransactionTest()
        {
            var categoryFactory = new RegularCategoryFactory();
            var accountFactory = new RegularAccountFactory();

            var quickTransaction = new QuickTransaction
            {
                Account = accountFactory.CreateAccount(
                    "TestAccount", "Description", "USD"),
                Category = categoryFactory.CreateCategory("TestAccount", "Description", 0L, null),
                Name = "TestName",
                AskForWeight = false,
                AskForTotal = true,
                Total = 2.34m,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsTrue(result, "Interaction required");
        }

        [TestMethod]
        public void ToTransactionIncompleteAskForWeightTransactionTest()
        {
            var categoryFactory = new RegularCategoryFactory();
            var accountFactory = new RegularAccountFactory();

            var quickTransaction = new QuickTransaction
            {
                Account = accountFactory.CreateAccount(
                    "TestAccount", "Description", "USD"),
                Category = categoryFactory.CreateCategory("TestAccount", "Description", 0L, null),
                Name = "TestName",
                AskForWeight = true,
                AskForTotal = false,
                Total = 2.34m,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsTrue(result, "Interaction required");
        }

        [TestMethod]
        public void ToTransactionCompleteTransactionTest()
        {
            var categoryFactory = new RegularCategoryFactory();
            var accountFactory = new RegularAccountFactory();

            var quickTransaction = new QuickTransaction
            {
                Account = accountFactory.CreateAccount(
                    "TestAccount", "Description", "USD"),
                Category = categoryFactory.CreateCategory("TestAccount", "Description", 0L, null),
                Name = "TestName",
                AskForWeight = false,
                AskForTotal = false,
                Total = 2.34m,
                Weight = 1
            };

            var result = QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

            Assert.IsFalse(result, "Interaction don't required");
        }
    }
}
