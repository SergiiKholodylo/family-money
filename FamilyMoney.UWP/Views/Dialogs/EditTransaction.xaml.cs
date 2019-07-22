using System;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditTransaction : ContentDialog
    {
        public EditTransactionViewModel ViewModel;
        private Action _editTransactionAction;

        public EditTransaction(ITransaction transaction = null)
        {
            this.InitializeComponent();

            if (transaction == null)
            {
                InitCreateMode();
            }
            else
            {
                InitUpdateMode();
            }
            ViewModel = new EditTransactionViewModel(transaction);
        }

        private void InitUpdateMode()
        {
            Title = "Update Transaction".GetLocalized();
            PrimaryButtonText = "Update Transaction".GetLocalized();
            SecondaryButtonText = "Cancel".GetLocalized();

            _editTransactionAction = delegate { ViewModel.UpdateTransaction(); };
        }

        private void InitCreateMode()
        {
            
            Title = "Create Transaction".GetLocalized();
            PrimaryButtonText = "Create Transaction".GetLocalized();
            SecondaryButtonText = "Cancel".GetLocalized();

            _editTransactionAction = delegate { ViewModel.CreateTransaction(); };
        }

        public EditTransaction(IAccount activeAccount)
        {
            this.InitializeComponent();
            ViewModel = new EditTransactionViewModel(activeAccount);
            InitCreateMode();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _editTransactionAction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
