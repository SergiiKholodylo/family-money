namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IBarCode:IIdAble
    {
        ITransaction Transaction { get; set; }
        string Code { get; }
        bool IsWeight { get; set; }
        int NumberOfDigitsForWeight { get; }

        void AnalyzeCodeByWeightKg(decimal weightInKg);
        string GetProductBarCode();
        decimal GetWeightKg();
        void TryExtractWeight(int barCodeLength);
    }
}