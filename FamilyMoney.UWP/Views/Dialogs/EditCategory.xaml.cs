using System;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditCategory : ContentDialog
    {
        public EditCategoryViewModel ViewModel;
        private readonly Action _saveCategoryAction;

        public EditCategory(ICategory category=null)
        {
            this.InitializeComponent();
            if (category == null)
            {
                ViewModel = new EditCategoryViewModel();
                _saveCategoryAction = delegate { ViewModel.CreateNewCategory(); };
                Title = "Create Category".GetLocalized();
                PrimaryButtonText = "Create Category".GetLocalized();
                SecondaryButtonText = "Cancel".GetLocalized();
            }
            else
            {
                ViewModel = new EditCategoryViewModel(category);
                _saveCategoryAction = delegate { ViewModel.UpdateCategory(); };
                Title = "Edit Category".GetLocalized();
                PrimaryButtonText = "Edit Category".GetLocalized();
                SecondaryButtonText = "Cancel".GetLocalized();
            }

            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _saveCategoryAction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
