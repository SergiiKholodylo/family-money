using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.CachedStorage
{
    public class CachedAccountStorage:IAccountStorage
    {
        private bool _isDirty = true;
        private IEnumerable<IAccount> _cache;
        private readonly IAccountStorage _storage;

        public CachedAccountStorage(IAccountStorage accountStorage)
        {
            _storage = accountStorage;
        }

        public IAccount CreateAccount(IAccount account)
        {
            var returnAccount = _storage.CreateAccount(account);
            _isDirty = true;
            return null;
        }

        public void DeleteAccount(IAccount account)
        {
            _storage.DeleteAccount(account);
            _isDirty = true;
        }

        public void UpdateAccount(IAccount account)
        {
            _storage.UpdateAccount(account);
            _isDirty = true;
        }

        public IEnumerable<IAccount> GetAllAccounts()
        {
            if (!_isDirty) return _cache;
            _cache = _storage.GetAllAccounts();
            _isDirty = false;
            return _cache;
        }

        public IAccount CreateAccount(string name, string description, string currency)
        {
            var returnAccount = _storage.CreateAccount(name, description, currency);
            _isDirty = true;
            return returnAccount;
        }

        public void DeleteAllData()
        {
            _storage.DeleteAllData();
            _isDirty = true;
        }
    }
}
