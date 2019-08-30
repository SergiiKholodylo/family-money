using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using ZXing.Mobile;

namespace FamilyMoney.UWP.Bases
{

    public class BarCodeScanner : IBarCodeScanner
    {

        public async void Init()
        {
        }

        public async Task<string> ScanBarCode()
        {
            Scanner = new MobileBarcodeScanner();

            var overlay = CreateOverlay();

            Scanner.CustomOverlay = overlay;
            Scanner.UseCustomOverlay = true;

            var result = await Scanner.Scan();
            return result?.Text;
        }

        private MobileBarcodeScanner Scanner { get; set; }

        private UIElement CreateOverlay()
        {
            var grid = new Grid();

                var gridInternal = new Grid();

                var button1 = new Button
                {
                    Background = new SolidColorBrush(Colors.Black),
                    Foreground = new SolidColorBrush(Colors.AliceBlue),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Name = "ButtonCancel",
                    Content = " Cancel ",
                };
                var button2 = new Button
                {
                    Background = new SolidColorBrush(Colors.Black),
                    Foreground = new SolidColorBrush(Colors.AliceBlue),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Name = "ButtonTorch",
                    Content = " Torch ",
                };
                var button3 = new Button
                {
                    Background = new SolidColorBrush(Colors.Black),
                    Foreground = new SolidColorBrush(Colors.AliceBlue),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Name = "ButtonFocus",
                    Content = " AutoFocus ",
                };

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            button1.SetValue(Grid.RowProperty,1);
            button2.SetValue(Grid.RowProperty, 1);
            button1.SetValue(Grid.ColumnProperty, 0);
            button2.SetValue(Grid.ColumnProperty, 1);
            button3.SetValue(Grid.RowProperty, 1);
            button3.SetValue(Grid.ColumnProperty, 2);

            gridInternal.ColumnDefinitions.Add(new ColumnDefinition());
            gridInternal.ColumnDefinitions.Add(new ColumnDefinition());
            gridInternal.ColumnDefinitions.Add(new ColumnDefinition());
            gridInternal.RowDefinitions.Add(new RowDefinition{Height = new GridLength(1, GridUnitType.Star)});
            gridInternal.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            gridInternal.Children.Add(button2);
            gridInternal.Children.Add(button3);
            gridInternal.Children.Add(button1);
            
            grid.Children.Add(gridInternal);
            return grid;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Scanner.ToggleTorch();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Scanner.Cancel();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Scanner.AutoFocus();
        }

        public async void ClearUp()
        {
        }
    }
}
