using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface IAccountStorage
    {
        IAccount CreateAccount(IAccount account);

        void DeleteAccount(IAccount account);

        void UpdateAccount(IAccount account);

        IEnumerable<IAccount> GetAllAccounts(IAccountFactory factory);
    }
}
