using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class TransactionStorage
    {
        public IDbStorage<Transaction> DataBaseConnector = new DbStorage<Transaction>();

        public long AddTransaction(Transaction transaction)
        {
            return DataBaseConnector.Add(transaction);
        }

        public Transaction GetTransaction(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            DataBaseConnector.Update(transaction);
        }

        public void DeleteTransaction(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return DataBaseConnector.GetAll();
        }

        public IEnumerable<Transaction> GetTransactionByCategory(Category category)
        {
            return DataBaseConnector.GetAll().Where(x => x.Category == category);
        }
    }
}
