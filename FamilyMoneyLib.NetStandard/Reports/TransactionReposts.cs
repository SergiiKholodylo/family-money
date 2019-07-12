using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Reports
{
    public class TransactionReport
    {
        private readonly TransactionStorage _transactionStorage;
        private readonly CategoryStorage _categoryStorage;

        public TransactionReport(TransactionStorage transactionStorage, CategoryStorage categoryStorage)
        {
            _transactionStorage = transactionStorage;
            _categoryStorage = categoryStorage;
        }

        public IEnumerable<Transaction> TransactionByCategory(Account account)
        {

            var transactions = _transactionStorage.GetAllTransactions().Where(x => x.Account.Id == account.Id);

            var topCategories = _categoryStorage.GetAllCategories().Where(x => x.ParentCategory == null);
            var transactionByCategory = transactions.ToList();
            foreach (var category in topCategories)
            {
                PrintCategory(transactionByCategory, category);
            }
            return transactionByCategory;
        }

        private void PrintCategory(IEnumerable<Transaction> transactions, Category category)
        {
            var trans = transactions.Where(x => x.Category.HasCategoryAsParent(category) || x.Category == category);
            var sum = trans.Sum(transaction => transaction.Total);
            Debug.WriteLine($"Category: {category.Name}  Amount: {sum}");
            foreach (var tran in trans)
            {
                if (tran.Category == category)
                {
                    Debug.WriteLine($"  Transaction: {tran.Id} {tran.Name}  Amount: {tran.Total}");
                }
            }
            var subCategories = _categoryStorage.GetSubcategories(category);
            foreach (var subCategory in subCategories)
            {
                PrintCategory(transactions, subCategory);
            }
        }
    }
}
