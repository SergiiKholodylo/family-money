using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditCategoryViewModel: INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private readonly ICategory _category;

        public EditCategoryViewModel()
        {

        }

        public EditCategoryViewModel(ICategory category)
        {
            _category = category;
            Name = category.Name;
            Description = category.Description;
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get => _name;
        }

        public string Description
        {
            set { _description = value; OnPropertyChanged(); }
            get => _description;
        }

        public void CreateNewCategory()
        {
            var manager = MainPage.GlobalSettings.CategoryManager;
            manager.CreateCategory(Name, Description);
        }

        public void UpdateCategory()
        {
            var manager = MainPage.GlobalSettings.CategoryManager;
            _category.Name = Name;
            _category.Description = Description;
            manager.UpdateCategory(_category);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
