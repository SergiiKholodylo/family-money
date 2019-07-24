using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public class ManagerException:Exception
    {
        public ManagerException(string exception) : base(exception)
        {

        }
    }
}
