using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryCategoryStorage : CategoryStorageBase
    {
        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();

        public MemoryCategoryStorage(ICategoryFactory categoryFactory) : base(categoryFactory)
        {
        }


        public override ICategory CreateCategory(ICategory category)
        {
            return _storageEngine.Create(category) as ICategory;
        }

        public override void DeleteAllData()
        {
            _storageEngine.DeleteAllData();
        }

        public override void DeleteCategory(ICategory category)
        {
            _storageEngine.Delete(category);
        }

        public override void UpdateCategory(ICategory category)
        {
            
        }

        public override IEnumerable<ICategory> GetAllCategories()
        {
            return _storageEngine.GetAll().Cast<ICategory>();
        }
    }
}
