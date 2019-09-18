using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class Report2
    {
        private readonly ITransactionStorage _transactionStorage;

        public Report2(ITransactionStorage transactionStorage)
        {
            _transactionStorage = transactionStorage;
        }

        public IEnumerable<ITransaction> Execute(ITransactionFilteredSource transactionFilteredSource)
        {
            return transactionFilteredSource.GetTransactions(_transactionStorage);
        }
    }
}
