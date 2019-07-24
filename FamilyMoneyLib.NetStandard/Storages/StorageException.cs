using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class StorageException:Exception
    {
        public StorageException(string exceptionMassage) : base(exceptionMassage)
        {

        }
    }
}
