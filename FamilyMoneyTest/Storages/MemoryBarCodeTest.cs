using System.Linq;
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


            var barCode = barCodeStorage.CreateBarCode(code, isWeight, numberOfDigits);


            Assert.AreEqual("2734336", barCode.GetProductBarCode());
            Assert.AreEqual(1.0584m, barCode.GetWeightKg());

        }

        [TestMethod]
        public void CreateNonWeightBarCodeTest()
        {
            const string code = "5060207697224";
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());


            var barCode = barCodeStorage.CreateBarCode(code);


            Assert.AreEqual(code, barCode.GetProductBarCode());
            Assert.AreEqual(0m, barCode.GetWeightKg());

        }

        [TestMethod]
        public void DeleteBarCodeTest()
        {
            var barCodeStorage = new MemoryBarCodeStorage(new BarCodeFactory());
            var barCode = barCodeStorage.CreateBarCode("2734336010584", true, 6);
            barCodeStorage.CreateBarCode("5060207697224");


            barCodeStorage.DeleteBarCode(barCode);


            var codes = barCodeStorage.GetAllBarCodes();
            Assert.AreEqual(1, codes.Count());

        }
    }
}
