using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyMoney.UWP.Bases
{
    public interface IBarCodeScanner
    {
        void  Init();
        Task<string> ScanBarCode();
        void ClearUp();
    }

    public class BarCodeScanner : IBarCodeScanner
    {
        public async void Init()
        {
        }

        public async Task<string> ScanBarCode()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            return result?.Text;
        }

        public async void ClearUp()
        {
        }
    }
}
