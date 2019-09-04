using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages.Memory
{
    public class MemoryTransactionStorage : TransactionStorageBase
    {
        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();

        public MemoryTransactionStorage(ITransactionFactory transactionFactory) : base(transactionFactory)
        {
        }

        public override ITransaction CreateTransaction(ITransaction transaction)
        {
            CheckParameters(transaction);

            return _storageEngine.Create(transaction) as ITransaction;
        }

        private static void CheckParameters(ITransaction transaction)
        {
            if (transaction.Account == null) throw new StorageException("Account mustn't be NULL");
            if (transaction.Category == null) throw new StorageException("Category mustn't be NULL");
        }

        public override void DeleteTransaction(ITransaction transaction)
        {
            _storageEngine.Delete(transaction);
        }

        public override void UpdateTransaction(ITransaction transaction)
        {
            CheckParameters(transaction);
        }

        public override IEnumerable<ITransaction> GetAllTransactions()
        {
            return _storageEngine.GetAll().Cast<ITransaction>();
        }
    }
}
