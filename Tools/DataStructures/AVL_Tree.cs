using System;

namespace Tools.DataStructures
{
    public class AVL_Tree<T> where T : IComparable<T>
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
            if(this.Root == null)
                throw new InvalidOperationException("Tree not initialized");

            return this.Root.Min().Value;
        }

        public T Max()
        {
            if(this.Root == null)
                throw new InvalidOperationException("Tree not initialized");

            return this.Root.Max().Value;
        }

        public void Remove(T value)
        {
            Remove(new TreeNode<T>(value));
        }

        public void Remove(TreeNode<T> value)
        {
            TreeNode<T> parent = this.Root;
            TreeNode<T> current = this.Root;

            bool isLeftChild = false;

            while(current != null && current != value)
            {
                parent = current;

                if(current < value)
                {
                    current = current.RightChild;
                    isLeftChild = false;
                }
                else
                {
                    current = current.LeftChild;
                    isLeftChild = true;
                }
            }

            if(current == null) return;

            if(current.IsLeafNode)
            {
                if(current == this.Root)
                    this.Root = null;
                else
                {
                    if(isLeftChild)
                        parent.LeftChild = null;
                    else
                        parent.RightChild = null;
                }
            }
            else if(current.RightChild == null)
            {
                if(current == this.Root)
                    this.Root = current.LeftChild;
                else if(isLeftChild)
                    parent.LeftChild = current.LeftChild;
                else
                    parent.RightChild = current.LeftChild;
            }
            else if(current.LeftChild == null)
            {
                if(current == this.Root)
                    this.Root = current.RightChild;
                else if(isLeftChild)
                    parent.LeftChild = current.RightChild;
                else
                    parent.RightChild = current.RightChild;
            }
            else
            {
                TreeNode<T> successor = GetSuccessor(current);

                if(current == this.Root)
                    this.Root = successor;
                if(isLeftChild)
                    parent.LeftChild = successor;
                else
                    parent.RightChild = successor;

                successor.LeftChild = current.LeftChild;
            }

            this.Count--;
        }

        private TreeNode<T> GetSuccessor(TreeNode<T> node) //TODO: Method is obsolete, introduced "Parent"
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
    }
}