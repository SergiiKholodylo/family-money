using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class CategoryStorageBase:ICategoryStorage
    {
        protected readonly ICategoryFactory CategoryFactory;

        protected CategoryStorageBase(ICategoryFactory categoryFactory)
        {
            CategoryFactory = categoryFactory;
        }

        public abstract ICategory CreateCategory(ICategory category);
        public ICategory CreateCategory(string name, string description, long id, ICategory parentCategory)
        {
            var category = CategoryFactory.CreateCategory(name, description, id, parentCategory);
            return CreateCategory(category);
        }

        public abstract void DeleteAllData();

        public abstract void DeleteCategory(ICategory category);

        public abstract IEnumerable<ICategory> GetAllCategories();

        public abstract void UpdateCategory(ICategory category);

        public IEnumerable<ICategory> MakeFlatCategoryTree()
        {
            var flatTree = new List<ICategory>();

            ICategory[] getAllCategories = GetAllCategories().ToArray();
            var roots = getAllCategories.Where(x => x.Parent == null);
            foreach (var category in roots)
            {
                flatTree.Add(category);
                AddTreeLeaves(getAllCategories, flatTree, category);
            }

            foreach (var category in flatTree)
            {
                Debug.WriteLine($"{category.Id} : {category.Name} ");
            }
            return flatTree;
        }

        private void AddTreeLeaves(ICategory[] getAllCategories, List<ICategory> flatTree,
            ICategory category)
        {
            var children = getAllCategories.Where(x => x.Parent?.Id == category.Id);
            foreach (var child in children)
            {
                category.AddChild(child);
                flatTree.Add(child);
                AddTreeLeaves(getAllCategories, flatTree, child);
            }

        }
    }
}
