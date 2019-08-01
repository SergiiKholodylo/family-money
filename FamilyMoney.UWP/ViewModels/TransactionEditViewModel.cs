using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionEditViewModel : TransactionViewModelBase, ITransactionViewModel
    {
        public TransactionEditViewModel(ITransaction transaction)
        {
            Transaction = transaction;
            Date = transaction == null || transaction.Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(transaction.Timestamp);
            Time = transaction?.Timestamp.TimeOfDay ?? DateTime.Now.TimeOfDay;
            Name = transaction?.Name;
            Account = Accounts.FirstOrDefault(x => x.Id == transaction?.Account.Id);
            Category = Categories.FirstOrDefault(x => x.Id == transaction?.Category.Id);
            Timestamp = transaction?.Timestamp ?? DateTime.Now;
            if (transaction != null)
            {
                Total = transaction.Total;
                Weight = transaction.Weight;
                IsComplexTransaction = transaction.IsComplexTransaction;
                ParentTransaction = (ITransaction)transaction.Parent;
            }

            ChildrenTransactions = transaction == null ?
                new ObservableCollection<ITransaction>() :
                new ObservableCollection<ITransaction>(transaction.Children.Select(x => (ITransaction)x));
        }

        public void SaveTransaction()
        {
            UpdateTransaction();
        }
    }
}