using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryBarCodeStorage
    {
        private readonly IBarCodeFactory _factory;
        private Dictionary<string, IBarCode> _storage;

        public MemoryBarCodeStorage(IBarCodeFactory factory)
        {
            _factory = factory;
            _storage = new Dictionary<string, IBarCode>();
        }

        public IBarCode CreateBarCode(string code, bool isWeight=false, int numberOfDigitsForWeight=0)
        {
            try
            {
                var barCode = _factory.CreateBarCode(code, isWeight, numberOfDigitsForWeight);
                _storage.Add(barCode.GetProductBarCode(),barCode);
                return barCode;
            }
            catch (ArgumentException e)
            {
                
                throw new StorageException( $"{e.Message}");
            }
        }

        public void UpdateBarCode(IBarCode barCode)
        {

        }

        public void DeleteBarCode(IBarCode barCode)
        {
            var id = barCode.GetProductBarCode();
            _storage.Remove(id);
        }

        public IEnumerable<IBarCode> GetAllBarCodes()
        {
            return _storage.Select(x => x.Value);
        }

    }
}
