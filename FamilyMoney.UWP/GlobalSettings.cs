using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Managers;
using FamilyMoneyLib.NetStandard.SQLite;

namespace FamilyMoney.UWP
{
    public class GlobalSettings
    {
        public readonly IAccountManager AccountManager;
        public readonly ICategoryManager CategoryManager;
        public readonly ITransactionManager TransactionManager;


        public GlobalSettings()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);

            AccountManager = new AccountManager(accountFactory, accountStorage);
            CategoryManager = new CategoryManager(categoryFactory, categoryStorage);
            var transactionFactory = new RegularTransactionFactory();
            TransactionManager = new TransactionManager(transactionFactory, new SqLiteTransactionStorage(transactionFactory, accountStorage,categoryStorage,accountFactory,categoryFactory));
        }
    }
}
