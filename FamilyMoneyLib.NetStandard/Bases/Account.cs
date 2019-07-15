using System;
using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{
    [DebuggerDisplay("Account {Name} ({Currency})")]
    public class Account : IAccount
    {
        public DateTime Timestamp { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Currency { set; get; }

        internal Account()
        {
            Timestamp = DateTime.Now;
        }

        internal Account(string name, string description, string currency)
        {
            Timestamp = DateTime.Now;
            Name = name;
            Description = description;
            Currency = currency;
        }
    }
}
