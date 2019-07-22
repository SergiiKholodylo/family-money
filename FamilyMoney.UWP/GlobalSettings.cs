using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP
{
    public class GlobalSettings
    {
        public readonly IAccountManager AccountManager;
        public readonly ICategoryManager CategoryManager;
        public readonly ITransactionManager TransactionManager;


        public GlobalSettings()
        {
            var accountStorage = new SqLiteAccountStorage();
            var categoryStorage = new SqLiteCategoryStorage();
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            AccountManager = new AccountManager(accountFactory, accountStorage);
            CategoryManager = new CategoryManager(categoryFactory, categoryStorage);
            TransactionManager = new TransactionManager(new RegularTransactionFactory(), new SqLiteTransactionStorage(accountStorage,categoryStorage,accountFactory,categoryFactory));
        }
    }
}
