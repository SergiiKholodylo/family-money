using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public interface IBarCodeStorage
    {
        IBarCode CreateBarCode(IBarCode barCode);
        void DeleteBarCode(IBarCode barCode);
        IEnumerable<IBarCode> GetAllBarCodes();
        ITransaction GetBarCodeTransaction(string barCode);
        void UpdateBarCode(IBarCode barCode);
    }
}