using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Category {Name} {Description}")]
    public class Category : TreeNodeBase<ICategory>,ICategory
    {

        public string Name { set; get; }
        public string Description { set; get; }


        public Category(string name, string description, long id = 0, ICategory parentCategory = null)
        {
            Name = name;
            Description = description;
            Id = id;
            Parent = parentCategory;
            BaseId = Guid.NewGuid();
            OwnerId = Guid.NewGuid();
        }

        public Guid OwnerId { get; set; }
        public Guid BaseId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   Name == category.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
