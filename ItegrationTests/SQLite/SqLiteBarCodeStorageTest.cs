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
            var barCode = CreateBarCode();


            storage.CreateBarCode(barCode);


            var weight = barCode.GetWeightKg();
            Assert.AreEqual(0.324m, weight);
        }


        [TestMethod]
        public void GetAllBarCodeTest()
        {
            var storage = new SqLiteBarCodeStorage(
                new BarCodeFactory(), new SqLiteTransactionStorage(new RegularTransactionFactory()));
            var barCode = CreateBarCode();
            storage.DeleteAllData();

            storage.CreateBarCode(barCode);


            var barCodes = storage.GetAllBarCodes();
            Assert.AreEqual(1, barCodes.Count());
        }

        private IBarCode CreateBarCode()
        {
            var barcode = new BarCode(SilpoZefir, true, 6);
            return barcode;
        }
    }
}
