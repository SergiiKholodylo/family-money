using System;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public class BarCode : IBarCode
    {
        private const int BarCodeWeightFactor = 10000;

        public string Code { get; private set; }

        public  bool IsWeight { get; private set; }

        public  int NumberOfDigitsForWeight { get; private set; }

        public BarCode(string code, bool isWeight = false, int numberOfDigitsForWeight = 0, long id = 0)
        {
            if (code.Length < numberOfDigitsForWeight) throw new ArgumentException();
            Id = id;
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
            return Math.Round(weightKg,3);
        }

        public long Id { get; set; }

        public string GetProductBarCode()
        {
            if (!IsWeight) return Code;
            var productBarCodeLength = Code.Length - NumberOfDigitsForWeight;
            var productBarCode = Code.Substring(0, productBarCodeLength);
            return productBarCode;
        }

        public virtual void AnalyzeCodeByWeightKg(decimal weightInKg)
        {
            IsWeight = false;
            if(weightInKg <= 0) return;
            if (Code == null || Code.Length < 5) return;
            if(weightInKg > 99.99m) return;


            for (var i = 6; i > 4; i--)
            for (var j = 2; j > 0; j--)
            {
                Debug.WriteLine($"{i} {j}");
                var weightStr = LastNDigits(i);
                var weight = IntegerPartMSymbols(weightStr, j);
                if (weight != weightInKg) continue;
                IsWeight = true;
                NumberOfDigitsForWeight = i;
                return;
            }
        }

        private string LastNDigits(int n)
        {
            return Code.Length < n ? string.Empty : Code.Substring(Code.Length - n);
        }

        private decimal IntegerPartMSymbols(string withNumberNoPoint, int m)
        {
            if (withNumberNoPoint.Length < m+1) return 0;
            var integerPart = withNumberNoPoint.Substring(0, m);
            var rest = withNumberNoPoint.Substring(m);
            return Math.Round(Convert.ToDecimal(integerPart + "." + rest),3);
        }


        public ITransaction Transaction { set; get; }

        public void TryExtractWeight(int barCodeLength)
        {
            if(string.IsNullOrWhiteSpace(Code)) return;
            if(Code.Length < barCodeLength) return;
            IsWeight = true;
            NumberOfDigitsForWeight = barCodeLength;
        }
    }
}
