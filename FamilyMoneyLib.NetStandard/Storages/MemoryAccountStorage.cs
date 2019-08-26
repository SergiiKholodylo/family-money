using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryAccountStorage: AccountStorageBase, IAccountStorage
    {
        private readonly List<IAccount> _accounts = new List<IAccount>();
        private static long counter = 0;
        public override IAccount CreateAccount(IAccount account)
        {
            account.Id = ++counter;
            _accounts.Add(account);
            return account;
        }

        public override void DeleteAccount(IAccount account)
        {
            _accounts.Remove(account);
        }

        public override void UpdateAccount(IAccount account)
        {
            
        }

        public override IEnumerable<IAccount> GetAllAccounts()
        {
            return _accounts.ToArray();
        }

        public override void DeleteAllData()
        {
            _accounts.Clear();
        }

        public MemoryAccountStorage(IAccountFactory factory) : base(factory)
        {
        }
    }
}
