using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryTransactionStorage : TransactionStorageBase
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();
        private static long _counter = 0;

        public MemoryTransactionStorage(ITransactionFactory transactionFactory) : base(transactionFactory)
        {
        }

        public override ITransaction CreateTransaction(ITransaction transaction)
        {
            if (IsExists(transaction))
            {
                _transactions.Remove(transaction);
            }
            else
            {
                if (transaction.Id == 0)
                    transaction.Id = ++_counter;
            }
            
            _transactions.Add(transaction);
            return transaction;
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            var quickTransactionsToDelete = _transactions.Where(x => x.Id == transaction.Id).ToArray();
            foreach (var qtTransaction in quickTransactionsToDelete)
            {
                _transactions.Remove(qtTransaction);
            }
        }

        public override void UpdateTransaction(ITransaction transaction)
        {

        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            return _transactions.ToArray();
        }

        private bool IsExists(ITransaction transaction)
        {
            return (transaction.Id != 0 && _transactions.Any(x => x.Id == transaction.Id));
        }
    }
}
