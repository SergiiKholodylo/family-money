using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class QuickTransactionStorageBase : IQuickTransactionStorageBase
    {
        protected readonly IQuickTransactionFactory _quickTransactionFactory;

        protected QuickTransactionStorageBase(IQuickTransactionFactory quickTransactionFactory)
        {
            _quickTransactionFactory = quickTransactionFactory;
        }

        public IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category,
            string name, decimal total, long id, decimal weight, bool askForTotal, bool askForWeight)
        {
            var quickTransaction = _quickTransactionFactory.CreateQuickTransaction(account,category,name,total,id,weight,askForTotal,askForWeight);

            return CreateQuickTransaction(quickTransaction);
        }

        public abstract IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction);

        public abstract void DeleteQuickTransaction(IQuickTransaction quickTransaction);
        public abstract void UpdateQuickTransaction(IQuickTransaction quickTransaction);
        public abstract IEnumerable<IQuickTransaction> GetAllQuickTransactions();
    }
}
