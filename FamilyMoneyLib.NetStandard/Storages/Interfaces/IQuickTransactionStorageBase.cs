using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages.Interfaces
{
    public interface IQuickTransactionStorage
    {
        IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category, string name, decimal total, long id, decimal weight, bool askForTotal, bool askForWeight);
        IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction);
        void DeleteAllData();
        void DeleteQuickTransaction(IQuickTransaction quickTransaction);
        IEnumerable<IQuickTransaction> GetAllQuickTransactions();
        void UpdateQuickTransaction(IQuickTransaction quickTransaction);
    }
}