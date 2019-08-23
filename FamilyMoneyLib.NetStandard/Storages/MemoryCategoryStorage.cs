using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryCategoryStorage : CategoryStorageBase, ICategoryStorage
    {
        private readonly List<ICategory> _categories = new List<ICategory>();

        private static long _counter = 0;

        public override ICategory CreateCategory(ICategory category)
        {
            category.Id = ++_counter;
            _categories.Add(category);
            return category;
        }

        public override void DeleteCategory(ICategory category)
        {
            _categories.Remove(category);
        }

        public override void UpdateCategory(ICategory category)
        {

        }

        public override IEnumerable<ICategory> GetAllCategories()
        {
            return _categories.ToArray();
        }

        public MemoryCategoryStorage(ICategoryFactory categoryFactory) : base(categoryFactory)
        {
        }
    }
}
