﻿using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryCategoryStorage : ICategoryStorage
    {
        private readonly List<ICategory> _categories = new List<ICategory>();

        public ICategory CreateCategory(ICategory category)
        {
            _categories.Add(category);
            return category;
        }

        public void DeleteCategory(ICategory category)
        {
            _categories.Remove(category);
        }

        public void UpdateCategory(ICategory category)
        {

        }

        public IEnumerable<ICategory> GetAllCategories(ICategoryFactory factory)
        {
            return _categories.ToArray();
        }
    }
}
