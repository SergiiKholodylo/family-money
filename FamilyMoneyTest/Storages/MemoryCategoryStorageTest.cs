﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var storage = new MemoryCategoryStorage();
            var category = CreateCategory();


            var newCategory = storage.CreateCategory(category);


            Assert.AreEqual(category.Name, newCategory.Name);
            Assert.AreEqual(category.Description, newCategory.Description);
        }

        [TestMethod]
        public void GetAllAccountsTest()
        {
            var storage = new MemoryCategoryStorage();
            var category = CreateCategory();
            storage.CreateCategory(category);

            var firstCategory= storage.GetAllCategories().First();

            Assert.AreEqual(category.Name, firstCategory.Name);
            Assert.AreEqual(category.Description, firstCategory.Description);
        }

        [TestMethod]
        public void DeleteAccountTest()
        {
            var storage = new MemoryCategoryStorage();
            var category = CreateCategory();
            storage.CreateCategory(category);

            var numberOfCategoriesAfterCreate = storage.GetAllCategories().Count();
            storage.DeleteCategory(category);


            var numberOfCategories = storage.GetAllCategories().Count();


            Assert.AreEqual(0, numberOfCategories);
            Assert.AreEqual(1, numberOfCategoriesAfterCreate);
        }


        [TestMethod]
        public void UpdateAccountClass()
        {
            var storage = new MemoryCategoryStorage();
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

        private ICategory CreateCategory()
        {
            var factory = new RegularCategoryFactory();
            var categoryName = "Category Name";
            var categoryDescription = "Category Description";


            var category = factory.CreateCategory(categoryName, categoryDescription);

            return category;
        }
    }
}