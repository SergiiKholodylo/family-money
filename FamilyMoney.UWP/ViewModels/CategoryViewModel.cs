using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels
{
    public sealed class CategoryViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ICategory> _categories = new ObservableCollection<ICategory>();
        private readonly ICategoryManager _manager;
        private readonly ObservableCollection<CategoryTreeItem> _categoryTree = new ObservableCollection<CategoryTreeItem>();


        public ObservableCollection<ICategory> Categories
        {
            private set { _categories = value; OnPropertyChanged();}
            get => _categories;
        }

        public ObservableCollection<CategoryTreeItem> CategoryTree => _categoryTree;

        public CategoryViewModel()
        {
            _manager = MainPage.GlobalSettings.CategoryManager;
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
            var allCategories = _manager.GetAllCategories().ToArray();
            _categoryTree.Clear();
            foreach (var category in allCategories)
            {
                if (category.ParentCategory == null)
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
                x.ParentCategory != null && x.ParentCategory.Id == categoryTreeItem.Category.Id);
            foreach (var child in children)
            {
                var treeChild = new CategoryTreeItem {Category = child};
                categoryTreeItem.Children.Add(treeChild);
                AddChildren(treeChild,allCategories);
            }
        }

        public void DeleteCategory(ICategory activeCategory)
        {
            _manager.DeleteCategory(activeCategory);
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
