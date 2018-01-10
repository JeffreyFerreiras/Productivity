using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tools.DataStructures;

namespace Tools.Test.DataStructures
{
    [TestClass]
    public class AVL_TreeTests
    {
        private AVL_Tree<int> BinTreeFactory()
        {
            int[] sample = { 212, 580, 6, 7, 28, 84, 112, 434 };

            var bst = new AVL_Tree<int>();

            foreach(int x in sample)
            {
                bst.Add(x);
            }

            return bst;
        }

        [TestMethod]
        public void ContainsTest()
        {
            var tree = BinTreeFactory();

            Assert.IsTrue(tree.Contains(6));
        }

        [TestMethod]
        public void AddTest_CharTree_BalancedTree()
        {
            var sample = "the quick brown fox jumps over the lazy dog".ToCharArray();
            var bst = new AVL_Tree<char>();

            foreach(var x in sample)
            {
                bst.Add(x);
            }

            Assert.IsTrue(bst.IsBalanced);
        }

        [TestMethod]
        public void AddTest()
        {
            int[] sample = { 212, 580, 6, 7, 28, 84, 112, 434 };

            var bst = new AVL_Tree<int>();

            foreach(int x in sample)
            {
                bst.Add(x);
            }
        }

        [TestMethod]
        public void AddTest_RotateTree_BalancedTree()
        {
            int[] sample = { 43, 18, 22, 9, 21, 6, 8, 20, 63, 50, 62 , 51};

            var bst = new AVL_Tree<int>();

            foreach(int x in sample)
            {
                bst.Add(x);
            }
            
            Assert.IsTrue(bst.IsBalanced);
        }

        [TestMethod]
        public void AddTest_RotateTreeStress_BalancedTree()
        {
            int[] sample = Helper.GetRandomArray(1000);

            var bst = new AVL_Tree<int>();

            foreach(int x in sample)
            {
                bst.Add(x);
            }

            Assert.IsTrue(bst.IsBalanced);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var bst = BinTreeFactory();

            bst.Remove(6);

            Assert.IsFalse(bst.Contains(6));
        }

        [TestMethod]
        public void MinTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.Min() == 6);
        }

        [TestMethod]
        public void MaxTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.Max() == 580);
        }

        [TestMethod]
        public void TraverseInOrderTest()
        {
            var bst = BinTreeFactory();

            Action<TreeNode<int>> action = (x) =>
            {               
                Debug.WriteLine($"Node value: {x.Value} \tIs Leaf: {x.IsLeafNode}");
            };

            bst.TraverseInOrder(action);
        }

        [TestMethod]
        public void LeafCountTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.LeafCount() > 0);
        }
    }
}
