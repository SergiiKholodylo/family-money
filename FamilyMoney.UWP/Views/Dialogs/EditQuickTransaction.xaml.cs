using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditQuickTransaction : ContentDialog
    {
        public readonly EditQuickTransactionViewModel ViewModel;
        private readonly Action _saveQuickTransactionAction;
        public EditQuickTransaction(IQuickTransaction quickTransaction = null)
        {
            this.InitializeComponent();
            if (quickTransaction == null)
            {
                ViewModel = new EditQuickTransactionViewModel(MainPage.GlobalSettings.Storages);
                _saveQuickTransactionAction = delegate { ViewModel.CreateQuickTransaction(); };
                Title = "Create Quick Transaction";
                PrimaryButtonText = "Create";
                SecondaryButtonText = "Cancel";
            }
            else
            {
                ViewModel = new EditQuickTransactionViewModel(MainPage.GlobalSettings.Storages, quickTransaction);
                _saveQuickTransactionAction = delegate { ViewModel.UpdateQuickTransaction(); };
                Title = "Edit Quick Transaction";
                PrimaryButtonText = "Save";
                SecondaryButtonText = "Cancel";
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                _saveQuickTransactionAction();
            }
            catch (ViewModelException)
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
