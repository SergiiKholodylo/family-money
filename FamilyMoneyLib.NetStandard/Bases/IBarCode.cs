﻿namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IBarCode:IIdAble
    {
        ITransaction Transaction { get; set; }
        long Id { get; set; }
        string Code { get; }
        bool IsWeight { get; }
        int NumberOfDigitsForWeight { get; }

        void AnalyzeCodeByWeightKg(decimal weightInKg);
        string GetProductBarCode();
        decimal GetWeightKg();
        void TryExtractWeight(int barCodeLength);
    }
}