﻿using System;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditChildTransaction : ContentDialog
    {
        public EditChildTransactionViewModel ViewModel;
        private Action _editTransactionAction;

        public EditChildTransaction(ITransaction parent, IAccount activeAccount,ITransaction transaction=null)
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
            ViewModel = new EditChildTransactionViewModel(parent, activeAccount,transaction);
        }


        private void InitUpdateMode()
        {
            Title = "Update Transaction".GetLocalized();
            PrimaryButtonText = "Update Transaction".GetLocalized();
            SecondaryButtonText = "Cancel".GetLocalized();

            _editTransactionAction = delegate { ViewModel.UpdateChildTransaction(); };
        }

        private void InitCreateMode()
        {
            
            Title = "Create Transaction".GetLocalized();
            PrimaryButtonText = "Create Transaction".GetLocalized();
            SecondaryButtonText = "Cancel".GetLocalized();

            _editTransactionAction = delegate { ViewModel.CreateChildTransaction(); };
        }



        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                _editTransactionAction();
            }
            catch (ViewModelException)
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selected = args.SelectedItem as ITransaction;
            ViewModel.FillFromTemplate(selected);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            var searchString = sender.Text;
            sender.ItemsSource = ViewModel.GetSuggestions(searchString);
        }
    }
}