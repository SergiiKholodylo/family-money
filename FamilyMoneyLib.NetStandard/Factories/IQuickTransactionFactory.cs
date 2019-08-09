using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface IQuickTransactionFactory
    {
        IQuickTransaction CreateQuickTransaction();
        IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category, string name, decimal total, long id, decimal weight, bool askForTotal, bool askForWeight);
    }
}