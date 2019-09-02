using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMoneyLib.NetStandard.Bases
{
    public interface ITreeNode<T>:IIdAble
    {
        ITreeNode<T> Parent { set; get; }
        List<ITreeNode<T>> Children { get; }

        bool HasChild { get; }

        bool IsChild(ITreeNode<T> category);
        bool IsParent(ITreeNode<T> category);

        void AddChild(ITreeNode<T> t);


        int Level();
    }
}
