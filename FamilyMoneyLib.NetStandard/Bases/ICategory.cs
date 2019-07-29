namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ICategory:ISecurity, ITreeNode<ICategory>
    {
        string Description { get; set; }
        string Name { get; set; }
    }
}