﻿
using System;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularTransactionFactory : ITransactionFactory
    {
        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total)
        {
            return new Transaction(account, category, name, total, null);
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp)
        {
            return new Transaction(account, category, name, total, timestamp);
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp, long id , decimal weight , IProduct product, ITransaction parentTransaction)
        {
            var transaction = new Transaction(account, category, name, total, timestamp, id, weight, product, parentTransaction);
            parentTransaction?.AddChildTransaction(transaction);
            return transaction;

        }
    }
}
