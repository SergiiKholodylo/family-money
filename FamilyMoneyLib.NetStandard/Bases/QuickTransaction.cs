using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public class QuickTransaction:IQuickTransaction,ISecurity
    {
        public long Id { set; get; }
        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public decimal Weight { get; set; }
        public bool AskForTotal { get; set; }
        public bool AskForWeight { get; set; }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }
    }
}
