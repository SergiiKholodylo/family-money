using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest
{
    [TestClass]
    public class ComplexTest
    {
        [TestMethod]
        public void TestTransactions()
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


            var account = new Account
                {
                    Name = "Main Account",
                    Currency = "UAH"
                };

            var transaction = new Transaction
            {
                Account = account,
                Category = fish,
                Total = 20
            };
            var transactionStorage = new TransactionStorage();

            transactionStorage.AddTransaction(transaction);

            var allTransactions = transactionStorage.GetAllTransactions();

            Assert.AreEqual(1, allTransactions.Count());

        }

        [TestMethod]
        public void TestTransactionReport()
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


            var account = new Account
            {
                Name = "Main Account",
                Currency = "UAH"
            };

            var transaction1 = new Transaction
            {
                Account = account,
                Category = fish,
                Total = 20
            };
            var transaction2 = new Transaction
            {
                Account = account,
                Category = vegetables,
                Total = 50
            };

            var transaction3 = new Transaction
            {
                Account = account,
                Category = vegetables,
                Total = 133
            };

            var transaction4 = new Transaction
            {
                Account = account,
                Category = c00perClothes,
                Total = 2000
            };

            var transactionStorage = new TransactionStorage();

            transactionStorage.AddTransaction(transaction1);
            transactionStorage.AddTransaction(transaction2);
            transactionStorage.AddTransaction(transaction3);
            transactionStorage.AddTransaction(transaction4);

            var report = new TransactionReport(transactionStorage);
            var transactionByCategory = report.TransactionByCategory(account);

            Assert.AreEqual(4, transactionByCategory.Count());

        }
    }
}
