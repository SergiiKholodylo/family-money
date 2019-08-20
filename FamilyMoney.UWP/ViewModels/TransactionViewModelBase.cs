using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Bases;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionViewModelBase : INotifyPropertyChanged
    {
        protected IAccount _account;
        protected ICategory _category;
        protected string _name;
        protected DateTime _timestamp = DateTime.Now;
        protected decimal _total;
        protected DateTimeOffset _date;
        protected TimeSpan _time;

        protected string _errorString;
        protected decimal _weight;
        protected bool _isComplexTransaction;
        protected ObservableCollection<ITransaction> _childrenTransactions;
        protected ITransaction _transaction;
        private Visibility _isChildTransactionVisible;

        protected TransactionViewModelBase()
        {
            
            Categories = MainPage.GlobalSettings.CategoryStorage.MakeFlatCategoryTree();
            Timestamp = DateTime.Now;
            Date = new DateTimeOffset(Timestamp);
            Time = Timestamp.TimeOfDay;
        }


        protected ITransaction ParentTransaction { private get; set; }

        public ObservableCollection<ITransaction> ChildrenTransactions
        {
            get => _childrenTransactions;
            set => _childrenTransactions = value;
        }

        public IBarCode BarCode { set; get; }

        public IAccount Account
        {
            set
            {
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

        protected DateTime Timestamp
        {
            set
            {
                if (_timestamp == value) return;
                _timestamp = value;
                OnPropertyChanged();
            }
            private get => _timestamp;
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
            set { _weight = value; OnPropertyChanged(); }
            get => _weight;
        }

        public bool IsComplexTransaction
        {
            set
            {
                if(_isComplexTransaction ==  value) return;
                _isComplexTransaction = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsChildTransactionVisible));
                OnPropertyChanged(nameof(IsChildTransactionHidden));
            }
            get => _isComplexTransaction;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public Visibility IsChildTransactionVisible => IsComplexTransaction?Visibility.Visible:Visibility.Collapsed;
        public Visibility IsChildTransactionHidden => !IsComplexTransaction ? Visibility.Visible : Visibility.Collapsed;

        public IEnumerable<ICategory> Categories { get; }


        public IEnumerable<IAccount> Accounts { get; } = MainPage.GlobalSettings.AccountStorage.GetAllAccounts();

        public IEnumerable<ITransaction> Transactions { get; } = MainPage.GlobalSettings.TransactionStorage.GetAllTransactions();

        public ITransaction Transaction
        {
            get => _transaction;
            set
            {
                if (value == _transaction) return;
                _transaction = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsExistingTransaction));

            }
        }

        protected void CreateTransaction()
        {
            try
            {
                ErrorString = string.Empty;
                var storage = MainPage.GlobalSettings.TransactionStorage;

                DateTimeFromDateAndTime();

                Transaction = storage.CreateTransaction(Account, Category, Name, Total, Timestamp, 0, Weight, null, ParentTransaction);

                CreateBarCodeWithTransaction();
            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        private void CreateBarCodeWithTransaction()
        {
            BarCode = MainPage.GlobalSettings.ScannedBarCode;
            if ( BarCode == null) return;
            var barCodeStorage = MainPage.GlobalSettings.BarCodeStorage;
            BarCode.AnalyzeCodeByWeightKg(Weight);
            BarCode.Transaction = _transaction;
            barCodeStorage.CreateBarCode(BarCode);
        }

        protected void UpdateTransaction()
        {
            try
            {
                DateTimeFromDateAndTime();
                var storage = MainPage.GlobalSettings.TransactionStorage;
                _transaction.Name = Name;
                _transaction.Account = Account;
                _transaction.Category = Category;
                _transaction.Timestamp = Timestamp;
                _transaction.Total = Total;
                _transaction.Weight = Weight;
                _transaction.Parent = ParentTransaction;
                storage.UpdateTransaction(_transaction);

                CreateBarCodeWithTransaction();

            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        private void DateTimeFromDateAndTime()
        {
            Timestamp = new DateTime(
                                Date.Year,
                                Date.Month,
                                Date.Day,
                                Time.Hours,
                                Time.Minutes,
                                Time.Seconds
                            );
        }
        public IEnumerable<ITransaction> GetSuggestions(string name)
        {
            return TransactionHelpers.GetSuggestions(Transactions,name);
        }

        public void FillFromTemplate(ITransaction selected)
        {
            Category = Categories.FirstOrDefault(x => x.Id == selected.Category?.Id);
            Total = selected.Total;
            Name = selected.Name;
            Weight = selected.Weight;
        }

        public async Task<string> ScanBarCode()
        {
            var scanner = new BarCodeScanner();

            var result = await scanner.ScanBarCode();

            return result;
        }

        public void ProcessScannedBarCode(string barCodeString)
        {
            if (string.IsNullOrWhiteSpace(barCodeString)) return;

            BarCode = new BarCode(barCodeString);
            var storage = MainPage.GlobalSettings.BarCodeStorage;
            var transaction = storage.GetBarCodeTransaction(BarCode.GetProductBarCode());
            if (transaction == null)
            {
                BarCode.TryExtractWeight(5);
                transaction = storage.GetBarCodeTransaction(BarCode.GetProductBarCode());
                if (transaction == null)
                {
                    BarCode.TryExtractWeight(6);
                    transaction = storage.GetBarCodeTransaction(BarCode.GetProductBarCode());
                }
            }

            MainPage.GlobalSettings.ScannedBarCode = BarCode;
            Weight = BarCode.GetWeightKg();
            Total = 0;
            if (transaction == null) return;
            Category = Categories.FirstOrDefault(x => x.Id == transaction.Category?.Id);
            Name = transaction.Name;
            if (!BarCode.IsWeight)
                Total = transaction.Total;
        }

        public void UpdateChildrenTransactionList()
        {
            if(_transaction == null)return;
            ChildrenTransactions.Clear();
            var newChildren = _transaction.Children.Select(x => (ITransaction) x);
            foreach (var transaction in newChildren)
            {
                ChildrenTransactions.Add(transaction);
            }
        }

        public bool IsExistingTransaction => _transaction != null;

        public async Task<EditChildTransaction> ScanChildTransaction()
        {
            var barCodeString = await ScanBarCode();

            var editTransaction = new EditChildTransaction(Transaction, Account);


            var barCode = new BarCode(barCodeString);

            var storage = MainPage.GlobalSettings.BarCodeStorage;
            var transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());
            if (transaction == null)
            {
                barCode.TryExtractWeight(5);
                transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());
                if (transaction == null)
                {
                    barCode.TryExtractWeight(6);
                    transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());
                }
            }

            editTransaction.ViewModel.BarCode = barCode;
            editTransaction.ViewModel.Weight = barCode.GetWeightKg();
            editTransaction.ViewModel.Total = 0;
            if (transaction == null) return editTransaction;
            editTransaction.ViewModel.Category = editTransaction.ViewModel.Categories.FirstOrDefault(x => x.Id == transaction.Category?.Id);
            editTransaction.ViewModel.Name = transaction.Name;
            if (!barCode.IsWeight)
                editTransaction.ViewModel.Total = transaction.Total;
            return editTransaction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
