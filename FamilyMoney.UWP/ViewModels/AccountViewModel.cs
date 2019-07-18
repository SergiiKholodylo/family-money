using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels
{
    public sealed class AccountViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<IAccount> _accounts;
        private readonly IAccountManager _manager;

        public ObservableCollection<IAccount> Accounts
        {
            private set
            {
                _accounts = value; 
                OnPropertyChanged();
            }
            get => _accounts;
        }


        public AccountViewModel()
        {
            _manager = MainPage.GlobalSettings.AccountManager;
            Accounts = new ObservableCollection<IAccount>(_manager.GetAllAccounts());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            Accounts.Clear();
            var allAccounts = _manager.GetAllAccounts();
            foreach (var account in allAccounts)
            {
                Accounts.Add(account);
            }
        }

        public void DeleteAccount(IAccount activeAccount)
        {
            _manager.DeleteAccount(activeAccount);
            Accounts.Remove(activeAccount);
        }
    }
}
