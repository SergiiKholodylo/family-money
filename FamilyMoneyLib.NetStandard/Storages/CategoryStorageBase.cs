using System.Collections.Generic;
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

        public abstract void DeleteCategory(ICategory category);
        public abstract IEnumerable<ICategory> GetAllCategories();
        public abstract void UpdateCategory(ICategory category);
    }
}
