using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FamilyMoney.UWP.Bases;
using FamilyMoney.ViewModels.NetStandard.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public class TransactionViewModelBase : INotifyPropertyChanged
    {
        private IAccount _account;
        private ICategory _category;
        private string _name;
        private DateTime _timestamp = DateTime.Now;
        private decimal _total;
        private DateTimeOffset _date;
        private TimeSpan _time;

        private string _errorString;
        private decimal _weight;
        private bool _isComplexTransaction;
        private ObservableCollection<ITransaction> _childrenTransactions;
        private ITransaction _transaction;

        protected readonly Storages _storages;
        //private Visibility _isChildTransactionVisible;

        protected TransactionViewModelBase(Storages storages)
        {
            _storages = storages;
            Accounts = _storages.AccountStorage.GetAllAccounts();
            Categories = _storages.CategoryStorage.MakeFlatCategoryTree();
            Transactions = _storages.TransactionStorage.GetAllTransactions();
            Timestamp = DateTime.Now;
            Date = new DateTimeOffset(Timestamp);
            Time = Timestamp.TimeOfDay;
            
        }


        protected ITransaction ParentTransaction { private get; set; }

        public ObservableCollection<ITransaction> ChildrenTransactions
        {
            get => _childrenTransactions;
            set
            {
                if(_childrenTransactions == value) return;
                _childrenTransactions = value;
                OnPropertyChanged();
            }
        }

        public IBarCode BarCode { set; get; }

        public IAccount Account
        {
            set
            {
                if (_account != null && _account.Equals(value)) return;
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
                if (_category!= null && _category.Equals(value)) return;
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
                //OnPropertyChanged(nameof(IsChildTransactionVisible));
                //OnPropertyChanged(nameof(IsChildTransactionHidden));
            }
            get => _isComplexTransaction;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public IEnumerable<ICategory> Categories { get; }

        public  IEnumerable<IAccount> Accounts { get; }

        public  IEnumerable<ITransaction> Transactions { get; }

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
                var storage = _storages.TransactionStorage;

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
            //BarCode = MainPage.GlobalSettings.ScannedBarCode;
            if ( BarCode == null) return;
            var barCodeStorage = _storages.BarCodeStorage;
            BarCode.AnalyzeCodeByWeightKg(Weight);
            BarCode.Transaction = _transaction;
            barCodeStorage.CreateBarCode(BarCode);
        }

        protected void UpdateTransaction()
        {
            try
            {
                DateTimeFromDateAndTime();
                var storage = _storages.TransactionStorage;
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

        public async Task<string> ScanBarCode(IBarCodeScanner scanner)
        {
            var result = await scanner.ScanBarCode();

            return result;
        }

        public void ProcessScannedBarCode(string barCodeString)
        {
            if (string.IsNullOrWhiteSpace(barCodeString)) return;

            BarCode = new BarCode(barCodeString);
            var storage = _storages.BarCodeStorage;
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

            //MainPage.GlobalSettings.ScannedBarCode = BarCode;
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

            Total = _transaction.Total;
        }

        public bool IsExistingTransaction => _transaction != null;

        public async Task<IBarCode> ScanChildTransaction(IBarCodeScanner scanner)
        {
            var barCodeString = await ScanBarCode(scanner);
            var barCode = new BarCode(barCodeString);
            //var transaction = FindBarCodeAmongExistingTransactions(barCode);

            return barCode;
        }

        public void DeleteChildTransaction(ITransaction childTransaction)
        {
            try
            {
                _storages.TransactionStorage.DeleteChildTransaction(_transaction, childTransaction);
                UpdateChildrenTransactionList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }


        public ITransaction FindBarCodeAmongExistingTransactions(IBarCode barCode)
        {
            var storage = _storages.BarCodeStorage;
            var transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());
            if (transaction != null) return transaction;
            barCode.TryExtractWeight(5);
            transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());
            if (transaction != null) return transaction;
            barCode.TryExtractWeight(6);
            transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());

            return transaction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
