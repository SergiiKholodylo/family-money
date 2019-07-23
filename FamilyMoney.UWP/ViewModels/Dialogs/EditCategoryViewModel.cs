using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private ICategory _parentCategory;

        public EditCategoryViewModel()
        {

        }

        public EditCategoryViewModel(ICategory category)
        {
            _category = category;
            Name = category.Name;
            Description = category.Description;
            if(category.ParentCategory!= null)
                ParentCategory = Categories.FirstOrDefault(x=>x.Id == category.ParentCategory.Id);
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

        public ICategory ParentCategory
        {
            set
            {
                if(_parentCategory == value) return;
                _parentCategory = value;
                OnPropertyChanged();
            }
            get => _parentCategory;
        }

        public IEnumerable<ICategory> Categories { get; } = MainPage.GlobalSettings.CategoryManager.GetAllCategories();

        public void CreateNewCategory()
        {
            var manager = MainPage.GlobalSettings.CategoryManager;
            manager.CreateCategory(Name, Description, 0, ParentCategory);
        }

        public void UpdateCategory()
        {
            var manager = MainPage.GlobalSettings.CategoryManager;
            _category.Name = Name;
            _category.Description = Description;
            _category.ParentCategory = ParentCategory;
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
