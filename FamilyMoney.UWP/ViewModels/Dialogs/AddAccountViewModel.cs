using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Helpers;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class AddAccountViewModel:INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _currency;

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

        public void CreateNewAccount()
        {
            var manager = MainPage.GlobalSettings.AccountManager;
            manager.CreateAccount(Name, Description, Currency);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
