using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface ICategoryStorage
    {
        ICategory CreateCategory(ICategory category);
        void DeleteCategory(ICategory category);
        IEnumerable<ICategory> GetAllCategories();
        void UpdateCategory(ICategory category);
    }
}