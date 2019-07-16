using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP
{
    public class GlobalSettings
    {
        public IAccountManager AccountManager;
        public ICategoryManager CategoryManager;
        public ITransactionManager TransactionManager;


        public GlobalSettings()
        {
            AccountManager = new AccountManager(new RegularAccountFactory(), new MemoryAccountStorage());
            CategoryManager = new CategoryManager(new RegularCategoryFactory(), new MemoryCategoryStorage());
            TransactionManager = new TransactionManager(new RegularTransactionFactory(), new MemoryTransactionStorage());
        }
    }
}
