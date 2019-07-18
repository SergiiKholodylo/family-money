using System;
using System.Collections.Generic;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ITransactionFactory _transactionFactory;
        private readonly ITransactionStorage _transactionStorage;

        public TransactionManager(ITransactionFactory factory, ITransactionStorage storage)
        {
            _transactionFactory = factory;
            _transactionStorage = storage;
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total)
        {
            var transaction = _transactionFactory.CreateTransaction(account, category, name, total);
            return _transactionStorage.CreateTransaction(transaction);
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp)
        {
            var transaction = _transactionFactory.CreateTransaction(account, category, name, total, timestamp);
            return _transactionStorage.CreateTransaction(transaction);
        }

        public void UpdateTransaction(ITransaction transaction)
        {
            _transactionStorage.UpdateTransaction(transaction);
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            _transactionStorage.DeleteTransaction(transaction);
        }

        public IEnumerable<ITransaction> GetAllTransactions()
        {
            return _transactionStorage.GetAllTransactions();
        }
    }
}
