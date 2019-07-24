using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.SQLite
{
    [TestClass]
    public class SqLiteAccountStorageTest
    {
        [TestMethod]
        public void CreateAccountTest()
        {
            var factory = new RegularAccountFactory();
            var storage = new SqLiteAccountStorage(factory);
            var account = CreateAccount();


            var newAccount = storage.CreateAccount(account);


            Assert.AreEqual(account.Name, newAccount.Name);
            Assert.AreEqual(account.Description, newAccount.Description);
            Assert.AreEqual(account.Currency, newAccount.Currency);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var factory = new RegularAccountFactory();
            var storage = new SqLiteAccountStorage(factory);
            var account = CreateAccount();
            account.Description = DateTime.Now.ToShortTimeString();
            storage.CreateAccount(account);

            var firstAccount = storage.GetAllAccounts().Last();

            Assert.AreEqual(account.Name, firstAccount.Name);
            Assert.AreEqual(account.Description, firstAccount.Description);
            Assert.AreEqual(account.Currency, firstAccount.Currency);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var factory = new RegularAccountFactory();
            var storage = new SqLiteAccountStorage(factory);
            storage.DeleteAllData();
            var account = CreateAccount();
            storage.CreateAccount(account);
            storage.DeleteAccount(account);


            var numberOfAccounts = storage.GetAllAccounts().Count();


            Assert.AreEqual(0, numberOfAccounts);
        }


        [TestMethod]
        public void UpdateAccountTest()
        {
            var factory = new RegularAccountFactory();
            var storage = new SqLiteAccountStorage(factory);
            storage.DeleteAllData();
            var account = CreateAccount();
            storage.CreateAccount(account);
            account.Name = "New Name";
            account.Description = "New Description";


            storage.UpdateAccount(account);


            var firstAccount = storage.GetAllAccounts().First();
            Assert.AreEqual(account.Name, firstAccount.Name);
            Assert.AreEqual(account.Description, firstAccount.Description);
        }

        private IAccount CreateAccount()
        {
            var factory = new RegularAccountFactory();
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";


            var account = factory.CreateAccount(accountName, accountDescription, accountCurrency);

            return account;
        }
    }
}
