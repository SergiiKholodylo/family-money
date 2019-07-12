namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ITransactionItem
    {
        long Id { set; get; }
        Category Category { set; get; }
        string Name { set; get; }
        decimal Total { set; get; }
    }
}
