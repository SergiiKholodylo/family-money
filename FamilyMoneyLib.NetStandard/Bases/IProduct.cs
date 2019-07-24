namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IProduct
    {
        long Id { get; set; }
        string Name { get; set; }
        ICategory LastCategory { get; set; }
    }
}