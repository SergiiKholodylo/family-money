using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP
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

            var accountStorage = new SqLiteAccountStorage(accountFactory);
            var categoryStorage = new SqLiteCategoryStorage(categoryFactory);
            var transactionStorage = new SqLiteTransactionStorage(transactionFactory, accountStorage, categoryStorage);

            Storages = new Storages
            {
                AccountStorage = accountStorage,
                CategoryStorage = categoryStorage,

                TransactionStorage = transactionStorage,
                BarCodeStorage = new SqLiteBarCodeStorage(new BarCodeFactory(), transactionStorage),
                QuickTransactionStorage = new SqLiteQuickTransactionStorage(quickTransactionFactory, accountStorage, categoryStorage)
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
