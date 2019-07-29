using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{

    public class TreeNodeBase<T> : ITreeNode<T>
    {
        private readonly List<ITreeNode<T>> _children = new List<ITreeNode<T>>();
        public long Id { get; set; }

        public ITreeNode<T> Parent { get; set; }

        public List<ITreeNode<T>> Children => _children;

        public bool IsChild(ITreeNode<T> category)
        {
            return category.IsParent(this);
        }

        public bool IsParent(ITreeNode<T> category)
        {
            ITreeNode<T> current = this;
            while (current.Parent != null)
            {
                if (current.Parent?.Id == category.Id) return true;
                current = category.Parent;
            }

            return false;
        }

        public int Level()
        {
            var level = 0;
            ITreeNode<T> category = this;
            while (category.Parent != null)
            {
                level++;
                category = category.Parent;
            }

            return level;
        }

    }
}
