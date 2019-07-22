using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

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

        public IEnumerable<ITransaction> GetAllTransactions(ITransactionFactory factory)
        {
            return _transactions.ToArray();
        }
    }
}
