using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.SQLite
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

        private ICategory CreateCategory()
        {
            var factory = new RegularCategoryFactory();
            var name = "Test Category";
            var description = "Test Description";
            var category = factory.CreateCategory(name, description );

            return category;
        }
    }
}
