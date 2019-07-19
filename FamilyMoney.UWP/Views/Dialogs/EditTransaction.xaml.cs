using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditTransaction : ContentDialog
    {
        public EditTransactionViewModel ViewModel;
        public EditTransaction(ITransaction transaction)
        {
            this.InitializeComponent();
            ViewModel = new EditTransactionViewModel(transaction);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.UpdateTransaction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
