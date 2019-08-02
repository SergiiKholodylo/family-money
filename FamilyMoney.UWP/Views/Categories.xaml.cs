using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using FamilyMoney.UWP.ViewModels;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.AddOn;
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
            var addAccount = new EditCategory(null,ViewModel.Category);

            var result = await addAccount.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.RefreshCategoryList();
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

        private void ListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            var a = ((Microsoft.UI.Xaml.Controls.TreeView) sender).SelectedNodes.ToArray();
            var b = a.FirstOrDefault();
            //var activeCategory = (ICategory)(((TreeView)sender).SelectedNodes);
            //var editAccount = new EditCategory(activeCategory);
            //var result = await editAccount.ShowAsync();
            //if (result == ContentDialogResult.Primary)
            //    ViewModel.Refresh();
        }

        private void DeleteItem_ItemInvoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
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

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var selected = (ICategory)((Windows.UI.Xaml.FrameworkElement) e.OriginalSource).DataContext;
            //if (!selected.HasChild) return;
            ViewModel.Category = selected;
            ViewModel.RefreshCategoryList();

        }


        private void BtTopLevel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OneLevelUp();
        }

        private async void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ICategory activeCategory = (ICategory)((FrameworkElement)e.OriginalSource).DataContext;
            var editAccount = new EditCategory(activeCategory);
            var result = await editAccount.ShowAsync();
            if (result == ContentDialogResult.Primary)
                ViewModel.RefreshCategoryList();
        }

        private void CreateDefaultCategories_OnClick(object sender, RoutedEventArgs e)
        {
            CreateCategoryTree.CreateDefaultCategoryTree();
        }
    }
}
