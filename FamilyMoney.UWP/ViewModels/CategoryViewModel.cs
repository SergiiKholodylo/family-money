using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;


namespace FamilyMoney.UWP.ViewModels
{
    public sealed class CategoryViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ICategory> _categories = new ObservableCollection<ICategory>();
        private IEnumerable<ICategory> _allCategories;
        private readonly ICategoryStorage _storage;
        private ICategory _category;


        public ObservableCollection<ICategory> Categories
        {
            private set { _categories = value; OnPropertyChanged();}
            get => _categories;
        }

        public CategoryViewModel()
        {
            _storage = MainPage.GlobalSettings.CategoryStorage;
            RefreshCategoryList();
        }


        public ICategory Category
        {
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged();
            }
            get => _category;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshCategoryList()
        {
            Categories.Clear();
            _allCategories = _storage.MakeFlatCategoryTree();

            var parent = Category?.Id;

            var newLevel = new ObservableCollection<ICategory>(_allCategories.Where(x => x?.Parent?.Id == parent));
            
            foreach (var category in newLevel)
            {
                Categories.Add(category);
            }

        }



        public void DeleteCategory(ICategory activeCategory)
        {
            _storage.DeleteCategory(activeCategory);
            Categories.Remove(activeCategory);
        }

        public void OneLevelUp()
        {
            if(Category==null)return;
            Category = (ICategory)Category.Parent;
            RefreshCategoryList();
        }
    }
}
