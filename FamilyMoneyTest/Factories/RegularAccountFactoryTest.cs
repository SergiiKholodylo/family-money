using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Factories
{
    [TestClass]
    public class RegularAccountFactoryTest
    {
        [TestMethod]
        public void TestCreateAccount()
        {
            var factory = new RegularAccountFactory();
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";


            var account = factory.CreateAccount(accountName, accountDescription, accountCurrency);


            Assert.AreEqual(accountName, account.Name);
            Assert.AreEqual(accountDescription, account.Description);
            Assert.AreEqual(accountCurrency, account.Currency);
        }
    }
}
