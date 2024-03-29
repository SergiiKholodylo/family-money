﻿using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.Storages
{
    public abstract class AccountStorageBase:IAccountStorage
    {
        protected readonly IAccountFactory AccountFactory;

        protected AccountStorageBase(IAccountFactory factory)
        {
            AccountFactory = factory;
        }

        public abstract IAccount CreateAccount(IAccount account);

        public abstract void DeleteAccount(IAccount account);

        public abstract void UpdateAccount(IAccount account);

        public abstract IEnumerable<IAccount> GetAllAccounts();
        public IAccount CreateAccount(string name, string description, string currency)
        {
            var account = AccountFactory.CreateAccount(name, description, currency);
            return CreateAccount(account);
        }

        public abstract void DeleteAllData();
    }
}
