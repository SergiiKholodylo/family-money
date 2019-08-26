using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.CachedStorage
{
    public class CachedTransactionStorage:ITransactionStorage
    {
        private bool _isDirty = true;
        private IEnumerable<ITransaction> _cache;
        private readonly ITransactionStorage _storage;

        public CachedTransactionStorage(ITransactionStorage transactionStorage)
        {
            _storage = transactionStorage;
        }

        public ITransaction CreateTransaction(ITransaction transaction)
        {
            var result = _storage.CreateTransaction(transaction);
            _isDirty = true;
            return result;
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp,
            long id, decimal weight, IProduct product, ITransaction parentTransaction)
        {
            var result = _storage.CreateTransaction(account,category,name,total,timestamp,id,weight,product,parentTransaction);
            _isDirty = true;
            return result;
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            _storage.DeleteTransaction(transaction);
            _isDirty = true;
        }

        public IEnumerable<ITransaction> GetAllTransactions()
        {
           if (!_isDirty) return _cache;

           _cache = _storage.GetAllTransactions();

           _isDirty = false;
            return _cache;
        }

        public void UpdateTransaction(ITransaction transaction)
        {
            _storage.UpdateTransaction(transaction);
            _isDirty = true;
        }

        public void AddChildrenTransaction(ITransaction parent, ITransaction child)
        {
            _storage.AddChildrenTransaction(parent,child);
            _isDirty = true;
        }

        public void DeleteAllChildrenTransactions(Transaction parent)
        {
            _storage.DeleteTransaction(parent);
            _isDirty = true;
        }

        public void DeleteChildrenTransaction(Transaction parent, Transaction child)
        {
            _storage.DeleteChildrenTransaction(parent,child);
            _isDirty = true;
        }

        public void DeleteAllData()
        {
            _storage.DeleteAllData();
            _isDirty = true;
        }
    }
}
