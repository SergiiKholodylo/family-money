﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Accounts : Page
    {
        public AccountViewModel ViewModel { set; get; } = new AccountViewModel();

        public Accounts()
        {
            this.InitializeComponent();

        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var addAccount = new AddAccount();

            var result = await addAccount.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.Refresh();
            }

        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Transactions));
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Accounts));
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Categories));
        }

        private async void DeleteItem_ItemInvoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            var activeAccount = (IAccount)args.SwipeControl.DataContext;
            var deleteConfirmation = new ContentDialog
            {
                Title = "Delete Account",
                PrimaryButtonText = "Delete Account",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = $"Do you want delete account \n '{activeAccount.Name}({activeAccount.Currency})'?"
            };

            var result = await deleteConfirmation.ShowAsync();

            if(result == ContentDialogResult.Primary)
                ViewModel.DeleteAccount(activeAccount);
        }

        private async void ListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            var activeAccount = (IAccount) (((ListView) sender).SelectedValue);
            var editAccount = new EditAccount(activeAccount);
            var result = await editAccount.ShowAsync();
            if(result == ContentDialogResult.Primary)
                ViewModel.Refresh();
        }
    }
}
