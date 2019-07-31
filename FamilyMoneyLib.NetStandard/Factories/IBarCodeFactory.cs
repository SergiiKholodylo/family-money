using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface IBarCodeFactory
    {
        IBarCode CreateBarCode(string code, bool isWeight = false, int numberOfDigitsForWeight = 0);
    }
}