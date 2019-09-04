using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Cached;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;
using FamilyMoneyLib.NetStandard.Storages.SQLite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests.Cached
{
    [TestClass]
    public class CachedBarCodeStorageTest
    {
        private const string SilpoZefir = "2710041003240";
        private const string BarCode2 = "7100241003240";
        private IAccountStorage _accountStorage;
        private ICategoryStorage _categoryStorage;
        private ITransactionStorage _transactionStorage;
        private BarCodeFactory _factory;
        private SqLiteBarCodeStorage _storage;

        [TestInitialize]
        public void Setup()
        {
            _factory = new BarCodeFactory();
            _accountStorage = new CachedAccountStorage(new SqLiteAccountStorage(new RegularAccountFactory()));
            _categoryStorage = new CachedCategoryStorage(new SqLiteCategoryStorage(new RegularCategoryFactory()));
            _transactionStorage =
                new CachedTransactionStorage(new SqLiteTransactionStorage(new RegularTransactionFactory(),
                    _accountStorage, _categoryStorage));
            _storage = new SqLiteBarCodeStorage(
                new BarCodeFactory(), _transactionStorage);
            _storage.DeleteAllData();
        }

    [TestMethod]
        public void CreateBarCodeTest()
        {
            var barCode = CreateBarCode(SilpoZefir,true,6);


            _storage.CreateBarCode(barCode);


            var weight = barCode.GetWeightKg();
            Assert.AreEqual(0.324m, weight);
        }

        [TestMethod]
        public void Create2BarCodesTest()
        {

            var barCode1 = _factory.CreateBarCode(SilpoZefir, true, 6,99L);
            var barCode2 = _factory.CreateBarCode(BarCode2, false, 0,99L);
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
        public void GetAllBarCodeTest()
        {
            var barCode = CreateBarCode(SilpoZefir, true, 6);
            _storage.DeleteAllData();

            _storage.CreateBarCode(barCode);


            var barCodes = _storage.GetAllBarCodes().ToArray();
            Assert.AreEqual(1, barCodes.Count());
            Assert.IsTrue(barCodes.First().IsWeight);
        }

        [TestMethod]
        public void CreateBarCodeBasedTransactionText()
        {
            _storage.DeleteAllData();
            _transactionStorage.DeleteAllData();

            var account = _accountStorage.CreateAccount("Account", "Description", "UAH");
            var category = _categoryStorage.CreateCategory("Category", "category Description", 0, null);
            var transaction = _transactionStorage.CreateTransaction(account, category, "test", 26.38m, DateTime.Now, 0, 0, null, null);
            var barCode = _storage.CreateBarCode(CreateBarCode("2734336010584", true, 6));

            barCode.Transaction = transaction;
            _storage.UpdateBarCode(barCode);
            _storage.CreateBarCode(CreateBarCode("5060207697224"));

            _storage.CreateTransactionBarCodeRelatedFromStorage("2734336");

            var transactions = _transactionStorage.GetAllTransactions();
            Assert.AreEqual(2, transactions.Count());
        }


        private IBarCode CreateBarCode(string code, bool isWeight=false, int numberOfDigits=0)
        {
            var factory = new BarCodeFactory();
            return factory.CreateBarCode(code, isWeight, numberOfDigits);
        }
    }
}
