using System;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public class BarCode : IBarCode
    {
        private const int BarCodeWeightFactor = 10000;

        private readonly string _code;

        private readonly bool _isWeight;

        private readonly int _numberOfDigitsForWeight;

        public BarCode(string code, bool isWeight = false, int numberOfDigitsForWeight = 0)
        {
            if (code.Length < numberOfDigitsForWeight) throw new ArgumentException();

            _code = code;
            _isWeight = isWeight;
            _numberOfDigitsForWeight = numberOfDigitsForWeight;
        }
        public decimal GetWeightKg()
        {
            if (!_isWeight) return 0;
            if (_code.Length < _numberOfDigitsForWeight) return 0;
            var lastNSymbols = _code.Substring(_code.Length - _numberOfDigitsForWeight);
            var weightKg = Convert.ToDecimal(lastNSymbols) / BarCodeWeightFactor;
            return weightKg;
        }

        public string GetProductBarCode()
        {
            if (!_isWeight) return _code;
            var productBarCodeLength = _code.Length - _numberOfDigitsForWeight;
            var productBarCode = _code.Substring(0, productBarCodeLength);
            return productBarCode;
        }
    }
}
