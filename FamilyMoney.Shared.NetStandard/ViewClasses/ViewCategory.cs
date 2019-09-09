using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewClasses
{
    public class ViewCategory:Category
    {
        public ViewCategory(string name, string description, long id = 0, ICategory parentCategory = null) : base(name, description, id, parentCategory)
        {
        }

        public string HierarchicalName => new string(' ', Level() * 3) + Name;
    }
}
