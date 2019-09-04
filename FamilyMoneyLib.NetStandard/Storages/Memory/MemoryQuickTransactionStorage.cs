using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages.Memory
{
    public class MemoryQuickTransactionStorage:QuickTransactionStorageBase
    {
        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();

        public MemoryQuickTransactionStorage(IQuickTransactionFactory quickTransactionFactory) : 
            base(quickTransactionFactory)
        {
        }

        public override IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction)
        {
            return _storageEngine.Create(quickTransaction) as IQuickTransaction;
        }

        public override void DeleteAllData()
        {
            _storageEngine.DeleteAllData();
        }

        public override void DeleteQuickTransaction(IQuickTransaction quickTransaction)
        {
            _storageEngine.Delete(quickTransaction);
        }

        public override IEnumerable<IQuickTransaction> GetAllQuickTransactions()
        {
            return _storageEngine.GetAll().Cast<IQuickTransaction>();
        }

        public override void UpdateQuickTransaction(IQuickTransaction quickTransaction)
        {
        }
    }
}
