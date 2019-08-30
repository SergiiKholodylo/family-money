using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Cached
{
    [TestClass]
    public class CachedAccountStorageTest
    {
        private RegularAccountFactory _factory;
        private SqLiteAccountStorage _storage;

        private IAccount _account;

        [TestInitialize]
        public void Setup()
        {
            _factory = new RegularAccountFactory();
            _storage = new SqLiteAccountStorage(_factory);
            _storage.DeleteAllData();
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";


            _account = _factory.CreateAccount(accountName, accountDescription, accountCurrency);

        }

        [TestMethod]
        public void CreateAccountTest()
        {


            var newAccount = _storage.CreateAccount(_account);


            Assert.AreEqual(_account.Name, newAccount.Name);
            Assert.AreEqual(_account.Description, newAccount.Description);
            Assert.AreEqual(_account.Currency, newAccount.Currency);
        }

        [TestMethod]
        public void Create2CategoriesTest()
        {

            var account1 = _factory.CreateAccount("Account 1", "Description", "UAH",5);
            var account2 = _factory.CreateAccount("Account 2", "Description 2", "USD", 5);
            _storage.CreateAccount(account1);
            _storage.CreateAccount(account2);

            var newAccount = _storage.GetAllAccounts().FirstOrDefault();

            Assert.IsNotNull(newAccount);
            Assert.AreEqual(1, _storage.GetAllAccounts().Count());
            Assert.AreEqual(account2.Name, newAccount.Name);
            Assert.AreEqual(account2.Description, newAccount.Description);
            Assert.AreEqual(5, newAccount.Id);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var factory = new RegularAccountFactory();
            var storage = new SqLiteAccountStorage(factory);
            _account.Description = DateTime.Now.ToShortTimeString();
            storage.CreateAccount(_account);

            var firstAccount = storage.GetAllAccounts().Last();

            Assert.AreEqual(_account.Name, firstAccount.Name);
            Assert.AreEqual(_account.Description, firstAccount.Description);
            Assert.AreEqual(_account.Currency, firstAccount.Currency);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            _storage.CreateAccount(_account);
            _storage.DeleteAccount(_account);


            var numberOfAccounts = _storage.GetAllAccounts().Count();


            Assert.AreEqual(0, numberOfAccounts);
        }


        [TestMethod]
        public void UpdateAccountTest()
        {
            _storage.CreateAccount(_account);
            _account.Name = "New Name";
            _account.Description = "New Description";


            _storage.UpdateAccount(_account);


            var firstAccount = _storage.GetAllAccounts().First();
            Assert.AreEqual(_account.Name, firstAccount.Name);
            Assert.AreEqual(_account.Description, firstAccount.Description);
        }

    }
}
