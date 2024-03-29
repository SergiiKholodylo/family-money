﻿using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public class TransactionEditViewModel : TransactionViewModelBase, ITransactionViewModel
    {
        public TransactionEditViewModel(Storages storages, ITransaction transaction):base(storages)
        {
            Transaction = transaction;
            Date = transaction == null || transaction.Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(transaction.Timestamp);
            Time = transaction?.Timestamp.TimeOfDay ?? DateTime.Now.TimeOfDay;
            Name = transaction?.Name;
            Account = Accounts.FirstOrDefault(x => x.Id == transaction?.Account.Id);
            Category = Categories.FirstOrDefault(x => x.Id == transaction?.Category.Id);
            Timestamp = transaction?.Timestamp ?? DateTime.Now;
            if (transaction != null)
            {
                Total = transaction.Total;
                Weight = transaction.Weight;
                IsComplexTransaction = transaction.IsComplexTransaction;
                ParentTransaction = (ITransaction)transaction.Parent;
            }
            CopyChildrenTransactions(transaction);
        }

        private void CopyChildrenTransactions(ITransaction transaction)
        {
            if(transaction == null)return;
            ChildrenTransactions.Clear();
            var children = transaction.Children.Select(x => (ITransaction) x);
            foreach (var child in children)
            {
                ChildrenTransactions.Add(child);
            }
        }

        public void SaveTransaction()
        {
            UpdateTransaction();
        }
    }
}