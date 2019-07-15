﻿
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularTransactionFactory : ITransactionFactory
    {
        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total)
        {
            return new Transaction(account, category, name, total, null);
        }
    }
}
