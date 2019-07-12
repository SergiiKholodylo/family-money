using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class AccountStorage : IAccountReaderStorage
    {
        public IDbStorage<Account> DataBaseConnector = new DbStorage<Account>();

        public long AddAccount(Account account)
        {
            return DataBaseConnector.Add(account);
        }

        public Account GetAccount(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateAccount(Account account)
        {
            DataBaseConnector.Update(account);
        }

        public void DeleteAccount(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return DataBaseConnector.GetAll();
        }
    }
}
