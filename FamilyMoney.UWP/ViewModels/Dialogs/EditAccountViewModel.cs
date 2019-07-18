﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditAccountViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _currency;
        private readonly IAccount _account;

        public EditAccountViewModel(IAccount account)
        {
            Name = account.Name;
            Description = account.Description;
            Currency = account.Currency;
            Id = account.Id;
            _account = account;
        }

        public long Id { set; get; }

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

        public void UpdateAccount()
        {
            var manager = MainPage.GlobalSettings.AccountManager;
            _account.Currency = Currency;
            _account.Description = Description;
            _account.Name = Name;
            manager.UpdateAccount(_account);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
