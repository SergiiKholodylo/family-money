using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public static class QuickTransactionConverter
    {
        public static ITransaction ToTransaction(ITransactionFactory transactionFactory, IQuickTransaction quickTransaction)
        {
            var result = transactionFactory.CreateTransaction(quickTransaction.Account, quickTransaction.Category,
                quickTransaction.Name, quickTransaction.Total, DateTime.Now, 0, quickTransaction.Weight, null, null);
            return result;
        }
    }
}
