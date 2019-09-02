namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface IQuickTransaction:ISecurity,IIdAble 
    {
        IAccount Account { get; set; }
        ICategory Category { get; set; }
        string Name { get; set; }
        decimal Total { get; set; }
        decimal Weight { get; set; }
        bool AskForTotal { set; get; }
        bool AskForWeight { set; get; }
    }
}