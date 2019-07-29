using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItegrationTests.SQLite
{
    [TestClass]
    public class SqLiteCategoryStorageTest
    {
        [TestMethod]
        public void CreateCategoryTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new SqLiteCategoryStorage(factory);
            var category = CreateCategory();


            var newCategory = storage.CreateCategory(category);


            Assert.AreEqual(category.Name, newCategory.Name);
            Assert.AreEqual(category.Description, newCategory.Description);
        }

        [TestMethod]
        public void GetAllCategoriesTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new SqLiteCategoryStorage(factory);
            var category = CreateCategory();
            category.Description = DateTime.Now.ToShortTimeString();
            storage.CreateCategory(category);

            var firstCategory = storage.GetAllCategories().Last();

            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void DeleteCategoryTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new SqLiteCategoryStorage(factory);
            storage.DeleteAllData();
            var category = CreateCategory();
            storage.CreateCategory(category);
            storage.DeleteCategory(category);


            var numberOfCategories = storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfCategories);
        }


        [TestMethod]
        public void UpdateCategoryTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new SqLiteCategoryStorage(factory);
            storage.DeleteAllData();
            var category = CreateCategory();
            storage.CreateCategory(category);
            category.Name = "New Name";
            category.Description = "New Description";


            storage.UpdateCategory(category);


            var firstCategory = storage.GetAllCategories().First();
            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void CreateTreeCategoryTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new SqLiteCategoryStorage(factory);
            storage.DeleteAllData();
            var category = CreateCategory();
            storage.CreateCategory(category);
            var childCategory = CreateChildCategory(category);
            storage.CreateCategory(childCategory);


            var categoryList = storage.GetAllCategories().ToArray();
            var categoryFromStorage = categoryList.FirstOrDefault(x => x.Id == category.Id);
            var childCategoryFromStorage = categoryList.FirstOrDefault(x => x.Id == childCategory.Id);


            Assert.AreEqual(category.Id, categoryFromStorage.Id);
            Assert.AreEqual(childCategory.Id, childCategoryFromStorage.Id);
            Assert.AreEqual(childCategoryFromStorage.ParentCategory.Id, categoryFromStorage.Id);
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
