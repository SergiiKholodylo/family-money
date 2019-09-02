using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryTransactionStorage : TransactionStorageBase
    {
        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();

        public MemoryTransactionStorage(ITransactionFactory transactionFactory) : base(transactionFactory)
        {
        }

        public override ITransaction CreateTransaction(ITransaction transaction)
        {
            return _storageEngine.Create(transaction) as ITransaction;
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            _storageEngine.Delete(transaction);
        }

        public override void UpdateTransaction(ITransaction transaction)
        {

        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            return _storageEngine.GetAll().Cast<ITransaction>();
        }
    }
}
