﻿using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface ITransactionFactory
    {
        ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total);
    }
}