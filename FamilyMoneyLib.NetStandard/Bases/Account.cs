using System;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{
    [DebuggerDisplay("Id {Id} {Name} ({Currency})")]
    public class Account : IIdBased
    {
        public static long NewAccountId { get; } = -1;

        public long Id { set; get; }
        public DateTime Timestamp { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Currency { set; get; }

        public Account()
        {
            Timestamp = DateTime.Now;
            Id = NewAccountId;
        }

    }
}
