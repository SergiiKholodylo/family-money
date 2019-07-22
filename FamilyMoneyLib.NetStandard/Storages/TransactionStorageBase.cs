using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class TransactionStorageBase:ITransactionStorage
    {
        private ITransactionFactory _transactionFactory;

        protected TransactionStorageBase(ITransactionFactory transactionFactory)
        {
            _transactionFactory = transactionFactory;
        }

        public abstract ITransaction CreateTransaction(ITransaction transaction);
        public abstract void DeleteTransaction(ITransaction transaction);
        public abstract IEnumerable<ITransaction> GetAllTransactions();
        public abstract void UpdateTransaction(ITransaction transaction);
    }
}
