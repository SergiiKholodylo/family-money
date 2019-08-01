using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public interface ITransactionViewModel
    {
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


        void SaveTransaction();
        void FillFromTemplate(ITransaction selected);
        IEnumerable<ITransaction> GetSuggestions(string searchString);
    }
}