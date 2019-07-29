using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ITreeNode<T>
    {
        long Id { set; get; }
        ITreeNode<T> Parent { set; get; }
        List<ITreeNode<T>> Children { get; }

        bool IsChild(ITreeNode<T> category);
        bool IsParent(ITreeNode<T> category);
        int Level();
    }
}
