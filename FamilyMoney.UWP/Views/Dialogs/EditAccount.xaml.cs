using System;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditAccount : ContentDialog
    {
        public EditAccountViewModel ViewModel;
        private readonly Action _saveAccountAction;
        public EditAccount(IAccount account = null)
        {
            this.InitializeComponent();
            if (account == null)
            {
                ViewModel = new EditAccountViewModel();
                _saveAccountAction = delegate { ViewModel.CreateNewAccount(); };
                Title = "Create Account".GetLocalized();
                PrimaryButtonText = "Create Account".GetLocalized();
                SecondaryButtonText = "Cancel".GetLocalized();
            }
            else
            {
                ViewModel = new EditAccountViewModel(account);
                _saveAccountAction = delegate { ViewModel.UpdateAccount(); };
                Title = "Edit Account".GetLocalized();
                PrimaryButtonText = "Edit Account".GetLocalized();
                SecondaryButtonText = "Cancel".GetLocalized();

            }

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _saveAccountAction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
