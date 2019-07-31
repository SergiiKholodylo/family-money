using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class BarCodeFactory : IBarCodeFactory
    {
        public IBarCode CreateBarCode(string code, bool isWeight = false, int numberOfDigitsForWeight = 0)
        {
            return new BarCode(code, isWeight, numberOfDigitsForWeight);
        }
    }
}
