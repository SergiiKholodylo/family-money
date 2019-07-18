using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels
{
    public sealed class CategoryViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ICategory> _categories;
        private readonly ICategoryManager _manager;

        public ObservableCollection<ICategory> Categories
        {
            private set { _categories = value; OnPropertyChanged();}
            get => _categories;
        }

        public CategoryViewModel()
        {
            _manager = MainPage.GlobalSettings.CategoryManager;
            Categories = new ObservableCollection<ICategory>(_manager.GetAllCategories());
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddCategory()
        {
            Categories.Add(_manager.CreateCategory("Category","Description"));
        }

        public void Refresh()
        {
            Categories.Clear();
            var allCategories = _manager.GetAllCategories();
            foreach (var category in allCategories)
            {
                Categories.Add(category);
            }
        }

        public void DeleteCategory(ICategory activeCategory)
        {
            _manager.DeleteCategory(activeCategory);
            Categories.Remove(activeCategory);
        }
    }
}
