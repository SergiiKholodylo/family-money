using System;
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

        internal Transaction()
        {
            Timestamp = DateTime.Now;
        }

        internal Transaction(IAccount account, ICategory category, string name, decimal total, DateTime? timestamp, long id=0)
        {
            Account = account;
            Category = category;
            Name = name;
            Total = total;
            Timestamp = timestamp ?? DateTime.Now;
            Id = id;
        }

    }
}
