using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public interface IAccountFactory
    {
        IAccount CreateAccount(string name, string description, string currency);
    }
}
