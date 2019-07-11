using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using FamilyMoneyLib.Storages;

namespace FamilyMoneyLib
{
    public interface IIdBased
    {
        long Id { set; get; }
    }

    public interface ITransactionItem
    {
         long Id { set; get; }
         Category Category { set; get; }
         string Name { set; get; }
         decimal Total { set; get; }
    }

   

    [DebuggerDisplay("Id {Id} {Name} ({Currency})") ]
    public class Account:IIdBased
    {
        public static long NewAccountId { get; } = -1;
    
        public long Id { set; get; }
        public DateTime Timestamp { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Currency { set; get; }

        public Account()
        {
            Timestamp = DateTime.Now;
            Id = NewAccountId;
        }

    }
    [DebuggerDisplay("Id {Id} {Name}")]
    public class Transaction:ITransactionItem, IIdBased
    {
        public static long NewTransactionId { get; } = -1;

        public DateTime Timestamp { set; get; }
        public Account Account { set; get; }

        public long Id { set; get; }
        public Category Category { set; get; }
        public string Name { set; get; }
        public decimal Total { set; get; }

        private List<ITransactionItem> _complexTransactionItems = new List<ITransactionItem>();

        public Transaction()
        {
            Timestamp = DateTime.Now;
            Id = NewTransactionId;
        }
    }

    [DebuggerDisplay("Id {Id}{ParentCategory} {Name}")]
    public class Category : IIdBased
    {

        public static long NewCategoryId { get; } = -1;
        public long Id { set; get; }
        public Category ParentCategory { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public Category()
        {
            Id = NewCategoryId;
        }

        public bool HasCategoryAsParent(Category parent)
        {
            var position = this;
            while (position.ParentCategory != null)
            {
                if (position.ParentCategory == parent) return true;
                position = position.ParentCategory;
            }
            return false;
        }

        public int GetCategoryLevel()
        {
            var level = 1;
            var position = this;
            while (position.ParentCategory != null)
            {
                position = position.ParentCategory;
                level++;
            }
            return level;

        }
    }


    public class TransactionStorage
    {
        public IDbStorage<Transaction> DataBaseConnector = new DbStorage<Transaction>();

        public long AddTransaction(Transaction transaction)
        {
            return DataBaseConnector.Add(transaction);
        }

        public Transaction GetTransaction(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            DataBaseConnector.Update(transaction);
        }

        public void DeleteTransaction(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return DataBaseConnector.GetAll();
        }

        public IEnumerable<Transaction> GetTransactionByCategory(Category category)
        {
            return DataBaseConnector.GetAll().Where(x=>x.Category == category);
        }
    }

    public class CategoryStorage
    {
        public IDbStorage<Category> DataBaseConnector = new DbStorage<Category>();

        public long AddCategory(Category category)
        {
            return DataBaseConnector.Add(category);
        }

        public Category GetCategory(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateCategory(Category category)
        {
            DataBaseConnector.Update(category);
        }

        public void DeleteCategory(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return DataBaseConnector.GetAll();
        }

        public IEnumerable<Category> GetSubcategories(Category category)
        {
            return DataBaseConnector.GetAll().Where(x => x.ParentCategory == category);
        }
    }

    public class AccountStorage
    {
        public IDbStorage<Account> DataBaseConnector = new DbStorage<Account>();

        public long AddAccount(Account account)
        {
            return DataBaseConnector.Add(account);
        }

        public Account GetAccount(long id)
        {
            return DataBaseConnector.Get(id);
        }

        public void UpdateAccount(Account account)
        {
            DataBaseConnector.Update(account);
        }

        public void DeleteAccount(long id)
        {
            DataBaseConnector.Delete(id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return DataBaseConnector.GetAll();
        }
    }


    

    public class StorageException : Exception
    {
        public StorageException() : base()
        {

        }
    }
}
