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
            try
            {
                var transaction = _transactionFactory.CreateTransaction(account, category, name, total);
                return _transactionStorage.CreateTransaction(transaction);
            }
            catch (StorageException e)
            {
                throw new ManagerException(e.Message);
            }
        }

        public ITransaction CreateTransaction(IAccount account, ICategory category, string name, decimal total, DateTime timestamp)
        {
            try
            {
                var transaction = _transactionFactory.CreateTransaction(account, category, name, total, timestamp);
                return _transactionStorage.CreateTransaction(transaction);
            }
            catch (StorageException e)
            {
                throw new ManagerException(e.Message);
            }
        }

        public void UpdateTransaction(ITransaction transaction)
        {
            try
            {
                _transactionStorage.UpdateTransaction(transaction);
            }
            catch (StorageException e)
            {
                throw new ManagerException(e.Message);
            }
        }

        public void DeleteTransaction(ITransaction transaction)
        {
            try
            {
                _transactionStorage.DeleteTransaction(transaction);
            }
            catch (StorageException e)
            {
                throw new ManagerException(e.Message);
            }
        }

        public IEnumerable<ITransaction> GetAllTransactions()
        {
            try
            {
                return _transactionStorage.GetAllTransactions();
            }
            catch (StorageException e)
            {
                throw new ManagerException(e.Message);
            }
        }
    }
}
