using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryAccountStorage: AccountStorageBase
    {
        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();
        public MemoryAccountStorage(IAccountFactory factory) : base(factory)
        {
        }

        public override IAccount CreateAccount(IAccount account)
        {
            return _storageEngine.Create(account) as IAccount;
        }

        public override void DeleteAccount(IAccount account)
        {
           _storageEngine.Delete(account);
        }

        public override void UpdateAccount(IAccount account)
        {
            
        }

        public override IEnumerable<IAccount> GetAllAccounts()
        {
            return _storageEngine.GetAll().Cast<IAccount>();
        }

        public override void DeleteAllData()
        {
            _storageEngine.DeleteAllData();
        }


    }
}
