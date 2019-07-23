using System;
using System.Diagnostics;


namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Category {Name} {Description}")]
    public class Category : ICategory
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public ICategory ParentCategory { set; get; }

        internal Category()
        {
        }

        internal Category(string name, string description, long id = 0, ICategory parentCategory = null)
        {
            Name = name;
            Description = description;
            Id = id;
            ParentCategory = parentCategory;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }
    }
}
