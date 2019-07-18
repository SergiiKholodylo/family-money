using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels.Dialogs;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class AddTransaction : ContentDialog
    {
        public AddTransactionViewModel ViewModel = new AddTransactionViewModel();
        public AddTransaction()
        {
            this.InitializeComponent();
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
