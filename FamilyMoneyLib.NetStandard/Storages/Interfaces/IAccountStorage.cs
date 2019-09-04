using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages.Interfaces
{
    public interface IAccountStorage
    {
        IAccount CreateAccount(IAccount account);

        void DeleteAccount(IAccount account);

        void UpdateAccount(IAccount account);

        IEnumerable<IAccount> GetAllAccounts();
        IAccount CreateAccount(string name, string description, string currency);
        void DeleteAllData();
    }
}
