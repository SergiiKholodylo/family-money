using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoneyLib.NetStandard.Factories
{
    public class RegularAccountFactory:IAccountFactory
    {
        public IAccount CreateAccount(string name, string description, string currency, long id = 0)
        {
            return new Account(name,description,currency,id);
        }
    }
}
