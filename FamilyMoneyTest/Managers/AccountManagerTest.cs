using System.Linq;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Managers
{
    [TestClass]
    public class AccountManagerTest
    {
        [TestMethod]
        public void CreateAccountTest()
        {
            var storage = new MemoryAccountStorage();
            var factory = new RegularAccountFactory();

            var manager = new AccountManager(factory, storage);
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";

            var account = manager.CreateAccount(accountName, accountDescription,accountCurrency);


            Assert.AreEqual(account.Name, accountName);
            Assert.AreEqual(account.Description, accountDescription);
            Assert.AreEqual(account.Currency, accountCurrency);

        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var storage = new MemoryAccountStorage();
            var factory = new RegularAccountFactory();

            var manager = new AccountManager(factory, storage);
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";

            var account = manager.CreateAccount(accountName, accountDescription, accountCurrency);

            var firstAccount = storage.GetAllAccounts(factory).First();

            Assert.AreEqual(account.Name, firstAccount.Name);
            Assert.AreEqual(account.Description, firstAccount.Description);
            Assert.AreEqual(account.Currency, firstAccount.Currency);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var storage = new MemoryAccountStorage();
            var factory = new RegularAccountFactory();

            var manager = new AccountManager(factory, storage);
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";

            var account = manager.CreateAccount(accountName, accountDescription, accountCurrency);
            storage.DeleteAccount(account);


            var numberOfAccounts = storage.GetAllAccounts(factory).Count();


            Assert.AreEqual(0, numberOfAccounts);
        }


        [TestMethod]
        public void UpdateAccountTest()
        {
            var storage = new MemoryAccountStorage();
            var factory = new RegularAccountFactory();

            var manager = new AccountManager(factory, storage);
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";

            var account = manager.CreateAccount(accountName, accountDescription, accountCurrency);
            account.Name = "New Name";
            account.Description = "New Description";


            storage.UpdateAccount(account);


            var firstAccount = storage.GetAllAccounts(factory).First();
            Assert.AreEqual(account.Name, firstAccount.Name);
            Assert.AreEqual(account.Description, firstAccount.Description);
        }
    }
}
