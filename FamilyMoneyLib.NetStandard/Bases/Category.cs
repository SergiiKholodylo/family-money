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
        public bool IsChild(ICategory category)
        {
            return category.IsParent(this);
        }

        public bool IsParent(ICategory category)
        {
            ICategory current = this;
            while (current.ParentCategory != null)
            {
                if (current.ParentCategory?.Id == category.Id) return true;
                current = category.ParentCategory;
            }

            return false;
        }

        public int Level()
        {
            var level = 0;
            ICategory category = this;
            while (category.ParentCategory != null)
            {
                level++;
                category = category.ParentCategory;
            }

            return level;
        }

        public string HierarchicalName => new string(' ', Level()*3) + Name;

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
