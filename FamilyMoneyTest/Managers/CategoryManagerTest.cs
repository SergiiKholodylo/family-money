using System.Linq;
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
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
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
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var manager = new CategoryManager(factory, storage);
            var categoryName = "Test Category";
            var categoryDescription = "Test Description";
            var category = manager.CreateCategory(categoryName, categoryDescription);


            var firstAccount = manager.GetAllCategories().First();


            Assert.AreEqual(category.Name, firstAccount.Name);
            Assert.AreEqual(category.Description, firstAccount.Description);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var manager = new CategoryManager(factory, storage);
            var categoryName = "Test Category";
            var categoryDescription = "Test Description";
            var category = manager.CreateCategory(categoryName, categoryDescription);


            manager.DeleteCategory(category);
            var numberOfAccounts = manager.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfAccounts);
        }


        [TestMethod]
        public void UpdateAccountTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var manager = new CategoryManager(factory, storage);

            var categoryName = "Test Category";
            var categoryDescription = "Test Description";

            var category = manager.CreateCategory(categoryName, categoryDescription);
            category.Name = "New Name";
            category.Description = "New Description";


            manager.UpdateCategory(category);


            var firstCategory = manager.GetAllCategories().First();
            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }
    }
}
