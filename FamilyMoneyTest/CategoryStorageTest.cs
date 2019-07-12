using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest
{
    [TestClass]
    public class CategoryStorageTest
    {
        [TestMethod]
        public void TransactionAddTransactionTest()
        {
            var category = new Category
            {
                Name = "category 1",
                Description = "Description for category 1"
            };
            var storage = new CategoryStorage();


            storage.AddCategory(category);


            Assert.AreNotEqual(Category.NewCategoryId, category.Id, "Category Number mustn't be equal New Category Id");
        }

        [TestMethod]
        public void CategoryAddMultiplyCategoriesTest()
        {
            var category1 = new Category
            {
                Name = "category 1",
                Description = "Description for category 1"
            };
            var category2 = new Category
            {
                Name = "category 2",
                Description = "Description for category 2"
            };
            var category3 = new Category
            {
                Name = "category 3",
                Description = "Description for category 3"
            };
            var storage = new CategoryStorage();


            storage.AddCategory(category1);
            storage.AddCategory(category2);
            storage.AddCategory(category3);
            var categories = storage.GetAllCategories();


            Assert.IsTrue(
                category1.Id != category2.Id &&
                category2.Id != category3.Id && 
                category1.Id != category3.Id);
            Assert.AreEqual(3, categories.Count());
        }

        [TestMethod]
        public void CategoryAddAccountAndGetAccountTest()
        {
            var category = new Category
            {
                Name = "category 1",
                Description = "Description for category 1"
            };
            var storage = new CategoryStorage();
            var categoryId = storage.AddCategory(category);


            var storedCategory = storage.GetCategory(categoryId);


            Assert.AreEqual(categoryId, storedCategory.Id, "Category Ids must be equal");
            Assert.AreEqual(category.Name, storedCategory.Name, "Category names must be the same");
            Assert.AreEqual(category.Description, storedCategory.Description, "Category descriptions must be the same");
        }

        [TestMethod]
        public void CategoryUpdateAccountAndGetAccountTest()
        {
            var category = new Category
            {
                Name = "category 1",
                Description = "Description for category 1"
            };
            var storage = new CategoryStorage();
            storage.AddCategory(category);
            var newCategory = new Category
            {
                Id = category.Id,
                Name = "category updated",
                Description = "Description for category updated"
            };


            storage.UpdateCategory(newCategory);
            var storedCategory = storage.GetCategory(category.Id);


            Assert.AreEqual(newCategory.Id, storedCategory.Id, "Category's Ids must be equal");
            Assert.AreEqual(newCategory.Name, storedCategory.Name, "Category's Names must be equal");
            Assert.AreEqual(newCategory.Description, storedCategory.Description, "Category's Descriptions must be equal");
        }

        [TestMethod]
        public void CategoryDeleteCategoryTest()
        {
            var category = new Category
            {
                Name = "category 1",
                Description = "Description for category 1"
            };
            var storage = new CategoryStorage();
            storage.AddCategory(category);


            storage.DeleteCategory(category.Id);
            var storedCategory = storage.GetCategory(category.Id);


            Assert.AreEqual(Category.NewCategoryId, storedCategory.Id, "Category must be Empty");
        }

        [TestMethod]
        public void CategoryDeleteNonexistentCategoryTest()
        {
            const long anyCategoryNumber = 567L;
            var storage = new CategoryStorage();


            storage.DeleteCategory(anyCategoryNumber);


            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void GetSubcategoriesTest()
        {
            var storage = GetCategoryStorage();
            var categories = storage.GetAllCategories();
            var food = categories.FirstOrDefault(x => x.Name.Equals("Food"));


            var foodSubcategories = storage.GetSubcategories(food);


            Assert.AreEqual(2, foodSubcategories.Count());
        }


        private CategoryStorage GetCategoryStorage()
        {
            var storage = new CategoryStorage();

            var food = new Category
            {
                Name = "Food",
                Description = "Food except fast-food"
            };
            var vegetables = new Category
            {
                Name = "Vegetables",
                Description = "Vegetables",
                ParentCategory = food
            };
            var tomato = new Category
            {
                Name = "Tomato",
                Description = "Vegetables",
                ParentCategory = vegetables
            };
            var fish = new Category
            {
                Name = "Fish",
                Description = "My favorite category!",
                ParentCategory = food
            };
            var clothes = new Category
            {
                Name = "Clothes"
            };
            var gerdaClothes = new Category
            {
                Name = "Gerda Clothes",
                ParentCategory = clothes
            };
            var c00perClothes = new Category
            {
                Name = "C00per Clothes",
                ParentCategory = clothes
            };

            storage.AddCategory(food);
            storage.AddCategory(vegetables);
            storage.AddCategory(tomato);
            storage.AddCategory(fish);
            storage.AddCategory(clothes);
            storage.AddCategory(gerdaClothes);
            storage.AddCategory(c00perClothes);


            return storage;
        }

    }
}
