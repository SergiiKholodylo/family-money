namespace FamilyMoneyLib.NetStandard.Bases
{
    public static class QuickTransactionValidator
    {
        public static bool IsRequireInteractionForTransaction(IQuickTransaction quickTransaction)
        {
            if (quickTransaction.AskForTotal || quickTransaction.AskForWeight) return true;
            if (quickTransaction.Account == null || quickTransaction.Category == null) return true;
            if (quickTransaction.Total == 0m) return true;

            return false;
        }
    }
}
