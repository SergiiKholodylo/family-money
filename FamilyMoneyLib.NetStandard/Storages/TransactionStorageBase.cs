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
        public abstract void DeleteTransaction(ITransaction transaction);
        public abstract IEnumerable<ITransaction> GetAllTransactions();
        public abstract void UpdateTransaction(ITransaction transaction);
    }
}
