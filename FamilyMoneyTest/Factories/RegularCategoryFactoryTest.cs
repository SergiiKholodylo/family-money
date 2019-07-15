﻿using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest.Factories
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


            var category= factory.CreateCategory(categoryName, categoryDescription);


            Assert.AreEqual(categoryName, category.Name);
            Assert.AreEqual(categoryDescription, category.Description);
        }
    }
}
