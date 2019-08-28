using System;
using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class TransactionStorageBase:ITransactionStorage
    {
        protected readonly ITransactionFactory TransactionFactory;

        protected TransactionStorageBase(ITransactionFactory transactionFactory)
        {
            TransactionFactory = transactionFactory;
        }

        public abstract ITransaction CreateTransaction(ITransaction transaction);

        public ITransaction CreateTransaction(IAccount account, ICategory category, 
            string name, decimal total, DateTime timestamp,
            long id, decimal weight, IProduct product, ITransaction parentTransaction)
        {
            var transaction = TransactionFactory.CreateTransaction(account, category, name, total, timestamp, id, weight, product, parentTransaction);
            return CreateTransaction(transaction);
        }

        public abstract void DeleteTransaction(ITransaction transaction);
        public abstract IEnumerable<ITransaction> GetAllTransactions();
        public abstract void UpdateTransaction(ITransaction transaction);
        public void AddChildTransaction(ITransaction parent, ITransaction child)
        {
            var transaction = (Transaction)parent;
            child.Parent = parent;
            transaction.AddChildTransaction(child);
            UpdateTransaction(child);
            UpdateTransaction(transaction);
        }

        public void DeleteAllChildrenTransactions(ITransaction parent)
        {
            parent.DeleteChildrenTransactions();

        }

        public void DeleteChildrenTransaction(ITransaction parent, ITransaction child)
        {
            parent.DeleteChildTransaction(child);
            DeleteTransaction(child);
        }

        public virtual void DeleteAllData()
        {
        }
    }
}
