using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditTransactionViewModel:INotifyPropertyChanged
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

        public EditTransactionViewModel(IAccount activeAccount)
        {
            Categories = MakeFlatCategoryTree(MainPage.GlobalSettings.CategoryManager.GetAllCategories().ToArray());
            Date = new DateTimeOffset(DateTime.Now);
            Time = DateTime.Now.TimeOfDay;
            if (activeAccount != null)
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
        }

        public EditTransactionViewModel(ITransaction transaction)
        {
            _transaction = transaction;
            Categories = MakeFlatCategoryTree(MainPage.GlobalSettings.CategoryManager.GetAllCategories().ToArray());
            Date = transaction.Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(transaction.Timestamp);
            Time =  transaction.Timestamp.TimeOfDay;
            Name = transaction.Name;
            Account = Accounts.FirstOrDefault(x=>x.Id == transaction.Account.Id);
            Category = Categories.FirstOrDefault(x=>x.Id == transaction.Category.Id);
            Timestamp = transaction.Timestamp;
            Total = transaction.Total;
            Weight = transaction.Weight;
        }

        public EditTransactionViewModel()
        {
            Categories = MakeFlatCategoryTree(MainPage.GlobalSettings.CategoryManager.GetAllCategories().ToArray());
        }

        private IEnumerable<ICategory> MakeFlatCategoryTree(ICategory[] getAllCategories)
        {
            var flatTree = new List<ICategory>();

            var roots = getAllCategories.Where(x => x.ParentCategory == null);
            foreach (var category in roots)
            {
                flatTree.Add(category);
                AddTreeLeaves(getAllCategories, flatTree, category);
            }

            return flatTree;
        }

        private void AddTreeLeaves(ICategory[] getAllCategories, List<ICategory> flatTree,
            ICategory category)
        {
            var children = getAllCategories.Where(x => x.ParentCategory?.Id == category.Id);
            foreach (var child in children)
            {
                flatTree.Add(child);
                AddTreeLeaves(getAllCategories, flatTree, child);
            }

        }

        public IAccount Account
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


        public IEnumerable<IAccount> Accounts { get; }= MainPage.GlobalSettings.AccountManager.GetAllAccounts();

        public IEnumerable<ITransaction> Transactions { get; } = MainPage.GlobalSettings.TransactionManager.GetAllTransactions();

        public void CreateTransaction()
        {
            try
            {
                ErrorString = string.Empty;
                var manager = MainPage.GlobalSettings.TransactionManager;
                Timestamp = new DateTime(
                    Date.Year,
                    Date.Month,
                    Date.Day,
                    Time.Hours,
                    Time.Minutes,
                    Time.Seconds
                );

                manager.CreateTransaction(Account, Category, Name, Total, Timestamp,0,Weight,null,null);
            }
            catch ( ManagerException e )
            {
                 ErrorString = $"You have the exception {e.Message}";
                 throw new ViewModelException(ErrorString);
            }
        }

        public void UpdateTransaction()
        {
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
                var manager = MainPage.GlobalSettings.TransactionManager;
                _transaction.Name = Name;
                _transaction.Account = Account;
                _transaction.Category = Category;
                _transaction.Timestamp = Timestamp;
                _transaction.Total = Total;
                _transaction.Weight = Weight;

                manager.UpdateTransaction(_transaction);
            }
            catch (ManagerException e)
            {
                ErrorString = $"You have the exception {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        public IEnumerable<ITransaction> GetSuggestions(string name)
        {
            return Transactions.Where(x => x.Name.Contains(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FillFromTemplate(ITransaction selected)
        {
            Category = Categories.FirstOrDefault(x=>x.Id == selected.Category?.Id);
            Total = selected.Total;
            Name = selected.Name;
            Weight = selected.Weight;
            //Product = selected.Product;
        }
    }
}
