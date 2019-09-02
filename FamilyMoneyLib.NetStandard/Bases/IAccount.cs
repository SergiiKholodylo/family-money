using System;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IAccount:ISecurity,IIdAble
    {
        string Currency { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        DateTime Timestamp { get; set; }
    }
}