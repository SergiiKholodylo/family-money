using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryBarCodeStorage : BarCodeStorageBase
    {

        private readonly ITransactionStorage _transactionStorage;

        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();


        public MemoryBarCodeStorage(IBarCodeFactory factory, ITransactionStorage transactionStorage):base(factory)
        {
            _transactionStorage = transactionStorage;
        }



        public override void UpdateBarCode(IBarCode barCode)
        {

        }

        public override IBarCode CreateBarCode(IBarCode barCode)
        {
            try
            {
                return _storageEngine.Create(barCode) as IBarCode;
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
            _storageEngine.DeleteAllData();
        }

        public override void DeleteBarCode(IBarCode barCode)
        {
            _storageEngine.Delete(barCode);
        }

        public override IEnumerable<IBarCode> GetAllBarCodes()
        {
            return _storageEngine.GetAll().Cast<IBarCode>();
        }

        public override ITransaction GetBarCodeTransaction(string barCode)
        {
            //var foundBarCode = _storage.FirstOrDefault(x => x.Key.Equals(barCode)).Value;
            var foundBarCode = GetAllBarCodes().FirstOrDefault(x => x.GetProductBarCode().Equals(barCode));
            return foundBarCode?.Transaction;
        }
    }
}
