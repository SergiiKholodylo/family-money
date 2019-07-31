using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Storages
{
    [TestClass]
    public class MemoryBarCodeTest
    {
        [TestMethod]
        public void CreateBarCodeTest()
        {
            const string code = "2734336010584";
            const bool isWeight = true;
            const int numberOfDigits = 6;
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());


            var barCode = barCodeStorage.CreateBarCode(CreateBarCode(code, isWeight, numberOfDigits));


            Assert.AreEqual("2734336", barCode.GetProductBarCode());
            Assert.AreEqual(1.0584m, barCode.GetWeightKg());

        }

        private IBarCode CreateBarCode(string code, bool isWeight=false, int numberOfDigits=0)
        {
            var factory = new BarCodeFactory();
            return factory.CreateBarCode(code, isWeight, numberOfDigits);
        }

        [TestMethod]
        public void CreateNonWeightBarCodeTest()
        {
            const string code = "5060207697224";
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());


            var barCode = barCodeStorage.CreateBarCode(CreateBarCode(code));


            Assert.AreEqual(code, barCode.GetProductBarCode());
            Assert.AreEqual(0m, barCode.GetWeightKg());

        }

        [TestMethod]
        public void DeleteBarCodeTest()
        {
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());
            var barCode = barCodeStorage.CreateBarCode(CreateBarCode("2734336010584", true, 6));
            barCodeStorage.CreateBarCode(CreateBarCode("5060207697224"));


            barCodeStorage.DeleteBarCode(barCode);


            var codes = barCodeStorage.GetAllBarCodes();
            Assert.AreEqual(1, codes.Count());

        }

        [TestMethod]
        public void GetBarCodeTransactionTest()
        {
            var transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());

            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);

            var transaction = transactionStorage.CreateTransaction(account, category, "test", 26.38m, DateTime.Now, 0,0,null,null);

            var barCode = barCodeStorage.CreateBarCode(CreateBarCode("2734336010584", true, 6));
            barCode.Transaction = transaction;
            barCodeStorage.UpdateBarCode(barCode);

            barCodeStorage.CreateBarCode(CreateBarCode("5060207697224"));


            ITransaction foundTransaction = barCodeStorage.GetBarCodeTransaction("2734336");

            Assert.AreEqual(26.38m, foundTransaction.Total);
        }
    }
}
