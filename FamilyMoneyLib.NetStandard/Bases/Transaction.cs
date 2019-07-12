using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Id {Id} {Name}")]
    public class Transaction : ITransactionItem, IIdBased
    {
        public static long NewTransactionId { get; } = -1;

        public DateTime Timestamp { set; get; }
        public Account Account { set; get; }

        public long Id { set; get; }
        public Category Category { set; get; }
        public string Name { set; get; }
        public decimal Total { set; get; }

        private List<ITransactionItem> _complexTransactionItems = new List<ITransactionItem>();

        public Transaction()
        {
            Timestamp = DateTime.Now;
            Id = NewTransactionId;
        }
    }
}
