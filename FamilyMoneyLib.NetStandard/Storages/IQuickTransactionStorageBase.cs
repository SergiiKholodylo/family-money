using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface IQuickTransactionStorageBase
    {
        IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category, string name, decimal total, long id, decimal weight, bool askForTotal, bool askForWeight);
        IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction);
        void DeleteQuickTransaction(IQuickTransaction quickTransaction);
        IEnumerable<IQuickTransaction> GetAllQuickTransactions();
        void UpdateQuickTransaction(IQuickTransaction quickTransaction);
    }
}