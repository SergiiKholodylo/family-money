using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyMoneyTest
{
    [TestClass]
    public class TransactionStorageTest
    {
        [TestMethod]
        public void TransactionAddTransactionTest()
        {
            var transaction = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();


            var transactionNumber = transactionStorage.AddTransaction(transaction);


            Assert.AreNotEqual(Transaction.NewTransactionId, transactionNumber, "Transaction Number mustn't be equal New Transaction Id");
        }

        [TestMethod]
        public void TransactionAddMultiplyTransactionsTest()
        {
            var transaction1 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transaction2 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transaction3 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();


            var transactionNumber1 = transactionStorage.AddTransaction(transaction1);
            var transactionNumber2 = transactionStorage.AddTransaction(transaction2);
            var transactionNumber3 = transactionStorage.AddTransaction(transaction3);


            Assert.AreEqual(
                (
                    transactionNumber1 != transactionNumber2 &&
                    transactionNumber1 != transactionNumber3 &&
                    transactionNumber2 != transactionNumber3), true, "All Transaction Numbers must be different!");
        }

        [TestMethod]
        public void TransactionAddTransactionAndGetTransactionTest()
        {
            var transaction = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();


            var transactionNumber = transactionStorage.AddTransaction(transaction);
            var storedTransaction = transactionStorage.GetTransaction(transactionNumber);


            Assert.AreEqual(transaction.Id, storedTransaction.Id, "Transaction's Numbers must be equal");
            Assert.AreEqual(transaction.Total, storedTransaction.Total, "Transaction's Total must be equal");
        }

        [TestMethod]
        public void TransactionUpdateTransactionAndGetTransactionTest()
        {
            var transaction = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();
            var transactionNumber = transactionStorage.AddTransaction(transaction);
            var newTransaction = new Transaction
            {
                Id = transaction.Id,
                Category = transaction.Category,
                Account = transaction.Account,
                Total = 50
            };


            transactionStorage.UpdateTransaction(newTransaction);
            var storedTransaction = transactionStorage.GetTransaction(transactionNumber);


            Assert.AreEqual(newTransaction.Id, storedTransaction.Id, "Transaction's Numbers must be equal");
            Assert.AreEqual(newTransaction.Total, storedTransaction.Total, "Transaction's Total must be equal");
        }

        [TestMethod]
        public void TransactionDeleteTransactionTest()
        {
            var transaction = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();


            var transactionNumber = transactionStorage.AddTransaction(transaction);
            transactionStorage.DeleteTransaction(transactionNumber);
            var storedTransaction = transactionStorage.GetTransaction(transactionNumber);


            Assert.AreEqual(Transaction.NewTransactionId, storedTransaction.Id, "Transaction must be Empty");
        }

        [TestMethod]
        public void TransactionDeleteNonexistentTransactionTest()
        {
            const long anyTransactionNumber = 567L;
            var transactionStorage = new TransactionStorage();


            transactionStorage.DeleteTransaction(anyTransactionNumber);


            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TransactionGetAllTransactionsTest()
        {
            var transaction1 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transaction2 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transaction3 = new Transaction
            {
                Account = new Account(),
                Category = new Category(),
                Total = 20
            };
            var transactionStorage = new TransactionStorage();


            transactionStorage.AddTransaction(transaction1);
            transactionStorage.AddTransaction(transaction2);
            transactionStorage.AddTransaction(transaction3);
            var allTransactions = transactionStorage.GetAllTransactions();


            Assert.AreEqual(3, allTransactions.Count(), "There must be 3 transactions");
        }
    }
}
