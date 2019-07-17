using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryTransactionStorage : ITransactionStorage
    {
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        public ITransaction CreateTransaction(ITransaction transaction)
        {
            _transactions.Add(transaction);
            return transaction;
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            _transactions.Remove(transaction);
        }

        public void UpdateTransaction(ITransaction transaction)
        {

        }

        public IEnumerable<ITransaction> GetAllTransactions()
        {
            return _transactions.ToArray();
        }
    }
}
