using Windows.Web.AtomPub;
using FamilyMoney.UWP.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP
{
    public class GlobalSettings
    {
        public readonly IAccountStorage AccountStorage;
        public readonly ICategoryStorage CategoryStorage;
        public readonly ITransactionStorage TransactionStorage;
        public readonly IBarCodeStorage BarCodeStorage;
        public readonly IQuickTransactionStorageBase QuickTransactionStorage;

        public GlobalSettings()
        {
            var accountFactory = new RegularAccountFactory();
            var categoryFactory = new RegularCategoryFactory();
            var transactionFactory = new RegularTransactionFactory();
            var quickTransactionFactory = new RegularQuickTransactionFactory();

            AccountStorage = new SqLiteAccountStorage(accountFactory);
            CategoryStorage = new SqLiteCategoryStorage(categoryFactory);

            TransactionStorage = new SqLiteTransactionStorage(transactionFactory, AccountStorage, CategoryStorage);
            BarCodeStorage = new SqLiteBarCodeStorage(new BarCodeFactory(), TransactionStorage);
            QuickTransactionStorage = new SqLiteQuickTransactionStorage(quickTransactionFactory,AccountStorage,CategoryStorage);
        }


        public IBarCode ScannedBarCode { get; set; }
        public FormView FormsView { get; set; } = new FormView();

        public class FormView
        {
            public ITransactionViewModel Transaction { get; set; }
        }
    }
}
