using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface ICategoryFactory
    {
        ICategory CreateCategory(string name, string description, long id , ICategory parentCategory);
    }
}