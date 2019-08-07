using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class BarCodeStorageBase:IBarCodeStorage
    {
        protected readonly IBarCodeFactory BarCodeFactory;

        protected BarCodeStorageBase(IBarCodeFactory barCodeFactory)
        {
            BarCodeFactory = barCodeFactory;
        }
        

        public IBarCode CreateBarCode(string name, bool isWeight, int numberOfDigits)
        {
            var barCode = BarCodeFactory.CreateBarCode(name, isWeight, numberOfDigits);
            return CreateBarCode(barCode);
        }

        public abstract IBarCode CreateBarCode(IBarCode barCode);

        public abstract ITransaction CreateBarCodeBasedTransaction(string barCode);

        public abstract void DeleteBarCode(IBarCode barCode);

        public abstract IEnumerable<IBarCode> GetAllBarCodes();

        public abstract ITransaction GetBarCodeTransaction(string barCode);

        public abstract void UpdateBarCode(IBarCode barCode);
    }
}
