using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public class Product : IProduct
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public ICategory LastCategory { set; get; }
        public decimal LastTotal { get; set; }
        public decimal LastWeight { get; set; }
    }
}
