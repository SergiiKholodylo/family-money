using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.CachedStorage
{
    public class CachedQuickTransactionStorage:IQuickTransactionStorage
    {
        private bool _isDirty = true;
        private IEnumerable<IQuickTransaction> _cache;
        private readonly IQuickTransactionStorage _storage;

        public CachedQuickTransactionStorage(IQuickTransactionStorage quickTransactionStorage)
        {
            _storage = quickTransactionStorage;
        }

        public IQuickTransaction CreateQuickTransaction(IAccount account, ICategory category, string name, decimal total, 
            long id,
            decimal weight, bool askForTotal, bool askForWeight)
        {
            var result =
                _storage.CreateQuickTransaction(account, category, name, total, id, weight, askForTotal, askForWeight);
            _isDirty = true;
            return result;
        }

        public IQuickTransaction CreateQuickTransaction(IQuickTransaction quickTransaction)
        {
            var result = _storage.CreateQuickTransaction(quickTransaction);
            _isDirty = true;
            return result;
        }

        public void DeleteAllData()
        {
            _storage.DeleteAllData();
            _isDirty = true;
        }

        public void DeleteQuickTransaction(IQuickTransaction quickTransaction)
        {
            _storage.DeleteQuickTransaction(quickTransaction);
            _isDirty = true;
        }

        public IEnumerable<IQuickTransaction> GetAllQuickTransactions()
        {
            if (!_isDirty) return _cache;
            _cache = _storage.GetAllQuickTransactions();
            _isDirty = false;
            return _cache;

        }

        public void UpdateQuickTransaction(IQuickTransaction quickTransaction)
        {
            _storage.UpdateQuickTransaction(quickTransaction);
            _isDirty = true;
        }
    }
}
