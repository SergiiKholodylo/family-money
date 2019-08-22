using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.SQLite;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class Storages
    {
        public IAccountStorage AccountStorage;
        public ICategoryStorage CategoryStorage;
        public ITransactionStorage TransactionStorage;
        public IBarCodeStorage BarCodeStorage;
        public IQuickTransactionStorageBase QuickTransactionStorage;
    }
}
