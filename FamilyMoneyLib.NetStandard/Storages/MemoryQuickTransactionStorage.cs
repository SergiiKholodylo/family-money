using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryQuickTransactionStorage:QuickTransactionStorageBase
    {
        private readonly List<IQuickTransaction> _quickTransactions = new List<IQuickTransaction>();
        private long _counter = 0;

        public MemoryQuickTransactionStorage(IQuickTransactionFactory quickTransactionFactory) : 
            base(quickTransactionFactory)
        {
        }

        public override IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction)
        {
            if (IsExists(quickTransaction))
            {
                _quickTransactions.Remove(quickTransaction);
            }
            else
            {
                if (quickTransaction.Id == 0)
                    quickTransaction.Id = ++_counter;
            }
            _quickTransactions.Add(quickTransaction);
            return quickTransaction;
        }

        public override void DeleteAllData()
        {
            _quickTransactions.Clear();
        }

        public override void DeleteQuickTransaction(IQuickTransaction quickTransaction)
        {
            var quickTransactionsToDelete = _quickTransactions.Where(x => x.Id == quickTransaction.Id).ToArray();
            foreach (var qtTransaction in quickTransactionsToDelete)
            {
                _quickTransactions.Remove(qtTransaction);
            }
        }

        public override IEnumerable<IQuickTransaction> GetAllQuickTransactions()
        {
            return _quickTransactions.ToArray();
        }

        public override void UpdateQuickTransaction(IQuickTransaction quickTransaction)
        {
        }

        private bool IsExists(IQuickTransaction quickTransaction)
        {
            return (quickTransaction.Id != 0 && _quickTransactions.Any(x => x.Id == quickTransaction.Id));
        }

    }
}
