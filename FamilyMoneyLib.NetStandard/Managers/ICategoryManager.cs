using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public interface ICategoryManager
    {
        ICategory CreateCategory(string name, string description, long id, ICategory parentCategory);
        void DeleteCategory(ICategory category);
        IEnumerable<ICategory> GetAllCategories();
        void UpdateCategory(ICategory category);
    }
}