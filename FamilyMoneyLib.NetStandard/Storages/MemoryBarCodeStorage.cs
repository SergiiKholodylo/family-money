using System;
using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public class MemoryBarCodeStorage : BarCodeStorageBase
    {

        private readonly MemoryStorageBase _storageEngine = new MemoryStorageBase();

        public MemoryBarCodeStorage(IBarCodeFactory factory, ITransactionStorage transactionStorage):base(factory,transactionStorage)
        {
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

    }
}
