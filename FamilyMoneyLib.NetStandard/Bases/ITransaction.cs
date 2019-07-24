using System;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ITransaction:ISecurity
    {
        IAccount Account { get; set; }
        ICategory Category { get; set; }
        string Name { get; set; }
        DateTime Timestamp { get; set; }
        decimal Total { get; set; }
        long Id { get; set; }
        decimal Weight { get; set; }
        IProduct Product { get; set; }
    }
}