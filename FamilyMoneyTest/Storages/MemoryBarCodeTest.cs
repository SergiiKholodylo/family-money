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
        private IBarCodeFactory _factory;
        private MemoryBarCodeStorage _storage;
        private MemoryTransactionStorage _transactionStorage;
        private const string SilpoZefir = "2710041003240";
        private const string BarCode2 = "7100241003240";

        [TestInitialize]
        public void Setup()
        {
            _factory = new BarCodeFactory();
            _transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());
            _storage = new MemoryBarCodeStorage(_factory,_transactionStorage);
        }

        [TestMethod]
        public void CreateBarCodeTest()
        {
            const string code = "2734336010584";
            const bool isWeight = true;
            const int numberOfDigits = 6;
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(),new MemoryTransactionStorage(new RegularTransactionFactory()));


            var barCode = barCodeStorage.CreateBarCode(CreateBarCode(code, isWeight, numberOfDigits));


            Assert.AreEqual("2734336", barCode.GetProductBarCode());
            Assert.AreEqual(1.058m, barCode.GetWeightKg());

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
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(), new MemoryTransactionStorage(new RegularTransactionFactory()));


            var barCode = barCodeStorage.CreateBarCode(CreateBarCode(code));


            Assert.AreEqual(code, barCode.GetProductBarCode());
            Assert.AreEqual(0m, barCode.GetWeightKg());

        }

        [TestMethod]
        public void CreateTwoBarCodesWithSameCodeTest()
        {

            var barCode1 = _factory.CreateBarCode(SilpoZefir, true, 6, 99L);
            var barCode2 = _factory.CreateBarCode(BarCode2, false, 0, 99L);
            _storage.CreateBarCode(barCode1);
            _storage.CreateBarCode(barCode2);

            var newBarCode = _storage.GetAllBarCodes().FirstOrDefault();

            Assert.IsNotNull(newBarCode);
            Assert.AreEqual(1, _storage.GetAllBarCodes().Count());
            Assert.AreEqual(barCode2.Code, newBarCode.Code);
            Assert.AreEqual(barCode2.IsWeight, newBarCode.IsWeight);
            Assert.AreEqual(99L, newBarCode.Id);
        }

        [TestMethod]
        public void DeleteBarCodeTest()
        {
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(), new MemoryTransactionStorage(new RegularTransactionFactory()));
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
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(), new MemoryTransactionStorage(new RegularTransactionFactory()));

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

        [TestMethod]
        public void CreateBarCodeBasedTransactionText()
        {
            var transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory(), transactionStorage);
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account = accountFactory.CreateAccount("Account", "Description", "UAH");
            var category = categoryFactory.CreateCategory("Category", "category Description", 0, null);
            var transaction = transactionStorage.CreateTransaction(account, category, "test", 26.38m, DateTime.Now, 0, 0, null, null);
            var barCode = barCodeStorage.CreateBarCode(CreateBarCode("2734336010584", true, 6));
            barCode.Transaction = transaction;
            barCodeStorage.UpdateBarCode(barCode);
            barCodeStorage.CreateBarCode(CreateBarCode("5060207697224"));

            barCodeStorage.CreateTransactionBarCodeRelatedFromStorage("2734336");

            var transactions = transactionStorage.GetAllTransactions();
            Assert.AreEqual(2,transactions.Count());
        }
    }
}
