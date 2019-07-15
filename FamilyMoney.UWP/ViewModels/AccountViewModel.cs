using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels
{
    public class AccountViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<IAccount> _accounts = new ObservableCollection<IAccount>();
        private readonly AccountManager _manager;

        public ObservableCollection<IAccount> Accounts
        {
            set
            {
                _accounts = value; 
                OnPropertyChanged();
            }
            get => _accounts;
        }


        public AccountViewModel()
        {
            IAccountFactory factory = new RegularAccountFactory();
            IAccountStorage storage = new MemoryAccountStorage();
            _manager = new AccountManager(factory, storage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddAccount()
        {
            Accounts.Add(_manager.CreateAccount("Account", "Description", "UAH"));
        }
    }
}
