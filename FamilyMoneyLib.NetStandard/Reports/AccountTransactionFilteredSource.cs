using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class AccountTransactionFilteredSource : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSource(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public bool IncludeSubCategories { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account)&&!x.IsComplexTransaction);
        }
        
    }

    public class AccountTransactionFilteredSourceToday : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSourceToday(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public bool IncludeSubCategories { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account) && !x.IsComplexTransaction && x.Timestamp.Date == DateTime.Today);
        }

        
    }

    public class AccountTransactionFilteredSourceYesterday : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSourceYesterday(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public bool IncludeSubCategories { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account) && !x.IsComplexTransaction && x.Timestamp.Date == DateTime.Today.AddDays(-1));
        }

        
    }

    public class AccountTransactionFilteredSourceThisMonth : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSourceThisMonth(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public bool IncludeSubCategories { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account) && !x.IsComplexTransaction && x.Timestamp.Month == DateTime.Today.Month);
        }

        
    }

    public class AccountTransactionFilteredSourceLastMonth : ITransactionFilteredSource
    {
        private readonly IAccount _account;

        public AccountTransactionFilteredSourceLastMonth(IAccount account)
        {
            _account = account ?? throw new ArgumentNullException("Account musn't be NULL");
        }

        public IAccount Account { get; set; }
        public ICategory Category { get; set; }
        public bool IncludeSubCategories { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataEnd { get; set; }

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().Where(x => x.Account.Equals(_account) && !x.IsComplexTransaction && x.Timestamp.Month == DateTime.Today.AddMonths(-1).Month);
        }

        
    }
}