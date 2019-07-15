using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularCategoryFactory : ICategoryFactory
    {
        public ICategory CreateCategory(string name, string description)
        {
            return new Category(name, description);
        }
    }
}
