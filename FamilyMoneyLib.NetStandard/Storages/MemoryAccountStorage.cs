using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryAccountStorage: AccountStorageBase, IAccountStorage
    {
        private readonly List<IAccount> _accounts = new List<IAccount>();
        private static long _counter = 0;

        public override IAccount CreateAccount(IAccount account)
        {
            if (IsExists(account))
            {
                DeleteAccount(account);
            }
            else
            {
                if (account.Id == 0)
                    account.Id = ++_counter;
            }
            _accounts.Add(account);
            return account;
        }

        public override void DeleteAccount(IAccount account)
        {
            var accountsToDelete = _accounts.Where(x => x.Id == account.Id).ToArray();
            foreach (var account1Account in accountsToDelete)
            {
                _accounts.Remove(account1Account);
            }
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

        private bool IsExists(IAccount category)
        {
            return (category.Id != 0 && _accounts.Any(x => x.Id == category.Id));
        }
    }
}
