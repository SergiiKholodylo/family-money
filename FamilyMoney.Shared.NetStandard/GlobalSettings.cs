using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Cached;
using FamilyMoneyLib.NetStandard.Storages.SQLite;

namespace FamilyMoney.Shared.NetStandard
{
    public class GlobalSettings
    {

        public readonly Storages Storages;

        public GlobalSettings()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var transactionFactory = new RegularTransactionFactory();
            var quickTransactionFactory = new RegularQuickTransactionFactory();

            var accountStorage = new CachedAccountStorage( 
                new SqLiteAccountStorage(accountFactory));
            var categoryStorage = new CachedCategoryStorage(
                new SqLiteCategoryStorage(categoryFactory));
            var transactionStorage = new CachedTransactionStorage(
                new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage));

            Storages = new Storages
            {
                AccountStorage = accountStorage,
                CategoryStorage = categoryStorage,

                TransactionStorage = transactionStorage,
                BarCodeStorage = new CachedBarCodeStorage(
                    new SqLiteBarCodeStorage(new BarCodeFactory(), transactionStorage)),
                QuickTransactionStorage = new CachedQuickTransactionStorage(
                    new SqLiteQuickTransactionStorage(quickTransactionFactory, accountStorage, categoryStorage))
        };
        }


        public IBarCode ScannedBarCode { get; set; }
        public FormView FormsView { get; set; } = new FormView();

        public class FormView
        {
            public ITransactionViewModel Transaction { get; set; }
        }
    }
}
