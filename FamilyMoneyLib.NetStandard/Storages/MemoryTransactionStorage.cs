using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryTransactionStorage : TransactionStorageBase
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        public MemoryTransactionStorage(ITransactionFactory transactionFactory) : base(transactionFactory)
        {
        }

        public override ITransaction CreateTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);
            return transaction;
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            _transactions.Remove(transaction);
        }

        public override void UpdateTransaction(ITransaction transaction)
        {

        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            return _transactions.ToArray();
        }
    }
}
