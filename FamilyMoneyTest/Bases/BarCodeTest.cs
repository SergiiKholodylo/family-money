using FamilyMoneyLib.NetStandard.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Bases
{
    [TestClass]
    public class BarCodeTest
    {
        private const string SilpoZefir = "2710041003248";

        [TestMethod]
        public void GetWeightTest()
        {
            var barcode = new BarCode(SilpoZefir, true, 6);


            var weight = barcode.GetWeightKg();


            Assert.AreEqual(0.325m,weight);
        }


        [TestMethod]
        public void GetProductBarCodeTest()
        {
            var barcode = new BarCode(SilpoZefir, true, 6);


            var productBarCode = barcode.GetProductBarCode();


            Assert.AreEqual("2710041",productBarCode);
        }

        [TestMethod]
        public void AnalyzeCodeByWeightKgTest()
        {
            var barcode = new BarCode(SilpoZefir);


            barcode.AnalyzeCodeByWeightKg(0.325m);


            Assert.AreEqual(true,barcode.IsWeight);
            Assert.AreEqual(6,barcode.NumberOfDigitsForWeight);
        }
    }
}
