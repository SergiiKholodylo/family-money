using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class BarCodeStorageBase:IBarCodeStorage
    {
        protected readonly IBarCodeFactory BarCodeFactory;
        protected readonly ITransactionStorage _transactionStorage;

        protected BarCodeStorageBase(IBarCodeFactory barCodeFactory, ITransactionStorage storage)
        {
            BarCodeFactory = barCodeFactory;
            _transactionStorage = storage;
        }
        

        public IBarCode CreateBarCode(string code, bool isWeight, int numberOfDigits)
        {
            var barCode = BarCodeFactory.CreateBarCode(code, isWeight, numberOfDigits);
            return CreateBarCode(barCode);
        }

        public virtual ITransaction GetBarCodeTransaction(string barCode)
        {
            var foundBarCodes = GetAllBarCodes().OrderByDescending(x => x.Id).FirstOrDefault(x => x.GetProductBarCode().Equals(barCode) && x.Transaction != null);
            return foundBarCodes?.Transaction;
        }

        public virtual ITransaction CreateTransactionBarCodeRelatedFromStorage(string barCode)
        {
            var transaction = GetBarCodeTransaction(barCode);
            if (transaction == null) return transaction;

            transaction.Timestamp = DateTime.Now;
            transaction.Id = 0;
            var newTransaction = _transactionStorage.CreateTransaction(transaction);
            return newTransaction;

        }

        public abstract IBarCode CreateBarCode(IBarCode barCode);

        public abstract void DeleteAllData();

        public abstract void DeleteBarCode(IBarCode barCode);

        public abstract IEnumerable<IBarCode> GetAllBarCodes();

        public abstract void UpdateBarCode(IBarCode barCode);
    }
}
