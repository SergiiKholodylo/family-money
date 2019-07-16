using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Accounts : Page
    {
        private readonly IAccountFactory factory;
        private readonly IAccountStorage storage;
        private readonly AccountManager manager;
        public AccountViewModel ViewModel { set; get; } = new AccountViewModel();

        public Accounts()
        {
            this.InitializeComponent();

        }

        public void RefreshPage()
        {
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAccount();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Transactions));
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Accounts));
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Categories));
        }

        //private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    CoreApplicationView newView = CoreApplication.CreateNewView();
        //    int newViewId = 0;
        //    await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        Frame frame = new Frame();
        //        frame.Navigate(typeof(AddAccount));
        //        Window.Current.Content = frame;
        //        // You have to activate the window in order to show it later.
        //        Window.Current.Activate();

        //        newViewId = ApplicationView.GetForCurrentView().Id;
        //    });
        //    bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        //}
    }
}
