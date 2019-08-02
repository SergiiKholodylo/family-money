﻿using System;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionCreateViewModel : TransactionViewModelBase, ITransactionViewModel
    {
        public TransactionCreateViewModel(IAccount activeAccount) : base()
        {

            Date = new DateTimeOffset(DateTime.Now);
            Time = DateTime.Now.TimeOfDay;
            if (activeAccount != null)
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
        }

        public void SaveTransaction()
        {
            CreateTransaction();
        }

    }
}