using FamilyMoneyLib.NetStandard.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest
{
    [TestClass]
    public class CategoryTest
    {
        [TestMethod]
        public void TestHasCategoryAsParent()
        {
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

            var foodAsParentForTomato = tomato.HasCategoryAsParent(food);
            var foodAsParentForGerdaClothes = gerdaClothes.HasCategoryAsParent(food);

            Assert.IsTrue(foodAsParentForTomato);
            Assert.IsFalse(foodAsParentForGerdaClothes);
        }

        [TestMethod]
        public void GetCategoryLevelTest()
        {
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


            var tomatoLevel = tomato.GetCategoryLevel();
            var gerdaClothesLevel = gerdaClothes.GetCategoryLevel();


            Assert.AreEqual(3,tomatoLevel);
            Assert.AreEqual(2, gerdaClothesLevel);
        }
    }
}
