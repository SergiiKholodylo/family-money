using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs
{
    public sealed class EditQuickTransactionViewModel:INotifyPropertyChanged
    {
        private IAccount _account;
        private ICategory _category;
        private string _name;
        private decimal _total;
        private decimal _weight;
        private string _errorString
            ;

        private bool _askForTotal;
        private bool _askForWeight;
        private readonly IQuickTransaction _quickTransaction;
        private readonly Storages _storages;

        public EditQuickTransactionViewModel(Storages storages)
        {
            _storages = storages;
        }

        public EditQuickTransactionViewModel(Storages storages, IQuickTransaction quickTransaction)
        {
            _storages = storages;
            _quickTransaction = quickTransaction;
            Account = Accounts.FirstOrDefault(x => x.Id == quickTransaction?.Account?.Id);
            Category = Categories.FirstOrDefault(x => x.Id == quickTransaction?.Category?.Id);
            Name = quickTransaction.Name;
            Total = quickTransaction.Total;
            Weight = quickTransaction.Weight;
            AskForWeight = quickTransaction.AskForWeight;
            AskForTotal = quickTransaction.AskForTotal;
        }

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

        public bool AskForTotal
        {
            set
            {
                if(_askForTotal == value) return;
                _askForTotal = value;
                OnPropertyChanged();
            }
            get => _askForTotal;
        }

        public bool AskForWeight
        {
            set
            {
                if(_askForWeight == value) return;
                _askForWeight = value;
                OnPropertyChanged();
            }
            get => _askForWeight;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public IEnumerable<ICategory> Categories  => _storages.CategoryStorage.GetAllCategories();


        public IEnumerable<IAccount> Accounts  => _storages.AccountStorage.GetAllAccounts();


        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void CreateQuickTransaction()
        {
            try
            {
                var storage = _storages.QuickTransactionStorage;
                storage.CreateQuickTransaction(Account, Category, Name, Total, 0, Weight, AskForTotal, AskForWeight);
            }
            catch (StorageException e)
            {
                ErrorString = e.Message;
                throw new ViewModelException(e.Message);
            }
        }

        public void UpdateQuickTransaction()
        {
            if(_quickTransaction == null)
                throw new ViewModelException("There is no Quick Transaction to Update");
            try
            {
                var storage = _storages.QuickTransactionStorage;
                _quickTransaction.Name = Name;
                _quickTransaction.Account = Account;
                _quickTransaction.Category = Category;
                _quickTransaction.AskForTotal = AskForTotal;
                _quickTransaction.AskForWeight = AskForWeight;
                _quickTransaction.Weight = Weight;
                storage.UpdateQuickTransaction(_quickTransaction);
            }
            catch (StorageException e)
            {
                ErrorString = e.Message;
                throw new ViewModelException(e.Message);
            }
        }
    }
}
