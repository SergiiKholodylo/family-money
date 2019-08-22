using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs
{
    public sealed class EditAccountViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _currency;
        private readonly IAccount _account;
        private readonly IAccountStorage _accountStorage;
        private string _errorString;

        public EditAccountViewModel(IAccountStorage accountStorage)
        {
            _accountStorage = accountStorage;
        }

        public EditAccountViewModel(IAccountStorage accountStorage, IAccount account)
        {
            Name = account.Name;
            Description = account.Description;
            Currency = account.Currency;
            _account = account;
            _accountStorage = accountStorage;
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged();}
            get => _name;
        }

        public string Description
        {
            set { _description = value; OnPropertyChanged(); }
            get => _description;
        }

        public string Currency
        {
            set { _currency = value; OnPropertyChanged(); }
            get => _currency;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public void CreateNewAccount()
        {
            try
            {
                _accountStorage.CreateAccount(Name, Description, Currency);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during creating Account {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        public void UpdateAccount()
        {
            if (_account == null)
                throw new ViewModelException("There is no Account to update");
            _account.Currency = Currency;
            _account.Description = Description;
            _account.Name = Name;
            try
            {
                _accountStorage.UpdateAccount(_account);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during creating category {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
