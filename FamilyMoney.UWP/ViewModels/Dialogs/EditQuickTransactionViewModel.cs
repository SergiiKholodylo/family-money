using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels.Dialogs
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

        public EditQuickTransactionViewModel()
        {
        }

        public EditQuickTransactionViewModel(IQuickTransaction quickTransaction)
        {
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

        public IEnumerable<ICategory> Categories { get; } = MainPage.GlobalSettings.CategoryStorage.GetAllCategories();


        public IEnumerable<IAccount> Accounts { get; } = MainPage.GlobalSettings.AccountStorage.GetAllAccounts();


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public void CreateQuickTransaction()
        {
            try
            {
                var storage = MainPage.GlobalSettings.QuickTransactionStorage;
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
            
            try
            {
                var storage = MainPage.GlobalSettings.QuickTransactionStorage;
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
