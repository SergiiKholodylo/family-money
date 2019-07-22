using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface ICategoryStorage
    {
        ICategory CreateCategory(ICategory category);
        void DeleteCategory(ICategory category);
        IEnumerable<ICategory> GetAllCategories(ICategoryFactory factory);
        void UpdateCategory(ICategory category);
    }
}