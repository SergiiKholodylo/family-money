using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class AccountStorageBase:IAccountStorage
    {
        protected IAccountFactory _accountFactory;

        protected AccountStorageBase(IAccountFactory factory)
        {
            _accountFactory = factory;
        }

        public abstract IAccount CreateAccount(IAccount account);

        public abstract void DeleteAccount(IAccount account);

        public abstract void UpdateAccount(IAccount account);

        public abstract IEnumerable<IAccount> GetAllAccounts();
    }
}
