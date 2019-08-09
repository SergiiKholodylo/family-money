using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionCreateViewModel : TransactionViewModelBase, ITransactionViewModel
    {
        public TransactionCreateViewModel(IAccount activeAccount) : base()
        {
            if (activeAccount != null)
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
        }

        public TransactionCreateViewModel(ITransaction templateTransaction) : base()
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

        public TransactionCreateViewModel():base()
        {
        }

        public void SaveTransaction()
        {
            CreateTransaction();
        }

    }
}
