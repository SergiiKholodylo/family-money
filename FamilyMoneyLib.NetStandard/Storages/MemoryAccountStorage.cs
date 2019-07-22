using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryAccountStorage:IAccountStorage
    {
        private readonly List<IAccount> _accounts = new List<IAccount>();

        public IAccount CreateAccount(IAccount account)
        {
            _accounts.Add(account);
            return account;
        }

        public  void DeleteAccount(IAccount account)
        {
            _accounts.Remove(account);
        }

        public  void UpdateAccount(IAccount account)
        {
            
        }

        public IEnumerable<IAccount> GetAllAccounts(IAccountFactory factory)
        {
            return _accounts.ToArray();
        }
    }
}
