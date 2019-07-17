using System.Diagnostics;


namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Category {Name} {Description}")]
    public class Category : ICategory
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        internal Category()
        {
        }

        internal Category(string name, string description, long id = 0)
        {
            Name = name;
            Description = description;
            Id = id;
        }

    }
}
