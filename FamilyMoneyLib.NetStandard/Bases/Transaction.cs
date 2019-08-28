using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Transaction {Id} {Name} Total {Total} Parent {ParentTransaction?.Id}")]
    public class Transaction : TreeNodeBase<ITransaction>, ITransaction
    {
        private decimal _total;
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
                if (!IsComplexTransaction) return _total;
                var total = 0m;
                foreach (var treeNode in Children)
                {
                    total += ((ITransaction) treeNode).Total;
                }
                return total;
            }
        }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }
        public decimal Weight { set; get; }
        public IProduct Product { set; get; }

        public bool IsComplexTransaction { set; get; }


        internal Transaction():base()
        {
            Timestamp = DateTime.Now;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        internal Transaction(IAccount account, ICategory category, string name, decimal total, DateTime? timestamp, long id=0, decimal weight=0, IProduct product=null, ITransaction parentTransaction = null):base()
        {
            Account = account;
            Category = category;
            Name = name;
            Total = total;
            Timestamp = timestamp ?? DateTime.Now;
            Id = id;
            Weight = weight;
            Product = product;
            Parent = parentTransaction;
            IsComplexTransaction = false;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        public void AddChildTransaction(ITransaction transaction)
        {
            if(transaction == this) throw new ArgumentException();
            if(Id == transaction.Id) throw new ArgumentException();
            if(Children.Contains(transaction)) throw new ArgumentException($"Transaction Already Exists!");
            if(Parent!=null) throw new ArgumentException("Can't add chil transaction to child transaction");
            //if (ChildrenTransactions.Count(x=>x.Id == transaction.Id)>0) throw new ArgumentException($"Transaction Already Exists!");

            IsComplexTransaction = true;
            transaction.Parent = this;
            transaction.Timestamp = Timestamp;
            transaction.Account = Account;
            Parent = null;
            Children.Add(transaction);
        }

        public void DeleteChildTransaction(ITransaction childTransaction)
        {
            if (!IsComplexTransaction) return;

            if (childTransaction.Parent != this) return;

            var toDelete = Children.Where(x => x.Id == childTransaction.Id).ToArray();
            foreach (var transaction1 in toDelete)
            {
                Children.Remove(transaction1);
            }

            IsComplexTransaction = Children.Any();
            if (!IsComplexTransaction) Total = 0;
        }

        public void DeleteChildrenTransactions()
        {
            if (!IsComplexTransaction) return;

            Children.Clear();
            IsComplexTransaction = false;
            Total = 0;
        }
    }
}
