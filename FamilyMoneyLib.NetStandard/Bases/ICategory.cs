namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ICategory
    {
        string Description { get; set; }
        string Name { get; set; }
        long Id { get; set; }
    }
}