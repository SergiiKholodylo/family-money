using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Transactions : Page
    {
        public Transactions()
        {
            this.InitializeComponent();
            ViewModel = new TransactionsViewModel();
        }

        public TransactionsViewModel ViewModel { get; }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
