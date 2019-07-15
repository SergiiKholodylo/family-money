using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Managers
{
    [TestClass]
    public class CategoryManagerTest
    {
        [TestMethod]
        public void CreateCategoryTest()
        {
            var storage = new MemoryCategoryStorage();
            var factory = new RegularCategoryFactory();
            var manager = new CategoryManager(factory, storage);


            var categoryName = "Test Category";
            var categoryDescription = "Test Description";

            var category = manager.CreateCategory(categoryName, categoryDescription);


            Assert.AreEqual(category.Name, categoryName);
            Assert.AreEqual(category.Description, categoryDescription);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var storage = new MemoryCategoryStorage();
            var factory = new RegularCategoryFactory();
            var manager = new CategoryManager(factory, storage);
            var categoryName = "Test Category";
            var categoryDescription = "Test Description";

            var category = manager.CreateCategory(categoryName, categoryDescription);

            var firstAccount = storage.GetAllCategories().First();

            Assert.AreEqual(category.Name, firstAccount.Name);
            Assert.AreEqual(category.Description, firstAccount.Description);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var storage = new MemoryCategoryStorage();
            var factory = new RegularCategoryFactory();
            var manager = new CategoryManager(factory, storage);

            var categoryName = "Test Category";
            var categoryDescription = "Test Description";

            var category = manager.CreateCategory(categoryName, categoryDescription);
            storage.DeleteCategory(category);


            var numberOfAccounts = storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfAccounts);
        }


        [TestMethod]
        public void UpdateAccountClass()
        {
            var storage = new MemoryCategoryStorage();
            var factory = new RegularCategoryFactory();
            var manager = new CategoryManager(factory, storage);

            var categoryName = "Test Category";
            var categoryDescription = "Test Description";

            var category = manager.CreateCategory(categoryName, categoryDescription);
            category.Name = "New Name";
            category.Description = "New Description";


            storage.UpdateCategory(category);


            var firstCategory = storage.GetAllCategories().First();
            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }
    }
}
