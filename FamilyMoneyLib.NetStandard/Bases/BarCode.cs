using System;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public class BarCode : IBarCode
    {
        private const int BarCodeWeightFactor = 10000;

        public string Code { get; private set; }

        public  bool IsWeight { get; private set; }

        public  int NumberOfDigitsForWeight { get; private set; }

        public BarCode(string code, bool isWeight = false, int numberOfDigitsForWeight = 0)
        {
            if (code.Length < numberOfDigitsForWeight) throw new ArgumentException();

            Code = code;
            IsWeight = isWeight;
            NumberOfDigitsForWeight = numberOfDigitsForWeight;
        }
        public decimal GetWeightKg()
        {
            if (!IsWeight) return 0;
            if (Code.Length < NumberOfDigitsForWeight) return 0;
            var lastNSymbols = Code.Substring(Code.Length - NumberOfDigitsForWeight);
            var weightKg = Convert.ToDecimal(lastNSymbols) / BarCodeWeightFactor;
            return weightKg;
        }

        public long Id { get; set; }

        public string GetProductBarCode()
        {
            if (!IsWeight) return Code;
            var productBarCodeLength = Code.Length - NumberOfDigitsForWeight;
            var productBarCode = Code.Substring(0, productBarCodeLength);
            return productBarCode;
        }

        public ITransaction Transaction { set; get; }
    }
}
