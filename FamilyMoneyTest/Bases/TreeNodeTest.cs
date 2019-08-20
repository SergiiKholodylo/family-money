using System;
using System.Collections.Generic;
using System.Text;
using FamilyMoneyLib.NetStandard.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Bases
{
    [TestClass]
    public class TreeNodeTest
    {
        [TestMethod]
        public void IsChildTest()
        {
            var tree = new TreeNodeBase<int> {Id = 1};
            var middle = new TreeNodeBase<int> { Id = 3 };
            var child = new TreeNodeBase<int> { Id = 2 };


            tree.AddChild(middle);
            middle.AddChild(child);


            Assert.IsTrue(tree.IsChild(child));
            Assert.IsFalse(child.IsChild(child));
            Assert.IsFalse(tree.IsChild(tree));
        }

        [TestMethod]
        public void IsParentTest()
        {
            var tree = new TreeNodeBase<int> { Id = 1 };
            var middle = new TreeNodeBase<int> { Id = 3 };
            var child = new TreeNodeBase<int> { Id = 2 };


            tree.AddChild(middle);
            middle.AddChild(child);


            Assert.IsTrue(child.IsParent(tree));
            Assert.IsFalse(child.IsParent(child));
            Assert.IsFalse(tree.IsParent(tree));
        }

        [TestMethod]
        public void HasChildTest()
        {
            var tree = new TreeNodeBase<int> { Id = 1 };
            var child = new TreeNodeBase<int> { Id = 2 };


            tree.AddChild(child);


            Assert.IsFalse(child.HasChild);
            Assert.IsTrue(tree.HasChild);
        }
    }
}
