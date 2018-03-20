using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.DataStructures;
using Tools.Extensions.Collection;
using Tools.Extensions.Validation;

namespace Tools.Test.DataStructures
{
    [TestFixture]
    public class AVL_TreeTests
    {
        private static readonly int[] s_sample = { 212, 580, 6, 7, 28, 84, 112, 434 };

        private AVL_Tree<int> BinTreeFactory()
        {
            var bst = new AVL_Tree<int>();

            foreach(int x in s_sample)
            {
                bst.Add(x);
            }

            return bst;
        }

        [Test]
        public void Contains_SampleData_ReturnsTrue()
        {
            var tree = BinTreeFactory();

            Assert.IsTrue(tree.Contains(s_sample.Min()));
        }

        [Test]
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

        [Test]
        public void AddTest_RotateTree_BalancedTree()
        {
            int[] sample = { 43, 18, 22, 9, 21, 6, 8, 20, 63, 50, 62, 51 };

            var bst = new AVL_Tree<int>();

            foreach(int x in sample)
            {
                bst.Add(x);
            }

            Assert.IsTrue(bst.IsBalanced);
        }

        [Test]
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

        [Theory]
        [TestCase(6)]
        [TestCase(112)]
        [TestCase(212)]
        public void RemovesNodeIndifferentLevels(int num)
        {
            var bst = BinTreeFactory();
            int countCache = bst.Count; //Keep track of count so we make sure to only remove one.

            bst.Remove(num);

            Assert.IsTrue(countCache == bst.Count + 1);
            Assert.IsFalse(bst.Contains(num));
            Assert.IsTrue(bst.IsSortedAsc());
        }

        [Test]
        public void MinTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.Min() == 6);
        }

        [Test]
        public void MaxTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.Max() == s_sample.Max());
        }

        [Test]
        public void TraverseInOrderTest()
        {
            var bst = BinTreeFactory();

            Action<TreeNode<int>> action = (x) =>
            {
                Debug.WriteLine($"Node value: {x.Value} \tIs Leaf: {x.IsLeafNode}");
            };

            bst.TraverseInOrder(action);
        }

        [Test]
        public void LeafCountTest()
        {
            var bst = BinTreeFactory();

            Assert.IsTrue(bst.LeafCount() > 0);
        }

        [Test]
        public void Delete_ValidInput_DeletesNode()
        {
            var bst = BinTreeFactory();
            bst.Delete(6);

            Assert.IsFalse(bst.Contains(6));
        }

        [Test]
        public void CopyTo_ValidInput_Copies()
        {
            var bst = BinTreeFactory();
            int[] values = new int[bst.Count];

            bst.CopyTo(values, 0);

            Assert.IsTrue(values.IsSortedAsc());
        }

        [Test]
        public void CopyTo_SmallArray_Copies()
        {
            var bst = BinTreeFactory();
            int[] values = new int[bst.Count - 1];

            Assert.Throws<IndexOutOfRangeException>(() => bst.CopyTo(values, 0));
        }

        [Test]
        public void CopyTo_OutOfRange_Throws()
        {
            var bst = BinTreeFactory();
            int[] values = new int[bst.Count + 2];

            Assert.Throws<IndexOutOfRangeException>(() => bst.CopyTo(values, 3));
        }

        [Test]
        public void GetEnumerator_ValidInput_Iterates()
        {
            var bst = BinTreeFactory();
            var values = new List<int>();

            foreach(var item in bst)
            {
                values.Add(item);
            }

            Assert.IsTrue(values.IsSortedAsc());
        }

        [Test]
        public void GetEnumerator_UsesIEnumerableExtensions()
        {
            var bst = BinTreeFactory();

            var where = bst.Where(x => x > 10);

            Assert.True(bst.ToArray().Length == bst.Count);
            Assert.True(bst.ToList().Count == bst.Count);
        }
    }
}