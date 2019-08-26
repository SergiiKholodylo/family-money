using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Cached
{
    [TestClass]
    public class CachedCategoryStorageTest
    {
        private ICategoryFactory _factory;
        private ICategoryStorage _storage;
        private ICategory _category;
        private ICategory _childCategory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new RegularCategoryFactory();
            _storage = new SqLiteCategoryStorage(_factory);
            _storage.DeleteAllData();

            {
                var name = "Test Category";
                var description = "Test Description";
                _category = _factory.CreateCategory(name, description, 0, null);
            }

            
            {
                var name = $"Child {_category.Name} Category";
                var description = $"Child {_category.Name} Description";
                _childCategory = _factory.CreateCategory(name, description, 0, _category);

            }
        }

        [TestMethod]
        public void CreateCategoryTest()
        {


            var newCategory = _storage.CreateCategory(_category);


            Assert.AreEqual(_category.Name, newCategory.Name);
            Assert.AreEqual(_category.Description, newCategory.Description);
        }

        [TestMethod]
        public void GetAllCategoriesTest()
        {
            _category.Description = DateTime.Now.ToShortTimeString();
            _storage.CreateCategory(_category);

            var firstCategory = _storage.GetAllCategories().Last();

            Assert.AreEqual(_category.Name, firstCategory.Name);
            Assert.AreEqual(_category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {

            _storage.DeleteCategory(_category);


            var numberOfCategories = _storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfCategories);
        }


        [TestMethod]
        public void UpdateCategoryTest()
        {
            _storage.CreateCategory(_category);
            _category.Name = "New Name";
            _category.Description = "New Description";


            _storage.UpdateCategory(_category);


            var firstCategory = _storage.GetAllCategories().First();
            Assert.AreEqual(_category.Name, firstCategory.Name);
            Assert.AreEqual(_category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void CreateTreeCategoryTest()
        {
            _storage.CreateCategory(_category);
            _storage.CreateCategory(_childCategory);


            var categoryList = _storage.GetAllCategories().ToArray();
            var categoryFromStorage = categoryList.FirstOrDefault(x => x.Id == _category.Id);
            var childCategoryFromStorage = categoryList.FirstOrDefault(x => x.Id == _childCategory.Id);

            Assert.IsNotNull(childCategoryFromStorage);
            Assert.IsNotNull(categoryFromStorage);
            Assert.AreEqual(_category.Id, categoryFromStorage.Id);
            Assert.AreEqual(_childCategory.Id, childCategoryFromStorage.Id);
            Assert.AreEqual(childCategoryFromStorage.Parent.Id, categoryFromStorage.Id);
        }
    }
}
