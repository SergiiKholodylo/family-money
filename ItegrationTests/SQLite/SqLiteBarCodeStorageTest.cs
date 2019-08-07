using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.SQLite
{
    [TestClass]
    public class SqLiteBarCodeStorageTest
    {
        private const string SilpoZefir = "2710041003240";

        [TestMethod]
        public void CreateBarCodeTest()
        {
            var storage = new SqLiteBarCodeStorage(
                new BarCodeFactory(), new SqLiteTransactionStorage(new RegularTransactionFactory()));
            var barCode = CreateBarCode(SilpoZefir,true,6);


            storage.CreateBarCode(barCode);


            var weight = barCode.GetWeightKg();
            Assert.AreEqual(0.324m, weight);
        }


        [TestMethod]
        public void GetAllBarCodeTest()
        {
            var storage = new SqLiteBarCodeStorage(
                new BarCodeFactory(), new SqLiteTransactionStorage(new RegularTransactionFactory()));
            var barCode = CreateBarCode(SilpoZefir, true, 6);
            storage.DeleteAllData();

            storage.CreateBarCode(barCode);


            var barCodes = storage.GetAllBarCodes().ToArray();
            Assert.AreEqual(1, barCodes.Count());
            Assert.IsTrue(barCodes.First().IsWeight);
        }

        [TestMethod]
        public void CreateBarCodeBasedTransactionText()
        {
            var accountStorage = new SqLiteAccountStorage(new RegularAccountFactory());
            var categoryStorage = new SqLiteCategoryStorage(new RegularCategoryFactory());

            var transactionStorage = new SqLiteTransactionStorage(new RegularTransactionFactory(),accountStorage,categoryStorage);
            var barCodeStorage = new SqLiteBarCodeStorage(new BarCodeFactory(), transactionStorage);
            barCodeStorage.DeleteAllData();
            transactionStorage.DeleteAllData();

            var account = accountStorage.CreateAccount("Account", "Description", "UAH");
            var category = categoryStorage.CreateCategory("Category", "category Description", 0, null);
            var transaction = transactionStorage.CreateTransaction(account, category, "test", 26.38m, DateTime.Now, 0, 0, null, null);
            var barCode = barCodeStorage.CreateBarCode(CreateBarCode("2734336010584", true, 6));

            barCode.Transaction = transaction;
            barCodeStorage.UpdateBarCode(barCode);
            barCodeStorage.CreateBarCode(CreateBarCode("5060207697224"));

            barCodeStorage.CreateBarCodeBasedTransaction("2734336");

            var transactions = transactionStorage.GetAllTransactions();
            Assert.AreEqual(2, transactions.Count());
        }


        private IBarCode CreateBarCode(string code, bool isWeight=false, int numberOfDigits=0)
        {
            var factory = new BarCodeFactory();
            return factory.CreateBarCode(code, isWeight, numberOfDigits);
        }
    }
}
