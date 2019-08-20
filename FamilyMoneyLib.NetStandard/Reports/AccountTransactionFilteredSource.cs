using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class AccountTransactionFilteredSource : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSource(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account)&&!x.IsComplexTransaction);
        }
    }
}