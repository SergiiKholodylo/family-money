using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryFactory _categoryFactory;
        private readonly ICategoryStorage _categoryStorage;

        public CategoryManager(ICategoryFactory factory, ICategoryStorage storage)
        {
            _categoryStorage = storage;
            _categoryFactory = factory;
        }

        public ICategory CreateCategory(string name, string description)
        {
            var category = _categoryFactory.CreateCategory(name, description);
            return _categoryStorage.CreateCategory(category);
        }

        public void UpdateCategory(ICategory category)
        {
            _categoryStorage.UpdateCategory(category);
        }

        public void DeleteCategory(ICategory category)
        {
            _categoryStorage.DeleteCategory(category);
        }

        public IEnumerable<ICategory> GetAllCategories()
        {
            return _categoryStorage.GetAllCategories();
        }
    }
}
