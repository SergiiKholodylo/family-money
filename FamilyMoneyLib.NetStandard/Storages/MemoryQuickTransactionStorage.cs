using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryQuickTransactionStorage:QuickTransactionStorageBase
    {
        private readonly List<IQuickTransaction> _quickTransactions = new List<IQuickTransaction>();
        public MemoryQuickTransactionStorage(IQuickTransactionFactory quickTransactionFactory) : 
            base(quickTransactionFactory)
        {
        }

        public override IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction)
        {
            _quickTransactions.Add(quickTransaction);
            return quickTransaction;
        }

        public override void DeleteQuickTransaction(IQuickTransaction quickTransaction)
        {
            _quickTransactions.Remove(quickTransaction);
        }

        public override IEnumerable<IQuickTransaction> GetAllQuickTransactions()
        {
            return _quickTransactions.ToArray();
        }

        public override void UpdateQuickTransaction(IQuickTransaction quickTransaction)
        {
        }

    }
}
