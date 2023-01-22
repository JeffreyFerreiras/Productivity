using Productivity.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Productivity.DataStructures
{
    public class AvlTree<T> : ICollection<T> where T : IComparable<T>
    {
        public int Count { get; private set; }

        public TreeNode<T> Root { get; private set; }

        public bool IsBalanced
        {
            get
            {
                int balance = Root.GetBalance();

                return Math.Abs(balance) < 2;
            }
        }

        public bool IsReadOnly => false;

        public AvlTree()
        {
        }

        public AvlTree(T[] data)
        {
            Root = TreeNode<T>.Create(data);
        }

        public void Add(T value)
        {
            if (Root == null)
            {
                Root = new TreeNode<T>(value);
            }
            else
            {
                Root.Add(value);
            }

            Count++;
        }

        public int Height()
        {
            if (Root == null)
                return 0;

            return Root.Height();
        }

        public int LeafCount()
        {
            if (Root == null)
                return 0;

            return Root.LeafCount();
        }

        public TreeNode<T> Find(T value)
        {
            if (Root != null)
            {
                return Root.Find(value);
            }

            return null;
        }

        public void Clear()
        {
            Root = null;
        }

        public bool Contains(T value)
        {
            TreeNode<T> node = Root?.Find(value);

            return node != null;
        }

        /// <summary>
        /// Applies a soft delete to the node with the given value.
        /// </summary>
        /// <param name="value">
        /// </param>
        public void Delete(T value)
        {
            TreeNode<T> node = Root.Find(value);

            if (node != null)
            {
                node.IsDeleted = true;
            }
        }

        public T Min()
        {
            Guard.AssertOperation(Root != null, "Tree not initialized");

            return Root.Min().Value;
        }

        public T Max()
        {
            Guard.AssertOperation(Root != null, "Tree not initialized");

            return Root.Max().Value;
        }

        public void Remove(T value)
        {
            TreeNode<T> node = Root.Find(value);

            if (node == null)
            {
                return; //if node not found get out now.
            }

            if (Root == node)
            {
                /* Find leaf with lowest value on the right sub tree
                 * then assign it as the new root */

                TreeNode<T> minRightNode = Root.RightChild.Min();
                TreeNode<T> minRightNodeParent = minRightNode.Parent;
                TreeNode<T> minRightNodeRightChild = minRightNode.RightChild;

                Root.Value = minRightNode.Value;

                minRightNodeParent.LeftChild = null;

                if (minRightNodeRightChild != null)
                {
                    Root.Add(minRightNodeRightChild);
                }
            }
            else
            {
                TreeNode<T> parent = node.Parent;

                //Remove the appropriate leaf reference from the parent.

                if (parent.LeftChild == node)
                {
                    parent.LeftChild = null;
                }
                else
                {
                    parent.RightChild = null;
                }

                //Re-Add the children nodes, this will also balance the tree.
                Root.Add(node.LeftChild);
                Root.Add(node.RightChild);
            }

            Count--;
        }

        public void TraverseInOrder(Action<TreeNode<T>> action)
        {
            if (Root == null) return;

            Root.LeftChild?.TraverseInOrder(action);

            action(Root);

            Root.RightChild?.TraverseInOrder(action);
        }

        public void TraversePreOrder(Action<TreeNode<T>> action)
        {
            if (Root == null) return;

            action(Root);

            Root.LeftChild?.TraversePreOrder(action);
            Root.RightChild?.TraversePreOrder(action);
        }

        public void TraversePostOrder(Action<TreeNode<T>> action)
        {
            if (Root == null) return;

            Root.LeftChild?.TraversePostOrder(action);
            Root.RightChild?.TraversePostOrder(action);

            action(Root);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int index = arrayIndex;

            void CopyToArray(TreeNode<T> x)
            {
                array[index] = x.Value;
                index++;
            }

            TraverseInOrder(CopyToArray);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var values = new T[Count];

            CopyTo(values, 0);

            foreach (T value in values)
            {
                yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<T>.Remove(T item)
        {
            Remove(item);

            return Contains(item);
        }
    }
}