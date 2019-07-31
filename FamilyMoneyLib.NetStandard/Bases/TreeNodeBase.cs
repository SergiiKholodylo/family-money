using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyMoneyLib.NetStandard.Bases
{

    public class TreeNodeBase<T> : ITreeNode<T>
    {
        public long Id { get; set; }

        public ITreeNode<T> Parent { get; set; }

        public List<ITreeNode<T>> Children { get; } = new List<ITreeNode<T>>();

        public bool IsChild(ITreeNode<T> node)
        {
            return node.IsParent(this);
        }

        public bool IsParent(ITreeNode<T> node)
        {
            ITreeNode<T> current = this;
            while (current.Parent != null)
            {
                if (current.Parent?.Id == node.Id) return true;
                current = node.Parent;
            }

            return false;
        }

        public void AddChild(ITreeNode<T> t)
        {
            if(Id == t.Id) throw new ArgumentException($"Could not add child to himself! Ids are equal");
            t.Parent = this;
            Children.Add(t);
        }

        public bool HasChild => Children.Any();

        public int Level()
        {
            var level = 0;
            ITreeNode<T> node = this;
            while (node.Parent != null)
            {
                level++;
                node = node.Parent;
            }

            return level;
        }

    }
}
