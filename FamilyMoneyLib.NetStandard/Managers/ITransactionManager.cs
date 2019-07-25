using System;
using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public interface ITransactionManager
    {
        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total);

        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp);

        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp, long id, decimal weight, IProduct product, ITransaction parentTransaction);

        void DeleteTransaction(ITransaction transaction);
        IEnumerable<ITransaction> GetAllTransactions();
        void UpdateTransaction(ITransaction transaction);
    }
}