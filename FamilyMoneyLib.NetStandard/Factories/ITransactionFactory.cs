using System;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface ITransactionFactory
    {
        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total);
        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp);
        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp, long id, decimal weight, IProduct product);
    }
}