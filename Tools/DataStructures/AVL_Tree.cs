using System;

namespace Tools.DataStructures
{
    public class AVL_Tree<T> where T : IComparable<T>
    {
        private int _count;
        private TreeNode<T> _root;

        public int Count { get => _count; private set => _count = value; }

        public TreeNode<T> Root { get => _root; private set => _root = value; }

        public bool IsBalanced
        {
            get
            {
                int balance = _root.GetBalance();

                return Math.Abs(balance) < 2;
            }
        }
        public void Add(T value)
        {
            if(_root == null)
            {
                _root = new TreeNode<T>(value);
            }
            else
            {
                _root.Add(value);
            }

            _count++;
        }

        public int Height()
        {
            if(_root == null)
                return 0;
            return _root.Height();
        }

        public int LeafCount()
        {
            if(_root == null)
                return 0;

            return _root.LeafCount();
        }

        public TreeNode<T> Find(T value)
        {
            if(_root != null)
            {
                return _root.Find(value);
            }

            return null;
        }

        public void Clear()
        {
            _root = null;
        }

        public bool Contains(T value)
        {
            TreeNode<T> node = _root?.Find(value);

            return node != null;
        }

        public void Delete(T value)
        {
            TreeNode<T> node = _root.Find(value);

            if(node != null)
            {
                node.IsDeleted = true;
            }
        }

        public T Min()
        {
            if(_root == null)
                throw new InvalidOperationException("Tree not initialized");

            return _root.Min().Value;
        }

        public T Max()
        {
            if(_root == null)
                throw new InvalidOperationException("Tree not initialized");

            return _root.Max().Value;
        }

        public void Remove(T value)
        {
            Remove(new TreeNode<T>(value));
        }

        public void Remove(TreeNode<T> value)
        {
            TreeNode<T> parent = _root;
            TreeNode<T> current = _root;

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
                if(current == _root)
                    _root = null;
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
                if(current == _root)
                    _root = current.LeftChild;
                else if(isLeftChild)
                    parent.LeftChild = current.LeftChild;
                else
                    parent.RightChild = current.LeftChild;
            }
            else if(current.LeftChild == null)
            {
                if(current == _root)
                    _root = current.RightChild;
                else if(isLeftChild)
                    parent.LeftChild = current.RightChild;
                else
                    parent.RightChild = current.RightChild;
            }
            else
            {
                TreeNode<T> successor = GetSuccessor(current);

                if(current == _root)
                    _root = successor;
                if(isLeftChild)
                    parent.LeftChild = successor;
                else
                    parent.RightChild = successor;

                successor.LeftChild = current.LeftChild;
            }

            _count--;
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
            if(_root == null) return;

            _root.LeftChild?.TraverseInOrder(action);

            action(_root);

            _root.RightChild?.TraverseInOrder(action);
        }

        public void TraversePreOrder(Action<TreeNode<T>> action)
        {
            if(_root == null) return;

            action(_root);

            _root.LeftChild?.TraversePreOrder(action);
            _root.RightChild?.TraversePreOrder(action);
        }

        public void TraversePostOrder(Action<TreeNode<T>> action)
        {
            if(_root == null) return;

            _root.LeftChild?.TraversePostOrder(action);
            _root.RightChild?.TraversePostOrder(action);

            action(_root);
        }
    }
}