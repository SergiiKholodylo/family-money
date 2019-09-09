using System.Threading.Tasks;
using Windows.UI;
using FamilyMoney.UWP.Bases;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using ZXing.Mobile;

namespace FamilyMoney.Shared.NetStandard.Bases
{

    public class BarCodeScanner : IBarCodeScanner
    {

        public void Init()
        {
        }

        public async Task<string> ScanBarCode()
        {
            Scanner = new MobileBarcodeScanner
            {
                UseCustomOverlay = false,
                CancelButtonText = " Cancel ",
                FlashButtonText = " Toggle torch "
            };



            var result = await Scanner.Scan( new MobileBarcodeScanningOptions()
            {
                AutoRotate = false,
            });
            return result?.Text;
        }

        private MobileBarcodeScanner Scanner { get; set; }

        public void ClearUp()
        {
        }
    }
}
