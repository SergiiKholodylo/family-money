﻿using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages.Interfaces
{
    public interface ICategoryStorage
    {
        ICategory CreateCategory(ICategory category);
        ICategory CreateCategory(string name, string description, long id, ICategory parentCategory);
        void DeleteAllData();
        void DeleteCategory(ICategory category);
        IEnumerable<ICategory> GetAllCategories();
        IEnumerable<ICategory> MakeFlatCategoryTree();
        void UpdateCategory(ICategory category);
    }
}