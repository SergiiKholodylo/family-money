using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.CachedStorage
{
    public class CachedBarCodeStorage:IBarCodeStorage
    {
        private bool _isDirty = true;
        private IEnumerable<IBarCode> _cache;
        private readonly IBarCodeStorage _storage;

        public CachedBarCodeStorage(IBarCodeStorage barCodeStorage)
        {
            _storage = barCodeStorage;
        }

        public IBarCode CreateBarCode(IBarCode barCode)
        {
            var returnBarCode = _storage.CreateBarCode(barCode);
            _isDirty = true;
            return returnBarCode;
        }

        public ITransaction CreateTransactionBarCodeRelatedFromStorage(string barCode)
        {
            var result = _storage.CreateTransactionBarCodeRelatedFromStorage(barCode);
            _isDirty = true;
            return result;
        }

        public void DeleteAllData()
        {
            _storage.DeleteAllData();
            _isDirty = true;
        }

        public void DeleteBarCode(IBarCode barCode)
        {
            _storage.DeleteBarCode(barCode);
            _isDirty = true;
        }

        public IEnumerable<IBarCode> GetAllBarCodes()
        {
            if (!_isDirty) return _cache;
            _cache = _storage.GetAllBarCodes();
            _isDirty = false;
            return _cache;
        }

        public ITransaction GetBarCodeTransaction(string barCode)
        {
            var result = _storage.GetBarCodeTransaction(barCode);
            _isDirty = true;
            return result;
        }

        public void UpdateBarCode(IBarCode barCode)
        {
            _storage.UpdateBarCode(barCode);
            _isDirty = true;
        }
    }
}
