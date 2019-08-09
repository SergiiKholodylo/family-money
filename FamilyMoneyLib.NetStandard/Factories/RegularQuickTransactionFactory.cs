using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularQuickTransactionFactory : IQuickTransactionFactory
    {
        public IQuickTransaction CreateQuickTransaction()
        {
            return new QuickTransaction();
        }

        public IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category,
            string name, decimal total, long id, decimal weight, bool askForTotal, bool askForWeight)
        {
            var quickTransaction = new QuickTransaction
            {
                Id = id,
                Account = account,
                Category = category,
                Name = name,
                Total = total,
                Weight = weight,
                AskForTotal = askForTotal,
                AskForWeight = askForWeight
            };
            return quickTransaction;
        }
    }
}
