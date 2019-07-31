namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IBarCode
    {
        ITransaction Transaction { get; set; }
        long Id { get; set; }
        string Code { get; }
        bool IsWeight { get; }
        int NumberOfDigitsForWeight { get; }

        string GetProductBarCode();
        decimal GetWeightKg();
    }
}