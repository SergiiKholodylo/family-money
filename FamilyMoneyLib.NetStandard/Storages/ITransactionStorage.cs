using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface ITransactionStorage
    {
        ITransaction CreateTransaction(ITransaction transaction);
        void DeleteTransaction(ITransaction transaction);
        IEnumerable<ITransaction> GetAllTransactions();
        void UpdateTransaction(ITransaction transaction);
    }
}