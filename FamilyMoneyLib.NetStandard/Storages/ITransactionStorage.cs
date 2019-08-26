using System;
using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface ITransactionStorage
    {
        ITransaction CreateTransaction(ITransaction transaction);
        ITransaction CreateTransaction(IAccount account, ICategory category,
            string name, decimal total, DateTime timestamp,
            long id, decimal weight, IProduct product, ITransaction parentTransaction);

        void DeleteTransaction(ITransaction transaction);
        IEnumerable<ITransaction> GetAllTransactions();
        void UpdateTransaction(ITransaction transaction);

        void AddChildrenTransaction(ITransaction parent, ITransaction child);
        void DeleteAllChildrenTransactions(Transaction parent);
        void DeleteChildrenTransaction(Transaction parent, Transaction child);

        void DeleteAllData();
    }
}