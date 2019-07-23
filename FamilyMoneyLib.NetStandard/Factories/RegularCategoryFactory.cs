using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularCategoryFactory : ICategoryFactory
    {

        public ICategory CreateCategory(string name, string description, long id, ICategory parentCategory)
        {
            return new Category(name, description, id, parentCategory);
        }
    }
}
