using System.Linq;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;
using FamilyMoneyLib.NetStandard.Storages.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ViewModels
{
    [TestClass]
    public class BarCodeViewModelTest
    {
        private ITransactionStorage _transactionStorage;
        private MemoryBarCodeStorage _storage;

        [TestInitialize]
        public void Setup()
        {
            _transactionStorage = new MemoryTransactionStorage(new RegularTransactionFactory());
            _storage = new MemoryBarCodeStorage(new BarCodeFactory(), _transactionStorage);
            _storage.CreateBarCode("55555555555", false, 0);
            _storage.CreateBarCode("44444444444", true, 5);

        }

        [TestMethod]
        public void BarCodeConstructorTest()
        {
            var viewModel = new BarCodeViewModel(_storage);


            Assert.AreEqual(2, viewModel.BarCodes.Count);
        }

        [TestMethod]
        public void DeleteBarCodeTest()
        {
            var viewModel = new BarCodeViewModel(_storage);
            var firstBarCode = viewModel.BarCodes.FirstOrDefault();
            viewModel.DeleteBarCode(firstBarCode);
            var barCodeInStorage = _storage.GetAllBarCodes();

            Assert.AreEqual(1, viewModel.BarCodes.Count);
            Assert.AreEqual(1, barCodeInStorage.Count());

        }
    }
}
