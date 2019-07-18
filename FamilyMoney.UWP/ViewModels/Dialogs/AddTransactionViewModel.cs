using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class AddTransactionViewModel:INotifyPropertyChanged
    {
        private  IAccount _account;
        private  ICategory _category;
        private  string _name;
        private  DateTime _timestamp = DateTime.Now;
        private  decimal _total;
        private DateTimeOffset _date;
        private TimeSpan _time;

        public AddTransactionViewModel()
        {
            Date = new DateTimeOffset(DateTime.Now);
            Time =  DateTime.Now.TimeOfDay;
        }

        public IAccount Account
        {
            set {
                if (_account == value) return;
                _account = value;
                OnPropertyChanged();
            }
            get => _account;
        }

        public ICategory Category
        {
            set
            {
                if (_category == value) return;
                _category = value; OnPropertyChanged();
            }
            get => _category;
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get => _name;
        }

        public DateTime Timestamp
        {
            set
            {
                if (_timestamp == value) return;
                _timestamp = value;
                OnPropertyChanged();
            }
            get => _timestamp;
        }

        public DateTimeOffset Date
        {
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
            }
            get => _date;

        }

        public TimeSpan Time
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

        public IEnumerable<ICategory> Categories
        {
            get
            {
                var manager = MainPage.GlobalSettings.CategoryManager;
                return manager.GetAllCategories();
            }
        }


        public IEnumerable<IAccount> Accounts
        {
            get
            {
                var manager = MainPage.GlobalSettings.AccountManager;
                return manager.GetAllAccounts();
            }
        }

        public void CreateTransaction()
        {
            var manager = MainPage.GlobalSettings.TransactionManager;
            Timestamp = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Time.Hours,
                Time.Minutes,
                Time.Seconds
                );

            manager.CreateTransaction(Account, Category, Name, Total, Timestamp);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
