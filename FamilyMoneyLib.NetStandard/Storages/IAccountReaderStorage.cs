using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard
{
    public interface IAccountReaderStorage
    {
        Account GetAccount(long id);
        IEnumerable<Account> GetAllAccounts();
    }
}