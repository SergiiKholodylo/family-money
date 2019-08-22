using System;
using Windows.UI.Xaml.Controls;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views.Dialogs
{
    public sealed partial class EditCategory : ContentDialog
    {
        public EditCategoryViewModel ViewModel;
        private readonly Action _saveCategoryAction;

        public EditCategory(ICategory category=null, ICategory parent=null)
        {
            this.InitializeComponent();
            if (category == null)
            {
                ViewModel = new EditCategoryViewModel(MainPage.GlobalSettings.Storages.CategoryStorage,category,parent);
                _saveCategoryAction = delegate { ViewModel.CreateNewCategory(); };
                Title = "Create Category";
                PrimaryButtonText = "Create Category";
                SecondaryButtonText = "Cancel";
            }
            else
            {
                ViewModel = new EditCategoryViewModel(MainPage.GlobalSettings.Storages.CategoryStorage, category, category);
                _saveCategoryAction = delegate { ViewModel.UpdateCategory(); };
                Title = "Edit Category";
                PrimaryButtonText = "Edit Category";
                SecondaryButtonText = "Cancel";
            }

            
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                _saveCategoryAction();
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
