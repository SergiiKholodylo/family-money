using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Storages
{
    [TestClass]
    public class MemoryCategoryStorageTest
    {
        [TestMethod]
        public void CreateAccountTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var category = CreateCategory();


            var newCategory = storage.CreateCategory(category);


            Assert.AreEqual(category.Name, newCategory.Name);
            Assert.AreEqual(category.Description, newCategory.Description);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var category = CreateCategory();
            storage.CreateCategory(category);

            var firstCategory= storage.GetAllCategories().First();

            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var category = CreateCategory();
            storage.CreateCategory(category);

            var numberOfCategoriesAfterCreate = storage.GetAllCategories().Count();
            storage.DeleteCategory(category);


            var numberOfCategories = storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfCategories);
            Assert.AreEqual(1, numberOfCategoriesAfterCreate);
        }


        [TestMethod]
        public void UpdateAccountTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var category = CreateCategory();
            storage.CreateCategory(category);
            category.Name = "New Name";
            category.Description = "New Description";


            storage.UpdateCategory(category);


            var firstCategory
                = storage.GetAllCategories().First();
            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void CreateTreeCategoryTest()
        {
            var factory = new RegularCategoryFactory();
            var storage = new MemoryCategoryStorage(factory);
            var category = CreateCategory();
            storage.CreateCategory(category);
            var childCategory = CreateChildCategory(category);
            storage.CreateCategory(childCategory);


            var categoryList = storage.GetAllCategories().ToArray();
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
