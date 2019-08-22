using System.Collections.Generic;
using System.Linq;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.ViewModels.NetStandard.Helpers
{
    public static class TransactionHelpers
    {
        public static IEnumerable<ITransaction> GetSuggestions(IEnumerable<ITransaction> transactions, string template)
        {
            return transactions.Where(x => x.Name.Contains(template) && !x.IsComplexTransaction);
        }
    }
}
