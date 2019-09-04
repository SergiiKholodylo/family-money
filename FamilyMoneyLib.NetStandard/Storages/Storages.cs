using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class Storages
    {
        public IAccountStorage AccountStorage;
        public ICategoryStorage CategoryStorage;
        public ITransactionStorage TransactionStorage;
        public IBarCodeStorage BarCodeStorage;
        public IQuickTransactionStorage QuickTransactionStorage;
    }
}
