using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryBarCodeStorage : BarCodeStorageBase
    {
        private readonly Dictionary<string, IBarCode> _storage;
        private readonly ITransactionStorage _transactionStorage;
        private long _counter = 0;

        public MemoryBarCodeStorage(IBarCodeFactory factory, ITransactionStorage transactionStorage):base(factory)
        {
            _storage = new Dictionary<string, IBarCode>();
            _transactionStorage = transactionStorage;
        }



        public override void UpdateBarCode(IBarCode barCode)
        {

        }

        public override IBarCode CreateBarCode(IBarCode barCode)
        {
            try
            {
                if (IsExists(barCode))
                {
                    DeleteBarCode(barCode);
                }
                else
                {
                    if (barCode.Id == 0)
                        barCode.Id = ++_counter;
                }
                _storage.Add(barCode.GetProductBarCode(), barCode);
                return barCode;
            }
            catch (ArgumentException e)
            {

                throw new StorageException($"{e.Message}");
            }
        }

        public override ITransaction CreateBarCodeBasedTransaction(string barCode)
        {
            var transaction = GetBarCodeTransaction(barCode);
            if (transaction == null) return transaction;

            transaction.Timestamp = DateTime.Now;
            transaction.Id = 0;
            var newTransaction = _transactionStorage.CreateTransaction(transaction);
            return newTransaction;
        }

        public override void DeleteAllData()
        {
            _storage.Clear();
        }

        public override void DeleteBarCode(IBarCode barCode)
        {
            var categoryToDelete = _storage.Where(x => x.Value.Id == barCode.Id).ToArray();
            foreach (var category1 in categoryToDelete)
            {
                _storage.Remove(category1.Key);
            }

            var id = barCode.GetProductBarCode();
            _storage.Remove(id);
        }

        public override IEnumerable<IBarCode> GetAllBarCodes()
        {
            return _storage.Select(x => x.Value);
        }

        public override ITransaction GetBarCodeTransaction(string barCode)
        {
            var foundBarCode = _storage.FirstOrDefault(x => x.Key.Equals(barCode)).Value;
            return foundBarCode?.Transaction;
        }
        private bool IsExists(IBarCode barCode)
        {
            return (barCode.Id != 0 && _storage.Any(x => x.Value.Id == barCode.Id));
        }
    }
}
