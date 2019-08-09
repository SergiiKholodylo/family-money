using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Factories
{
    [TestClass]
    public class RegularCategoryFactoryTest
    {
        [TestMethod]
        public void TestCreateCategory()
        {
            var factory = new RegularCategoryFactory();
            var categoryName = "Category Name";
            var categoryDescription = "Category Descriptor";


            var category= factory.CreateCategory(categoryName, categoryDescription,0,null);


            Assert.AreEqual(categoryName, category.Name);
            Assert.AreEqual(categoryDescription, category.Description);
        }
    }
}
