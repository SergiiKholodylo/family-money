using System.Threading.Tasks;

namespace FamilyMoney.UWP.Bases
{
    public interface IBarCodeScanner
    {
        void  Init();
        Task<string> ScanBarCode();
        void ClearUp();
    }
}
