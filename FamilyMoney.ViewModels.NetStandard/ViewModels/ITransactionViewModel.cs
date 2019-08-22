using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
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

        bool IsExistingTransaction { get; }
        ObservableCollection<ITransaction> ChildrenTransactions { set; get; }

        void SaveTransaction();
        void FillFromTemplate(ITransaction selected);
        IEnumerable<ITransaction> GetSuggestions(string searchString);
        Task<string> ScanBarCode();
        void UpdateChildrenTransactionList();
        void ProcessScannedBarCode(string barCodeString);
        //Task<EditChildTransaction> ScanChildTransaction();
    }
}