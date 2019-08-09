using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public interface ITransactionViewModel:INotifyPropertyChanged
    {
        IBarCode BarCode { set; get; }
        IAccount Account { set; get; }
        ICategory Category { set; get; }
        ITransaction Transaction { set; get; }
        string Name { set; get; }
        DateTimeOffset Date { set; get; }
        TimeSpan Time { get; set; }
        decimal Total { set; get; }
        decimal Weight { set; get; }
        bool IsComplexTransaction { set; get; }
        string ErrorString { set; get; }
        IEnumerable<ICategory> Categories { get; }
        IEnumerable<IAccount> Accounts { get; }
        IEnumerable<ITransaction> Transactions { get; }
        ObservableCollection<ITransaction> ChildrenTransactions { set; get; }
        Visibility IsChildTransactionVisible { get; }
        Visibility IsChildTransactionHidden { get; }

        void SaveTransaction();
        void FillFromTemplate(ITransaction selected);
        IEnumerable<ITransaction> GetSuggestions(string searchString);
        Task<string> ScanBarCode();

        void ProcessScannedBarCode(string barCodeString);
        Task<EditChildTransaction> ScanChildTransaction();
    }
}