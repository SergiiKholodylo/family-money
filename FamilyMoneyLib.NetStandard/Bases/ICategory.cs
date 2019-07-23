namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ICategory:ISecurity
    {
        string Description { get; set; }
        string Name { get; set; }
        long Id { get; set; }
        ICategory ParentCategory { get; set; }
    }
}