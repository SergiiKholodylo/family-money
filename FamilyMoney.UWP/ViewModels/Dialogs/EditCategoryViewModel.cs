using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditCategoryViewModel: INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private readonly ICategory _category;
        private ICategory _parentCategory;
        private string _errorString;

        public EditCategoryViewModel(ICategory category, ICategory parent)
        {
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

        public IEnumerable<ICategory> Categories { get; } = MainPage.GlobalSettings.CategoryStorage.MakeFlatCategoryTree();

        public void CreateNewCategory()
        {
            try
            {
                var storage = MainPage.GlobalSettings.CategoryStorage;
                storage.CreateCategory(Name, Description, 0, ParentCategory);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during creating category {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }

        public void UpdateCategory()
        {
            try
            {
                var storage = MainPage.GlobalSettings.CategoryStorage;
                _category.Name = Name;
                _category.Description = Description;
                _category.Parent = ParentCategory;
                storage.UpdateCategory(_category);
            }
            catch (StorageException e)
            {
                ErrorString = $"Error during updating category {e.Message}";
                throw new ViewModelException(ErrorString);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
