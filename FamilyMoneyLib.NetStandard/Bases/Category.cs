using System.Diagnostics;


namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Category {Name} {Description}")]
    public class Category : ICategory
    {
        public string Name { set; get; }
        public string Description { set; get; }

        internal Category()
        {
        }

        internal Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}
