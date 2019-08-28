using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Bases
{
    [TestClass]
    public class TransactionTest
    {
        private RegularTransactionFactory _factory;
        private ITransaction _mainTransaction;
        private ITransaction _child1;
        private ITransaction _child2;
        private ITransaction _child3;

        [TestInitialize]
        public void Setup()
        {
            _factory = new RegularTransactionFactory();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var account1 = accountFactory.CreateAccount("Main Account", "Description", "UAH");
            var account2 = accountFactory.CreateAccount("Reserve Account", "Description", "AUD");

            var category1 = categoryFactory.CreateCategory("Complex Transaction", "Description", 1, null);
            var category2 = categoryFactory.CreateCategory("Category 1", "Description", 2, null);
            var category3 = categoryFactory.CreateCategory("Category 2", "Description", 3, null);
            var category4 = categoryFactory.CreateCategory("Category 3", "Description", 4, null);
            _mainTransaction = _factory.CreateTransaction(account1, category1, "Complex Transaction", 20m,DateTime.Now,1,0,null,null);
            _child1 = _factory.CreateTransaction(account1, category2, "Child 1", 40m, DateTime.Now, 2, 0, null, null);
            _child2 = _factory.CreateTransaction(account2, category3, "Child 2", 80m, DateTime.Now, 3, 0, null, null);
            _child3 = _factory.CreateTransaction(account2, category4, "Child 3", 160m, DateTime.Now, 4, 0, null, null);
        }

        [TestMethod]
        public void AddChildTransactionTest()
        {
            _mainTransaction.AddChildTransaction(_child1);
            _mainTransaction.AddChildTransaction(_child2);
            _mainTransaction.AddChildTransaction(_child3);


            Assert.IsTrue(_mainTransaction.IsComplexTransaction);
            Assert.AreEqual(280m,_mainTransaction.Total);
            Assert.AreEqual(_mainTransaction.Account, _child2.Account);
            Assert.AreEqual(_mainTransaction.Account, _child3.Account);
            Assert.AreEqual(_mainTransaction.Timestamp, _child1.Timestamp);
            Assert.AreEqual(_mainTransaction.Timestamp, _child2.Timestamp);
            Assert.AreEqual(_mainTransaction.Timestamp, _child3.Timestamp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddChildTransactionExceptionTest()
        {
            _mainTransaction.AddChildTransaction(_child1);
            _mainTransaction.AddChildTransaction(_child1);


            Assert.IsTrue(false);
        }


        [TestMethod]
        public void DeleteChildTransactionTest()
        {
            _mainTransaction.AddChildTransaction(_child1);
            _mainTransaction.AddChildTransaction(_child2);
            _mainTransaction.AddChildTransaction(_child3);


            _mainTransaction.DeleteChildTransaction(_child2);


            Assert.IsTrue(_mainTransaction.IsComplexTransaction);
            Assert.AreEqual(200m, _mainTransaction.Total);
        }

        [TestMethod]
        public void DeleteMissedChildTransactionTest()
        {
            _mainTransaction.AddChildTransaction(_child1);
            _mainTransaction.AddChildTransaction(_child3);


            _mainTransaction.DeleteChildTransaction(_child2);


            Assert.IsTrue(_mainTransaction.IsComplexTransaction);
            Assert.AreEqual(200m, _mainTransaction.Total);
        }


        [TestMethod]
        public void DeleteLastChildTransactionTest()
        {
            _mainTransaction.AddChildTransaction(_child2);


            _mainTransaction.DeleteChildTransaction(_child2);


            Assert.IsFalse(_mainTransaction.IsComplexTransaction);
            Assert.AreEqual(0m, _mainTransaction.Total);
        }

        [TestMethod]
        public void DeleteChildrenTransactionTest()
        {
            _mainTransaction.AddChildTransaction(_child1);
            _mainTransaction.AddChildTransaction(_child2);
            _mainTransaction.AddChildTransaction(_child3);


            _mainTransaction.DeleteChildrenTransactions();


            Assert.IsFalse(_mainTransaction.IsComplexTransaction);
            Assert.AreEqual(0m, _mainTransaction.Total);
        }

    }
}
