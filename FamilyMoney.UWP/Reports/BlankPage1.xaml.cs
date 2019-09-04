﻿using System;
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
using FamilyMoney.UWP.Views;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Reports
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public readonly Report1ViewModel ViewModel;
        public BlankPage1()
        {
            this.InitializeComponent();
            var accountStorage = MainPage.GlobalSettings.Storages.AccountStorage;
            var categoryStorage = MainPage.GlobalSettings.Storages.CategoryStorage;
            var transactionStorage = MainPage.GlobalSettings.Storages.TransactionStorage;
            var model = new Report1ViewModel(accountStorage, categoryStorage, transactionStorage);
            model.Execute();
            ViewModel = model;
        }

        private void BtTransactionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Transactions));
        }

        private void BtQuickTransactionButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void BtSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Views.Settings.Settings));
        }

        private void BtReportsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Report));
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Execute();
        }

        private void ReportPeriod_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Execute();
        }
    }
}
