using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels.Dialogs
{
    public sealed class EditCategoryViewModel: INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private readonly ICategory _category;
        private ICategory _parentCategory;
        private string _errorString;
        private readonly ICategoryStorage _categoryStorage;

        public EditCategoryViewModel(ICategoryStorage categoryStorage, ICategory category, ICategory parent)
        {
            _categoryStorage = categoryStorage;
            Categories = _categoryStorage.MakeFlatCategoryTree();
            if (category != null)
            {
                _category = category;
                Name = category.Name;
                Description = category.Description;
                if (category.Parent != null)
                    ParentCategory = Categories.FirstOrDefault(x => x.Id == category.Parent.Id);
            }
            else
            {
                if(parent != null)
                    ParentCategory = Categories.FirstOrDefault(x => x.Id == parent.Id);
            }
            
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

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public readonly IEnumerable<ICategory> Categories;

        public void CreateNewCategory()
        {
            try
            {
                _categoryStorage.CreateCategory(Name, Description, 0, ParentCategory);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during creating category {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        public void UpdateCategory()
        {
            if(_category == null)
                throw new ViewModelException("There is no Category to update");
            try
            {
                _category.Name = Name;
                _category.Description = Description;
                _category.Parent = ParentCategory;
                _categoryStorage.UpdateCategory(_category);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during updating category {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
