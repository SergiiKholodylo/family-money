using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public interface IAccountManager
    {
        IAccount CreateAccount(string name, string description, string currency);
        void DeleteAccount(IAccount account);
        IEnumerable<IAccount> GetAllAccounts();
        void UpdateAccount(IAccount account);
    }
}