using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditChildTransactionViewModel:INotifyPropertyChanged
    {
        private  IAccount _account;
        private  ICategory _category;
        private  string _name;
        private  DateTime _timestamp = DateTime.Now;
        private  decimal _total;
        private DateTimeOffset _date;
        private TimeSpan _time;
        private readonly ITransaction _transaction;
        private string _errorString;
        private decimal _weight;

        public EditChildTransactionViewModel(ITransaction parent, IAccount activeAccount, ITransaction transaction)
        {
            ErrorString = String.Empty;
            _transaction = transaction;
            Categories = MainPage.GlobalSettings.CategoryStorage.MakeFlatCategoryTree();
            if (transaction == null)
            {
                Timestamp = DateTime.Now;
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
            }
            else
            {
                Name = transaction.Name;
                Timestamp = transaction.Timestamp;
                Total = transaction.Total;
                Weight = transaction.Weight;
                
                Category = Categories.FirstOrDefault(x => x.Id == transaction.Category.Id);
            }
            Account = Accounts.FirstOrDefault(x => x.Id == parent.Account.Id);
            Date = Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(Timestamp);
            Time = Timestamp.TimeOfDay;
            
            ParentTransaction = parent;
        }

        private ITransaction ParentTransaction { get; }


        private IAccount Account
        {
            set {
                if (_account == value) return;
                _account = value;
                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged();
            }
            get => _account;
        }

        public ICategory Category
        {
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged(nameof(Categories));
                OnPropertyChanged();
            }
            get => _category;
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get => _name;
        }

        private DateTime Timestamp
        {
            set
            {
                if (_timestamp == value) return;
                _timestamp = value;
                OnPropertyChanged();
            }
            get => _timestamp;
        }

        private DateTimeOffset Date
        {
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
            }
            get => _date;

        }

        private TimeSpan Time
        {
            set
            {
                if (_time == value) return;
                _time = value;
                OnPropertyChanged();
            }
            get => _time;

        }

        public decimal Total
        {
            set { _total = value; OnPropertyChanged(); }
            get => _total;
        }

        public decimal Weight
        {
            set { _weight = value; OnPropertyChanged();}
            get => _weight;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged();}
            get => _errorString;
        }

        public IEnumerable<ICategory> Categories { get; }


        private IEnumerable<IAccount> Accounts { get; }= MainPage.GlobalSettings.AccountStorage.GetAllAccounts();

        private IEnumerable<ITransaction> Transactions { get; } = MainPage.GlobalSettings.TransactionStorage.GetAllTransactions();
        public BarCode BarCode { get; set; }

        public void CreateChildTransaction()
        {
            ErrorString = String.Empty;
            try
            {
                ErrorString = string.Empty;
                var storage= MainPage.GlobalSettings.TransactionStorage;
                Timestamp = new DateTime(
                    Date.Year,
                    Date.Month,
                    Date.Day,
                    Time.Hours,
                    Time.Minutes,
                    Time.Seconds
                );

                storage.CreateTransaction(Account, Category, Name, Total, Timestamp,0,Weight,null,ParentTransaction);
            }
            catch ( StorageException e )
            {
                 ErrorString = $"You have the exception {e.Message}";
                 throw new ViewModelException(ErrorString);
            }
        }

        public void UpdateChildTransaction()
        {
            ErrorString = String.Empty;
            try
            {
                Timestamp = new DateTime(
                    Date.Year,
                    Date.Month,
                    Date.Day,
                    Time.Hours,
                    Time.Minutes,
                    Time.Seconds
                );
                var storage = MainPage.GlobalSettings.TransactionStorage;
                _transaction.Name = Name;
                _transaction.Account = Account;
                _transaction.Category = Category;
                _transaction.Timestamp = Timestamp;
                _transaction.Total = Total;
                _transaction.Weight = Weight;
                _transaction.Parent = ParentTransaction;

                storage.UpdateTransaction(_transaction);
            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        public void FillFromTemplate(ITransaction selected)
        {
            Category = Categories.FirstOrDefault(x => x.Id == selected.Category?.Id);
            Total = selected.Total;
            Name = selected.Name;
            Weight = selected.Weight;
        }
        public IEnumerable<ITransaction> GetSuggestions(string name)
        {
            return TransactionHelpers.GetSuggestions(Transactions,name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
