using System;
using System.IO;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ITransaction:ISecurity, ITreeNode<ITransaction>
    {
        IAccount Account { get; set; }
        ICategory Category { get; set; }
        string Name { get; set; }
        DateTime Timestamp { get; set; }
        decimal Total { get; set; }
        decimal Weight { get; set; }
        IProduct Product { get; set; }
        bool IsComplexTransaction { get; set; }

        void AddChildTransaction(ITransaction transaction);
        void DeleteChildrenTransactions();
        void DeleteChildTransaction(ITransaction transaction);
    }
}