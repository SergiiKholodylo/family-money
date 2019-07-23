using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ISecurity
    {
        Guid OwnerId { set; get; }
        Guid BaseId { get; set; }
    }
}
