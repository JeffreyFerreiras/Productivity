using System;
using System.Collections;
using System.Collections.Generic;
using Tools.Exceptions;

namespace Tools.DataStructures
{
    public class AVL_Tree<T> : ICollection<T> where T : IComparable<T>
    {
        public int Count { get; private set; }

        public TreeNode<T> Root { get; private set; }

        public bool IsBalanced
        {
            get
            {
                int balance = this.Root.GetBalance();

                return Math.Abs(balance) < 2;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T value)
        {
            if(Root == null)
            {
                Root = new TreeNode<T>(value);
            }
            else
            {
                Root.Add(value);
            }

            this.Count++;
        }

        public int Height()
        {
            if(this.Root == null)
                return 0;

            return this.Root.Height();
        }

        public int LeafCount()
        {
            if(this.Root == null)
                return 0;

            return this.Root.LeafCount();
        }

        public TreeNode<T> Find(T value)
        {
            if(this.Root != null)
            {
                return this.Root.Find(value);
            }

            return null;
        }

        public void Clear()
        {
            this.Root = null;
        }

        public bool Contains(T value)
        {
            TreeNode<T> node = this.Root?.Find(value);

            return node != null;
        }

        public void Delete(T value)
        {
            TreeNode<T> node = this.Root.Find(value);

            if(node != null)
            {
                node.IsDeleted = true;
            }
        }

        public T Min()
        {
            Guard.AssertOperation(this.Root != null, "Tree not initialized");

            return this.Root.Min().Value;
        }

        public T Max()
        {
            Guard.AssertOperation(this.Root != null, "Tree not initialized");

            return this.Root.Max().Value;
        }

        public void Remove(T value)
        {
            TreeNode<T> node = this.Root.Find(value);

            if(node == null) return;

            if(this.Root == node)
            {
                //Find lowest leaf on right.
                //Assign as root.

                TreeNode<T> minRightNode = this.Root.RightChild.Min();
                TreeNode<T> minRightNodeParent = minRightNode.Parent;
                TreeNode<T> minRightNodeRightChild = minRightNode.RightChild;

                this.Root.Value = minRightNode.Value;
                minRightNodeParent.LeftChild = null;

                if(minRightNodeRightChild != null)
                {
                    this.Root.Add(minRightNodeRightChild);
                }
            }
            else
            {
                TreeNode<T> parent = node.Parent;

                if(parent.LeftChild == node)
                {
                    parent.LeftChild = null;
                }
                else
                {
                    parent.RightChild = null;
                }

                this.Root.Add(node.LeftChild);
                this.Root.Add(node.RightChild);
            }

            this.Count--;
        }

        private TreeNode<T> GetSuccessor(TreeNode<T> node)
        {
            TreeNode<T> current = node.RightChild;
            TreeNode<T> successor = node;
            TreeNode<T> parentOfSuccessor = node;

            while(current != null)
            {
                parentOfSuccessor = successor;
                successor = current;
                current = current.LeftChild;
            }

            if(successor != node.RightChild)
            {
                parentOfSuccessor.LeftChild = successor.RightChild;
                successor.RightChild = node.RightChild;
            }

            return successor;
        }

        public void TraverseInOrder(Action<TreeNode<T>> action)
        {
            if(this.Root == null) return;

            this.Root.LeftChild?.TraverseInOrder(action);

            action(this.Root);

            this.Root.RightChild?.TraverseInOrder(action);
        }

        public void TraversePreOrder(Action<TreeNode<T>> action)
        {
            if(this.Root == null) return;

            action(this.Root);

            this.Root.LeftChild?.TraversePreOrder(action);
            this.Root.RightChild?.TraversePreOrder(action);
        }

        public void TraversePostOrder(Action<TreeNode<T>> action)
        {
            if(this.Root == null) return;

            this.Root.LeftChild?.TraversePostOrder(action);
            this.Root.RightChild?.TraversePostOrder(action);

            action(this.Root);
        }


        public void CopyTo(T[] array, int arrayIndex)
        {
            int index = arrayIndex;

            void copyToArray(TreeNode<T> x)
            {
                array[index] = x.Value;
                index++;
            }

            TraverseInOrder(copyToArray);
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] values = new T[this.Count];

            this.CopyTo(values, 0);
            
            foreach(T value in values)
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
            this.Remove(item);

            return this.Contains(item);
        }
    }
}