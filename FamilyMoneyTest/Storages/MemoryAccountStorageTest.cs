using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Storages
{
    [TestClass]
    public class MemoryAccountStorageTest
    {
        private IAccountFactory _factory;
        private MemoryAccountStorage _storage;
        private IAccount _account;

        [TestInitialize]
        public void Setup()
        {
            _factory = new RegularAccountFactory();
            _storage = new MemoryAccountStorage(_factory);
            _account = CreateAccount();
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
        public void CreateAccountWithCodeTest()
        {

            var account = _factory.CreateAccount("Account", "Description", "UAH",5);


            var newAccount = _storage.CreateAccount(account);


            Assert.AreEqual(5, newAccount.Id);
        }

        [TestMethod]
        public void CreateTwoAccountsWithSameCodeTest()
        {

            var account1 = _factory.CreateAccount("Category", "Description","UAH", 5);
            var account2 = _factory.CreateAccount("Updated Category", "Updated Description","USD", 5);


            _storage.CreateAccount(account1);
            _storage.CreateAccount(account2);
            var accountsList = _storage.GetAllAccounts().ToArray();
            var accountFromStorage = accountsList.FirstOrDefault();

            Assert.IsNotNull(accountFromStorage);
            Assert.IsNotNull(accountsList);
            Assert.AreEqual(1, accountsList.Count());
            Assert.AreEqual(5, accountFromStorage.Id);
            Assert.AreEqual("Updated Category", accountFromStorage.Name);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {

            _storage.CreateAccount(_account);

            var firstAccount = _storage.GetAllAccounts().First();

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

        private IAccount CreateAccount()
        {
            var accountName = "Test Account";
            var accountDescription = "Test Description";
            var accountCurrency = "USD";


            var account1 = _factory.CreateAccount(accountName, accountDescription, accountCurrency);

            return account1;
        }
    }
}
