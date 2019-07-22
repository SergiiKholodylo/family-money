using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface ITransactionStorage
    {
        ITransaction CreateTransaction(ITransaction transaction);
        void DeleteTransaction(ITransaction transaction);
        IEnumerable<ITransaction> GetAllTransactions(ITransactionFactory factory);
        void UpdateTransaction(ITransaction transaction);
    }
}