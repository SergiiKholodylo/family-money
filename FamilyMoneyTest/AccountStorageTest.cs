using System.Linq;
using FamilyMoneyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest
{
    [TestClass]
    public class AccountStorageTest
    {
        [TestMethod]
        public void AccountAddAccountTest()
        {
            var account = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var storage = new AccountStorage();


            storage.AddAccount(account);


            Assert.AreNotEqual(Account.NewAccountId, account.Id);
        }

        [TestMethod]
        public void AccountAddMultiplyAccountTest()
        {
            var account1 = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var account2 = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var account3 = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };

            var storage = new AccountStorage();


            storage.AddAccount(account1);
            storage.AddAccount(account2);
            storage.AddAccount(account3);
            var accounts = storage.GetAllAccounts();


            Assert.IsTrue(account1.Id != account2.Id && account2.Id != account3.Id && account1.Id != account3.Id);
            Assert.AreEqual(3, accounts.Count());
        }

        [TestMethod]
        public void AccountAddAccountAndGetAccountTest()
        {
            var account1 = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var storage = new AccountStorage();


            var accountId = storage.AddAccount(account1);

            var storedAccount = storage.GetAccount(accountId);


            Assert.AreEqual(accountId, storedAccount.Id, "Account's Ids must be equal");
            Assert.AreEqual("USD", storedAccount.Currency, "Account currency must be USD");
        }

        [TestMethod]
        public void AccountUpdateAccountAndGetAccountTest()
        {
            var account = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var storage = new AccountStorage();


            storage.AddAccount(account);

            var newAccount = new Account
            {
                Name = "updated account",
                Description = "updated description",
                Currency = "EUR",
                Id = account.Id
            };

            storage.UpdateAccount(newAccount);
            var storedAccount = storage.GetAccount(account.Id);


            Assert.AreEqual(newAccount.Id, storedAccount.Id, "Account's Ids must be equal");
            Assert.AreEqual(newAccount.Name, storedAccount.Name, "Account's Names must be equal");
            Assert.AreEqual(newAccount.Description, storedAccount.Description, "Account's Descriptions must be equal");
            Assert.AreEqual(newAccount.Currency, storedAccount.Currency, "Account's Currencies must be equal");
        }

        [TestMethod]
        public void AccountDeleteAccountTest()
        {
            var account = new Account
            {
                Name = "account",
                Description = "description",
                Currency = "USD"
            };
            var storage = new AccountStorage();


            storage.AddAccount(account);

            storage.DeleteAccount(account.Id);
            var storedAccount = storage.GetAccount(account.Id);


            Assert.AreEqual(Account.NewAccountId, storedAccount.Id, "Account must be Empty");
        }

        [TestMethod]
        public void AccountDeleteNonexistentAccountTest()
        {
            const long anyAccountNumber = 567L;
            var storage = new AccountStorage();


            storage.DeleteAccount(anyAccountNumber);


            Assert.AreEqual(0, 0);
        }
    }
}
