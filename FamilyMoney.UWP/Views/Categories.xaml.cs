using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Categories : Page
    {
        public readonly CategoryViewModel ViewModel = new CategoryViewModel();

        public Categories()
        {
            this.InitializeComponent();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var addAccount = new EditCategory();

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

        private async void ListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            var a = ((Microsoft.UI.Xaml.Controls.TreeView) sender).SelectedNodes.ToArray();
            var b = a.FirstOrDefault();
            //var activeCategory = (ICategory)(((TreeView)sender).SelectedNodes);
            //var editAccount = new EditCategory(activeCategory);
            //var result = await editAccount.ShowAsync();
            //if (result == ContentDialogResult.Primary)
            //    ViewModel.Refresh();
        }

        private async void DeleteItem_ItemInvoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            //var activeCategory = (ICategory)args.SwipeControl.DataContext;
            //var deleteConfirmation = new ContentDialog
            //{
            //    Title = "Delete Category",
            //    PrimaryButtonText = "Delete",
            //    SecondaryButtonText = "Cancel",
            //    DefaultButton = ContentDialogButton.Primary,
            //    Content = $"Do you want delete category \n '{activeCategory.Name}'?"
            //};

            //var result = await deleteConfirmation.ShowAsync();

            //if (result == ContentDialogResult.Primary)
            //    ViewModel.DeleteCategory(activeCategory);
        }


        private void TreeView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            var a = ((Microsoft.UI.Xaml.Controls.TreeView)sender).SelectedNodes.ToArray();
            var b = a.FirstOrDefault();
            //var activeCategory = (ICategory)(((TreeView)sender).SelectedNodes);

        }

        private void TreeView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void TreeViewItem_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {

        }

        private async void TreeView_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args)
        {
            var category = ((FamilyMoney.UWP.ViewModels.CategoryTreeItem) args.InvokedItem).Category;

            var editAccount = new EditCategory(category);
            var result = await editAccount.ShowAsync();
            if (result == ContentDialogResult.Primary)
                ViewModel.Refresh();
        }
    }
}
