using System;

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

        public virtual void AnalyzeCodeByWeightKg(decimal weightInKg)
        {
            IsWeight = false;
            if(weightInKg <= 0) return;
            if (Code == null || Code.Length < 5) return;
            if(weightInKg > 99.99m) return;


            var weightString = weightInKg.ToString("N3").Replace(".", "").Replace(",", "");
            var isCodeContainWeight = Code.Contains(weightString);
            if(!isCodeContainWeight) return;
            var index = Code.IndexOf(weightString, StringComparison.Ordinal);
            var weightInCode = Code.Substring(index);

            IsWeight = true;
            if(Code.Substring(index-1,1).Equals("0"))
                NumberOfDigitsForWeight = weightInCode.Length+1;
            else
                NumberOfDigitsForWeight = weightInCode.Length;
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
