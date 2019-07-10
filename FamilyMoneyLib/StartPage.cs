using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;

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
    public class Category : IIdBased
    {

        public static long NewCategoryId { get; } = -1;
        public long Id { set; get; }
        public Category TopCategory { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public Category()
        {
            Id = NewCategoryId;
        }
    }


    public class TransactionStorage
    {
        public DbStorage<Transaction> DataBaseConnector = new DbStorage<Transaction>();

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
    }
    
    public class CategoryStorage
    {
        public DbStorage<Category> DataBaseConnector = new DbStorage<Category>();

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
    }

    public class AccountStorage
    {
        public DbStorage<Account> DataBaseConnector = new DbStorage<Account>();

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


    public class DbStorage<T> where T : IIdBased, new()
    {
        private readonly List<T> _storage = new List<T>();
        public long Add(T t)
        {
            var id = GetId();
            t.Id = id;
            _storage.Add(t);
            return id;
        }

        public T Get(long id)
        {
            var found = _storage.FirstOrDefault(x => x.Id == id);
            if(found == null)
                return new T();
            return found;

        }

        public void Update(T t)
        {
            Delete(t.Id);
            _storage.Add(t);
        }

        public void Delete(long id)
        {
            var toDelete = Get(id);
            _storage.Remove(toDelete);
        }

        public IEnumerable<T> GetAll()
        {
            return _storage; 
        }

        private long GetId()
        {
            var rnd = new Random();
            while (true)
            {
                var newId = (long)rnd.Next(0xFFFF);
                if (_storage.Exists(x => x.Id == newId))
                    continue;
                return newId;
            }
        }
    }
}
