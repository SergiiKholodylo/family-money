﻿using System;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Transaction {Name} Total {Total}")]
    public class Transaction : ITransaction
    {
        public long Id { set; get; }
        public DateTime Timestamp { set; get; }
        public IAccount Account { set; get; }
        public ICategory Category { set; get; }
        public string Name { set; get; }
        public decimal Total { set; get; }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }
        public decimal Weight { set; get; }
        public IProduct Product { set; get; }

        public bool IsComplexTransaction { set; get; }
        public ITransaction ParenTransaction { set; get; }

        internal Transaction()
        {
            Timestamp = DateTime.Now;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        internal Transaction(IAccount account, ICategory category, string name, decimal total, DateTime? timestamp, long id=0, decimal weight=0, IProduct product=null)
        {
            Account = account;
            Category = category;
            Name = name;
            Total = total;
            Timestamp = timestamp ?? DateTime.Now;
            Id = id;
            Weight = weight;
            Product = product;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

    }
}
