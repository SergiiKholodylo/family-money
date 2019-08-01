using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.Views
{
    public class TransactionPageParameter
    {
        public readonly IAccount ActiveAccount;
        public readonly TransactionAction Action;


        public TransactionPageParameter(ITransaction activeTransaction)
        {
            ActiveTransaction = activeTransaction;
            Action = TransactionAction.EditTransaction;
        }

        public TransactionPageParameter(IAccount activeAccount)
        {
            ActiveAccount = activeAccount;
            Action = TransactionAction.CreateTransactionForAccount;
        }

        public ITransaction ActiveTransaction { get; private set; }
    }
}