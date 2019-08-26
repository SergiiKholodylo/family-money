using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.CachedStorage
{
    public class CachedCategoryStorage:ICategoryStorage
    {

        private bool _isDirty = true;
        private IEnumerable<ICategory> _cache;
        private readonly ICategoryStorage _storage;

        public CachedCategoryStorage(ICategoryStorage categoryStorage)
        {
            _storage = categoryStorage;
        }

        public ICategory CreateCategory(ICategory category)
        {
            var result = _storage.CreateCategory(category);
            _isDirty = true;
            return result;
        }

        public ICategory CreateCategory(string name, string description, long id, ICategory parentCategory)
        {
            var result = _storage.CreateCategory(name, description, id, parentCategory);
            _isDirty = true;
            return result;
        }

        public void DeleteAllData()
        {
            _storage.DeleteAllData();
            _isDirty = true;
        }

        public void DeleteCategory(ICategory category)
        {
            _storage.DeleteCategory(category);
            _isDirty = true;
        }

        public IEnumerable<ICategory> GetAllCategories()
        {
            if (!_isDirty) return _cache;
            _cache = _storage.GetAllCategories();
            _isDirty = false;
            return _cache;
        }

        public IEnumerable<ICategory> MakeFlatCategoryTree()
        {
            return _storage.MakeFlatCategoryTree();
        }

        public void UpdateCategory(ICategory category)
        {
            _storage.UpdateCategory(category);
            _isDirty = true;
        }
    }
}
