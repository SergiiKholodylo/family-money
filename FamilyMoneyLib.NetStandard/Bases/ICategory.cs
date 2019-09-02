namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ICategory:ISecurity, ITreeNode<ICategory>,IIdAble
    {
        string Description { get; set; }
        string Name { get; set; }
    }
}