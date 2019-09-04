using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public sealed class AccountViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<IAccount> _accounts;
        private readonly IAccountStorage _storage;

        public ObservableCollection<IAccount> Accounts
        {
            private set
            {
                _accounts = value; 
                OnPropertyChanged();
            }
            get => _accounts;
        }


        public AccountViewModel(IAccountStorage accountStorage)
        {
            _storage = accountStorage;
            Accounts = new ObservableCollection<IAccount>(_storage.GetAllAccounts());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            Accounts.Clear();
            var allAccounts = _storage.GetAllAccounts();
            foreach (var account in allAccounts)
            {
                Accounts.Add(account);
            }
        }

        public void DeleteAccount(IAccount activeAccount)
        {
            _storage.DeleteAccount(activeAccount);
            Accounts.Remove(activeAccount);
        }
    }
}
