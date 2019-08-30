using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Storages
{
    [TestClass]
    public class MemoryCategoryStorageTest
    {
        private RegularCategoryFactory _factory;
        private MemoryCategoryStorage _storage;

        [TestInitialize]
        public void Setup()
        {
            _factory = new RegularCategoryFactory();
            _storage = new MemoryCategoryStorage(_factory);
        }


        [TestMethod]
        public void CreateCategoryTest()
        {

            var category = CreateCategory();


            var newCategory = _storage.CreateCategory(category);


            Assert.AreEqual(category.Name, newCategory.Name);
            Assert.AreEqual(category.Description, newCategory.Description);
        }


        [TestMethod]
        public void CreateCategoryWithCodeTest()
        {

            var category = _factory.CreateCategory("Category", "Description", 5, null);


            var newCategory = _storage.CreateCategory(category);


            Assert.AreEqual(5, newCategory.Id);
        }

        [TestMethod]
        public void CreateTwoCategoriesWithSameCodeTest()
        {

            var category = _factory.CreateCategory("Category", "Description", 5, null);
            var category2 = _factory.CreateCategory("Updated Category", "Updated Description", 5, null);


            _storage.CreateCategory(category);
            _storage.CreateCategory(category2);
            var categoriesList = _storage.GetAllCategories().ToArray();
            var categoryFromStorage = categoriesList.FirstOrDefault();

            Assert.IsNotNull(categoryFromStorage);
            Assert.IsNotNull(categoriesList);
            Assert.AreEqual(1,categoriesList.Count());
            Assert.AreEqual(5, categoryFromStorage.Id);
            Assert.AreEqual("Updated Category",categoryFromStorage.Name);
        }

        [TestMethod]
        public void GetAllCategoriesTest()
        {

            var category = CreateCategory();
            _storage.CreateCategory(category);

            var firstCategory= _storage.GetAllCategories().First();

            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {

            var category = CreateCategory();
            _storage.CreateCategory(category);

            var numberOfCategoriesAfterCreate = _storage.GetAllCategories().Count();
            _storage.DeleteCategory(category);


            var numberOfCategories = _storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfCategories);
            Assert.AreEqual(1, numberOfCategoriesAfterCreate);
        }


        [TestMethod]
        public void UpdateCategoryTest()
        {

            var category = CreateCategory();
            _storage.CreateCategory(category);
            category.Name = "New Name";
            category.Description = "New Description";


            _storage.UpdateCategory(category);


            var firstCategory
                = _storage.GetAllCategories().First();
            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void CreateTreeCategoryTest()
        {
            var category = CreateCategory();
            _storage.CreateCategory(category);
            var childCategory = CreateChildCategory(category);
            _storage.CreateCategory(childCategory);


            var categoryList = _storage.GetAllCategories().ToArray();
            var categoryFromStorage = categoryList.FirstOrDefault(x => x.Id == category.Id);
            var childCategoryFromStorage = categoryList.FirstOrDefault(x => x.Id == childCategory.Id);


            Assert.AreEqual(category.Id, categoryFromStorage.Id);
            Assert.AreEqual(childCategory.Id, childCategoryFromStorage.Id);
            Assert.AreEqual(childCategoryFromStorage.Parent.Id, categoryFromStorage.Id);
        }

        private ICategory CreateCategory()
        {
            var factory = new RegularCategoryFactory();
            var name = "Test Category";
            var description = "Test Description";
            var category = factory.CreateCategory(name, description, 0, null);

            return category;
        }

        private ICategory CreateChildCategory(ICategory parent)
        {
            var factory = new RegularCategoryFactory();
            var name = $"Child {parent.Name} Category";
            var description = $"Child {parent.Name} Description";
            var category = factory.CreateCategory(name, description, 0, parent);

            return category;
        }
    }
}
