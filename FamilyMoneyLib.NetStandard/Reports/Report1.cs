using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class Report1
    {
        private readonly ITransactionStorage _transactionStorage;
        private readonly ICategoryStorage _categoryStorage;

        public Report1(ITransactionStorage transactionStorage, ICategoryStorage categoryStorage)
        {
            _transactionStorage = transactionStorage;
            _categoryStorage = categoryStorage;
        }

        public Dictionary<CategoryAccountPair, decimal> Execute(ITransactionFilteredSource transactionFilteredSource)
        {
            var sum = RetrieveDataFromStorage(transactionFilteredSource).ToArray();

            var activeAccounts = sum.GroupBy(x => x.Account).Select(g=>(g.Key));

            var flatCategory = _categoryStorage.MakeFlatCategoryTree().ToArray();

            var totalCategoryList = FillTotalCategoryList(flatCategory,activeAccounts);

            CalculateSumForCategories(sum, flatCategory, totalCategoryList);

            RemoveLinesWithZeroSum(totalCategoryList);

            return totalCategoryList; 
        }

        private IEnumerable<SumByCategories> RetrieveDataFromStorage(
            ITransactionFilteredSource transactionFilteredSource)
        {
            var allTransactions = transactionFilteredSource.GetTransactions(_transactionStorage);//_transactionStorage.GetAllTransactions();

            var sumByCategories = allTransactions.GroupBy(x => new Tuple<IAccount,ICategory>(x.Account, x.Category ));

            var sum = sumByCategories.Select(g => new SumByCategories
            {
                Account = g.Key.Item1,
                Category = g.Key.Item2,
                Total = g.Sum(x => x.Total)
            });
            return sum;
        }

        private static Dictionary<CategoryAccountPair, decimal> FillTotalCategoryList(ICategory[] flatCategory,
            IEnumerable<IAccount> activeAccounts)
        {
            var totalCategoryList = new Dictionary<CategoryAccountPair, decimal>();
            foreach (var activeAccount in activeAccounts)
            {
                foreach (var category in flatCategory)
                {
                    totalCategoryList.Add(new CategoryAccountPair(activeAccount, category), 0m);
                }
            }

            return totalCategoryList;
        }

        private static void CalculateSumForCategories(IEnumerable<SumByCategories> sum, ICategory[] flatCategory, 
            Dictionary<CategoryAccountPair, decimal> totalCategoryList)
        {
            foreach (var sumByCategory in sum)
            {
                totalCategoryList[new CategoryAccountPair(sumByCategory.Account,sumByCategory.Category)] += sumByCategory.Total;

                CalculateSumForParentCategories(flatCategory, totalCategoryList, sumByCategory);
            }
        }

        private static void CalculateSumForParentCategories(ICategory[] flatCategory, 
            Dictionary<CategoryAccountPair, decimal> totalCategoryList, 
            SumByCategories sumByCategory)
        {

            foreach (var category in flatCategory)
            {
                if (sumByCategory.Category.IsParent(category))
                {
                    totalCategoryList[new CategoryAccountPair(sumByCategory.Account, category)] += sumByCategory.Total;
                }
            }
        }

        private static void RemoveLinesWithZeroSum(Dictionary<CategoryAccountPair, decimal> totalCategoryList)
        {
            var toDeleteFromList = (from total in totalCategoryList where total.Value == 0 select total.Key).ToList();

            foreach (var category in toDeleteFromList)
            {
                totalCategoryList.Remove(category);
            }
        }

        [DebuggerDisplay("{Account?.Name} {Category?.Name}")]
        public class CategoryAccountPair
        {
            protected CategoryAccountPair()
            {
            }

            
            public CategoryAccountPair(IAccount account, ICategory category)
            {
                Account = account;
                Category = category;
            }

            public ICategory Category { get; set; }
            public IAccount Account { get; set; }

            public override bool Equals(object obj)
            {
                return obj is CategoryAccountPair pair &&
                       EqualityComparer<ICategory>.Default.Equals(Category, pair.Category) &&
                       EqualityComparer<IAccount>.Default.Equals(Account, pair.Account);
            }

            public override int GetHashCode()
            {
                var hashCode = -416904139;
                hashCode = hashCode * -1521134295 + EqualityComparer<ICategory>.Default.GetHashCode(Category);
                hashCode = hashCode * -1521134295 + EqualityComparer<IAccount>.Default.GetHashCode(Account);
                return hashCode;
            }
        }

        [DebuggerDisplay("{Total} {Account?.Name} {Category?.Name}")]
        private class SumByCategories:CategoryAccountPair
        {
            public decimal Total { get; set; }

            public SumByCategories(IAccount account, ICategory category) : base(account, category)
            {
            }

            public SumByCategories() 
            {
            }
        }

    }

    public interface ITransactionFilteredSource
    {
        IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage);
    }

    public class AllTransactionFilteredSource : ITransactionFilteredSource
    {

        public IEnumerable<ITransaction> GetTransactions(ITransactionStorage transactionStorage)
        {
            return transactionStorage.GetAllTransactions();
        }
    }
}
