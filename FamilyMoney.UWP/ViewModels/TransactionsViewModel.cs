using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionsViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<Account> Accounts { set; get; } = new ObservableCollection<Account>();

        public ObservableCollection<Transaction> Transactions { set; get; } = new ObservableCollection<Transaction>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
