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
        private readonly ICategoryStorage _storage;
        private readonly ObservableCollection<CategoryTreeItem> _categoryTree = new ObservableCollection<CategoryTreeItem>();


        public ObservableCollection<ICategory> Categories
        {
            private set { _categories = value; OnPropertyChanged();}
            get => _categories;
        }

        public ObservableCollection<CategoryTreeItem> CategoryTree => _categoryTree;

        public CategoryViewModel()
        {
            _storage = MainPage.GlobalSettings.CategoryStorage;
            Refresh();
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            Categories.Clear();
            var allCategories = _storage.GetAllCategories().ToArray();
            _categoryTree.Clear();
            foreach (var category in allCategories)
            {
                if (category.Parent == null)
                {
                    var categoryTreeItem = new CategoryTreeItem
                    {
                        Category = category
                    };
                    _categoryTree.Add(categoryTreeItem);
                    
                    AddChildren(categoryTreeItem, allCategories);
                }
            }
        }

        private void AddChildren(CategoryTreeItem categoryTreeItem, ICategory[] allCategories)
        {
            var children = allCategories.Where(x =>
                x.Parent != null && x.Parent.Id == categoryTreeItem.Category.Id);
            foreach (var child in children)
            {
                var treeChild = new CategoryTreeItem {Category = child};
                categoryTreeItem.Children.Add(treeChild);
                AddChildren(treeChild,allCategories);
            }
        }

        public void DeleteCategory(ICategory activeCategory)
        {
            _storage.DeleteCategory(activeCategory);
            Categories.Remove(activeCategory);
        }
    }

    public class CategoryTreeItem
    {
        public ICategory Category { set; get; }
        public ObservableCollection<CategoryTreeItem> Children { set; get; } = new ObservableCollection<CategoryTreeItem>();

        public override string ToString()
        {
            return Category.Name;
        }
    }
}
