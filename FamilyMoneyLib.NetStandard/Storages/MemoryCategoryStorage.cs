using System.Collections.Generic;
using System.Linq;
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
            if (IsExists(category))
            {
                DeleteCategory(category);
            }
            else
            {
                if(category.Id == 0)
                    category.Id = ++_counter;
            }
            _categories.Add(category);
            return category;
        }

        public override void DeleteAllData()
        {
            _categories.Clear();
        }

        public override void DeleteCategory(ICategory category)
        {
            var categoryToDelete = _categories.Where(x => x.Id == category.Id).ToArray();
            foreach (var category1 in categoryToDelete)
            {
                _categories.Remove(category1);
            }
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

        private bool IsExists(ICategory category)
        {
            return (category.Id != 0 && _categories.Any(x => x.Id == category.Id));
        }
    }
}
