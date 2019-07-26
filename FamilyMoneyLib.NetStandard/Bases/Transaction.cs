using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Transaction {Id} {Name} Total {Total} Parent {ParentTransaction?.Id}")]
    public class Transaction : ITransaction
    {
        private decimal _total;
        public long Id { set; get; }
        public DateTime Timestamp { set; get; }
        public IAccount Account { set; get; }
        public ICategory Category { set; get; }
        public string Name { set; get; }

        public decimal Total
        {
            set
            {
                if(!IsComplexTransaction)
                    _total = value;
            }
            get
            {
                return IsComplexTransaction ? ChildrenTransactions.Sum(x => x.Total) : _total;
            }
        }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }
        public decimal Weight { set; get; }
        public IProduct Product { set; get; }

        public bool IsComplexTransaction { set; get; }
        public ITransaction ParentTransaction { set; get; }

        public List<ITransaction> ChildrenTransactions { get; set; } = new List<ITransaction>();

        internal Transaction()
        {
            Timestamp = DateTime.Now;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        internal Transaction(IAccount account, ICategory category, string name, decimal total, DateTime? timestamp, long id=0, decimal weight=0, IProduct product=null, ITransaction parentTransaction = null)
        {
            Account = account;
            Category = category;
            Name = name;
            Total = total;
            Timestamp = timestamp ?? DateTime.Now;
            Id = id;
            Weight = weight;
            Product = product;
            ParentTransaction = parentTransaction;
            IsComplexTransaction = false;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
            parentTransaction?.ChildrenTransactions.Add(this);
        }

        internal void AddChildrenTransaction(ITransaction transaction)
        {
            if(transaction == this) throw new ArgumentException();
            if(Id == transaction.Id) throw new ArgumentException();
            if(ChildrenTransactions.Contains(transaction)) throw new ArgumentException($"Transaction Already Exists!");
            //if (ChildrenTransactions.Count(x=>x.Id == transaction.Id)>0) throw new ArgumentException($"Transaction Already Exists!");

            IsComplexTransaction = true;
            transaction.ParentTransaction = this;
            transaction.Timestamp = Timestamp;
            transaction.Account = Account;
            ParentTransaction = null;
            ChildrenTransactions.Add(transaction);
        }

        internal void DeleteChildrenTransaction(ITransaction transaction)
        {
            var toDelete = ChildrenTransactions.Where(x => x.Id == transaction.Id);
            foreach (var transaction1 in toDelete)
            {
                ChildrenTransactions.Remove(transaction1);
            }

            IsComplexTransaction = false;
        }

        internal void DeleteAllChildrenTransactions()
        {
            ChildrenTransactions.Clear();
            IsComplexTransaction = false;
        }
    }
}
