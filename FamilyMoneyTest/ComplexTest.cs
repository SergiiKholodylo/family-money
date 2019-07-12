using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.Storages;
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

            var root = new Category{
                Name = "Root",
                Description = "Root category"
                };
            var food = new Category
            {
                Name = "Food",
                Description = "Food except fast-food",
                ParentCategory = root
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
                Name = "Clothes",
                ParentCategory = root
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
                Name = "Cod",
                Total = 20
            };
            var transaction2 = new Transaction
            {
                Name = "Tomato",
                Account = account,
                Category = vegetables,
                Total = 50
            };

            var transaction3 = new Transaction
            {
                Name = "Onion",
                Account = account,
                Category = vegetables,
                Total = 133
            };

            var transaction4 = new Transaction
            {
                Name = "Turtleneck",
                Account = account,
                Category = c00perClothes,
                Total = 2000
            };

            var transactionStorage = new TransactionStorage();
            var categoryStorage = new CategoryStorage();

            categoryStorage.AddCategory(root);
            categoryStorage.AddCategory(food);
            categoryStorage.AddCategory(vegetables);
            categoryStorage.AddCategory(fish);
            categoryStorage.AddCategory(clothes);
            categoryStorage.AddCategory(gerdaClothes);
            categoryStorage.AddCategory(c00perClothes);
            
            transactionStorage.AddTransaction(transaction1);
            transactionStorage.AddTransaction(transaction2);
            transactionStorage.AddTransaction(transaction3);
            transactionStorage.AddTransaction(transaction4);

            var report = new TransactionReport(transactionStorage, categoryStorage);
            var transactionByCategory = report.TransactionByCategory(account);

            Assert.AreEqual(4, transactionByCategory.Count());

        }
    }
}
