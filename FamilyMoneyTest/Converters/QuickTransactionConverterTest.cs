using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Converters
{
    [TestClass]
    public class QuickTransactionConverterTest
    {
        [TestMethod]
        public void ToTransactionTest()
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

            var transaction =
                QuickTransactionConverter.ToTransaction(new RegularTransactionFactory(), quickTransaction);

            Assert.AreEqual(quickTransaction.Account, transaction.Account);
            Assert.AreEqual(quickTransaction.Category, transaction.Category);
            Assert.AreEqual(quickTransaction.Total, transaction.Total);
            Assert.AreEqual(quickTransaction.Weight, transaction.Weight);
            Assert.AreEqual(quickTransaction.Name, transaction.Name);

        }
    }
}
