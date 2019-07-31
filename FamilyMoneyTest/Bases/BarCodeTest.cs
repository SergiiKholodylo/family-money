using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Bases
{
    [TestClass]
    public class BarCodeTest
    {
        private const string SilpoZefir = "2710041003240";

        [TestMethod]
        public void GetWeightTest()
        {
            var barcode = new BarCode(SilpoZefir, true, 6);


            var weight = barcode.GetWeightKg();


            Assert.AreEqual(0.324m,weight);
        }


        [TestMethod]
        public void GetProductBarCodeTest()
        {
            var barcode = new BarCode(SilpoZefir, true, 6);


            var productBarCode = barcode.GetProductBarCode();


            Assert.AreEqual("2710041",productBarCode);
        }
    }
}
