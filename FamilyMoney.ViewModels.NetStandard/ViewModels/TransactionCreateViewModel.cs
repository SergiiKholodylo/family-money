using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public class TransactionCreateViewModel : TransactionViewModelBase, ITransactionViewModel
    {
        public TransactionCreateViewModel(Storages storages, IAccount activeAccount) : base(storages)
        {
            if (activeAccount != null)
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
        }

        public TransactionCreateViewModel(Storages storages,ITransaction templateTransaction) : base(storages)
        {
            if(templateTransaction == null) return;

            if (templateTransaction.Account != null)
                Account = Accounts.FirstOrDefault(x => x.Id == templateTransaction.Account.Id);
            if (templateTransaction.Category != null)
                Category = Categories.FirstOrDefault(x => x.Id == templateTransaction.Category.Id);
            Name = templateTransaction.Name;
            Total = templateTransaction.Total;
            Weight = templateTransaction.Weight;

        }

        public TransactionCreateViewModel(Storages storages):base(storages)
        {
        }

        public void SaveTransaction()
        {
            CreateTransaction();
        }

    }
}
