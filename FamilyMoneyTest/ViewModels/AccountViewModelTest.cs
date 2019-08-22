using System.Linq;
using FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class AccountViewModelTest
    {
        private const string TestAccount = "Test Account";
        private const string Description = "Description";
        private const string Usd = "USD";
        private string _fieldName = "";

        [TestMethod]
        public void CreateNewAccountTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var viewModel = new EditAccountViewModel(storage)
            {
                Name = TestAccount,
                Description = Description,
                Currency = Usd
            };

            viewModel.CreateNewAccount();
            var newAccount = storage.GetAllAccounts().FirstOrDefault();

            Assert.IsNotNull(newAccount);
            Assert.AreEqual(TestAccount, newAccount.Name);
            Assert.AreEqual(Description, newAccount.Description);
            Assert.AreEqual(Usd, newAccount.Currency);
        }

        [TestMethod]
        public void UpdateAccountTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var account = storage.CreateAccount("New Account", "Old description", "UAH");

            var viewModel = new EditAccountViewModel(storage,account);

            viewModel.Name = TestAccount;
            viewModel.Description = Description;
            viewModel.Currency = Usd;

            viewModel.UpdateAccount();
            var newAccount = storage.GetAllAccounts().FirstOrDefault();

            Assert.IsNotNull(newAccount);
            Assert.AreEqual(account.Id, newAccount.Id);
            Assert.AreEqual(TestAccount, newAccount.Name);
            Assert.AreEqual(Description, newAccount.Description);
            Assert.AreEqual(Usd, newAccount.Currency);

        }

        [TestMethod]
        public void EventNameTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var viewModel = new EditAccountViewModel(storage);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Name = "Name";
            Assert.AreEqual("Name", _fieldName);
        }

        [TestMethod]
        public void EventDescriptionTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var viewModel = new EditAccountViewModel(storage);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Description = "Desc";
            Assert.AreEqual("Description", _fieldName);
        }

        [TestMethod]
        public void EventCurrencyTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var viewModel = new EditAccountViewModel(storage);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Currency = "Name";
            Assert.AreEqual("Currency", _fieldName);
        }

        [TestMethod]
        public void EventErrorStringTest()
        {
            var storage = new MemoryAccountStorage(new RegularAccountFactory());
            var viewModel = new EditAccountViewModel(storage);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.ErrorString = "Name";
            Assert.AreEqual("ErrorString", _fieldName);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _fieldName = e.PropertyName;
        }
    }
}
