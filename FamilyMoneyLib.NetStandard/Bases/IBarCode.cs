namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IBarCode
    {
        string GetProductBarCode();
        decimal GetWeightKg();
    }
}