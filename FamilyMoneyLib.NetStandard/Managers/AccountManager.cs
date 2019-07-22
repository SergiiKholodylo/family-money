using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountFactory _accountFactory;
        private readonly IAccountStorage _accountStorage;

        public AccountManager(IAccountFactory factory, IAccountStorage storage)
        {
            _accountFactory = factory;
            _accountStorage = storage;
        }

        public IAccount CreateAccount(string name, string description, string currency)
        {
            var account = _accountFactory.CreateAccount(name, description, currency);
            return _accountStorage.CreateAccount(account);
        }

        public void UpdateAccount(IAccount account)
        {
            _accountStorage.UpdateAccount(account);
        }

        public void DeleteAccount(IAccount account)
        {
            _accountStorage.DeleteAccount(account);
        }

        public IEnumerable<IAccount> GetAllAccounts()
        {
            return _accountStorage.GetAllAccounts();
        }
    }


}
