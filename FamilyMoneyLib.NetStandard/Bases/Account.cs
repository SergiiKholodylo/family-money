using System;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{
    [DebuggerDisplay("Account {Name} ({Currency})")]
    public class Account : IAccount,ISecurity
    {
        public long Id { set; get; }
        public DateTime Timestamp { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Currency { set; get; }

        internal Account()
        {
            Timestamp = DateTime.Now;
        }

        internal Account(string name, string description, string currency, long id = 0)
        {
            Timestamp = DateTime.Now;
            Name = name;
            Description = description;
            Currency = currency;
            Id = id;
        }

        public Guid OwnerId { get;  set; }
        public Guid BaseId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   Id == account.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}
