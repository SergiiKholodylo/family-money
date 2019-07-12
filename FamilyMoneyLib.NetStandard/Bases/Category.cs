using System.Diagnostics;

namespace FamilyMoneyLib.NetStandard.Bases
{

    [DebuggerDisplay("Id {Id}{ParentCategory} {Name}")]
    public class Category : IIdBased
    {

        public static long NewCategoryId { get; } = -1;
        public long Id { set; get; }
        public Category ParentCategory { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }

        public Category()
        {
            Id = NewCategoryId;
        }

        public bool HasCategoryAsParent(Category parent)
        {
            var position = this;
            while (position.ParentCategory != null)
            {
                if (position.ParentCategory == parent) return true;
                position = position.ParentCategory;
            }
            return false;
        }

        public int GetCategoryLevel()
        {
            var level = 1;
            var position = this;
            while (position.ParentCategory != null)
            {
                position = position.ParentCategory;
                level++;
            }
            return level;

        }
    }
}
