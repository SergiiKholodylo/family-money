using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class TransactionFilteredSource : ITransactionFilteredSource
    {
        public IAccount Account { get;  set; }
        public ICategory Category { get;  set; }
        public bool IncludeSubCategories { get;  set; }
        public DateTime DataStart { get ; set; }
        public DateTime DataEnd { get; set; }


        public TransactionFilteredSource(DateTime dataStart, DateTime dataEnd, IAccount account = null, ICategory category = null,
            bool includeSubCategories = false )
        {
            DataStart = dataStart;
            DataEnd = dataEnd;
            Account = account;
            Category = category;
            IncludeSubCategories = includeSubCategories;
        }


        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            var temp = ApplyFilterByDate(transactionStorage);
            temp = ApplyFilterByAccount(temp);
            temp = ApplyFilterByCategory(temp);

            return temp;
        }

        private IEnumerable<ITransaction> ApplyFilterByCategory(IEnumerable<ITransaction> temp)
        {
            if (Category != null)
            {
                temp = IncludeSubCategories ? temp.Where(x => x.Category.Equals(Category) || x.Category.IsParent(Category)) : temp.Where(x => x.Category.Equals(Category));
            }

            return temp;
        }

        private IEnumerable<ITransaction> ApplyFilterByAccount(IEnumerable<ITransaction> temp)
        {
            if (Account != null)
                temp = temp.Where(x => x.Account.Equals(Account));
            return temp;
        }

        private IEnumerable<ITransaction> ApplyFilterByDate(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions().
                Where(x => !x.IsComplexTransaction && x.Timestamp > DataStart && x.Timestamp < DataEnd);
        }
    }
}