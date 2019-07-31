using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryBarCodeStorage : IBarCodeStorage
    {
        private readonly IBarCodeFactory _factory;
        private readonly Dictionary<string, IBarCode> _storage;

        public MemoryBarCodeStorage(IBarCodeFactory factory)
        {
            _factory = factory;
            _storage = new Dictionary<string, IBarCode>();
        }



        public void UpdateBarCode(IBarCode barCode)
        {

        }

        public IBarCode CreateBarCode(IBarCode barCode)
        {
            try
            {
                _storage.Add(barCode.GetProductBarCode(), barCode);
                return barCode;
            }
            catch (ArgumentException e)
            {

                throw new StorageException($"{e.Message}");
            }
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

        public ITransaction GetBarCodeTransaction(string barCode)
        {
            var foundBarCode = _storage.FirstOrDefault(x => x.Key.Equals(barCode)).Value;
            return foundBarCode?.Transaction;
        }
    }
}
