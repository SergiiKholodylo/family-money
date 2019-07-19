﻿using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class AddTransaction : ContentDialog
    {
        public AddTransactionViewModel ViewModel;

        public AddTransaction()
        {
            this.InitializeComponent();
            ViewModel = new AddTransactionViewModel(null);
        }

        public AddTransaction(IAccount activeAccount)
        {
            this.InitializeComponent();
            ViewModel = new AddTransactionViewModel(activeAccount);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.CreateTransaction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
