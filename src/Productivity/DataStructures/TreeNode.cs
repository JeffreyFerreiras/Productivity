using Productivity.Exceptions;
using System;

namespace Productivity.DataStructures
{
    public class TreeNode<T> where T : IComparable<T>
    {
        public T Value { get; set; }

        internal TreeNode<T> Parent { get; private set; }
        internal TreeNode<T> LeftChild { get; set; }
        internal TreeNode<T> RightChild { get; set; }

        internal bool IsDeleted { get; set; }

        public bool IsLeafNode
        {
            get
            {
                return LeftChild == null && RightChild == null;
            }
        }

        private TreeNode()
        {
        }

        internal TreeNode(T value)
        {
            Guard.AssertOperation(value != null, $"{nameof(value)} cannot be null");

            Value = value;
        }

        internal static TreeNode<T> Create(T[] data)
        {
            Array.Sort(data);

            return new TreeNode<T>().AddSorted(data, 0, data.Length - 1);
        }

        internal TreeNode<T> AddSorted(T[] data, int low, int high)
        {
            if (low <= high)
            {
                int mid = (low + high) / 2;

                var node = new TreeNode<T>(data[mid]);

                node.LeftChild = node.AddSorted(data, low, mid - 1);
                node.RightChild = node.AddSorted(data, mid + 1, high);

                return node;
            }

            return null;
        }

        internal void Add(T value)
        {
            Add(new TreeNode<T>(value));
        }

        internal void Add(TreeNode<T> node)
        {
            if (node == null) return;

            node.Parent = this;

            if (node > this)
            {
                if (RightChild == null)
                    RightChild = node;
                else
                    RightChild.Add(node);
            }
            else if (node < this)
            {
                if (LeftChild == null)
                    LeftChild = node;
                else
                    LeftChild.Add(node);
            }

            Balance();
        }

        internal void Balance(int tension = 2)
        {
            int balance = GetBalance();

            if (balance <= -tension)
            {
                if (LeftChild == null)
                {
                    if (RightChild.LeftChild != null)
                    {
                        RotateRightLeft();
                    }
                    else if (RightChild.RightChild != null)
                    {
                        RotateRight();
                    }
                }
                else
                {
                    RotateRight();
                }
            }
            else if (balance >= tension)
            {
                if (RightChild == null)
                {
                    if (LeftChild.RightChild != null)
                    {
                        RotateLeftRight();
                    }
                    else if (LeftChild.LeftChild != null)
                    {
                        RotateLeft();
                    }
                }
                else
                {
                    RotateLeft();
                }
            }
        }

        internal int GetBalance()
        {
            int leftHeight = LeftChild?.Height() ?? 0;
            int rightHeight = RightChild?.Height() ?? 0;

            int balance = leftHeight - rightHeight;
            return balance;
        }

        private void RotateRight()
        {
            TreeNode<T> rightTemp = RightChild;

            SwapValues(this, rightTemp);

            RightChild = RightChild.RightChild;
            RightChild.Parent = this;

            rightTemp.RightChild = rightTemp.LeftChild;
            rightTemp.LeftChild = LeftChild;

            if (rightTemp.LeftChild != null)
            {
                rightTemp.LeftChild.Parent = rightTemp;
            }

            rightTemp.Parent = this;

            LeftChild = rightTemp;
        }

        private void RotateLeft()
        {
            TreeNode<T> leftTemp = LeftChild;

            SwapValues(this, leftTemp);

            LeftChild = LeftChild.LeftChild;
            LeftChild.Parent = this;

            leftTemp.LeftChild = leftTemp.RightChild;
            leftTemp.RightChild = RightChild;

            if (leftTemp.RightChild != null)
            {
                leftTemp.RightChild.Parent = leftTemp;
            }

            leftTemp.Parent = this;

            RightChild = leftTemp;
        }

        internal void RotateLeftRight()
        {
            SwapValues(this, LeftChild.RightChild);

            RightChild = LeftChild.RightChild;
            RightChild.Parent = this;
            LeftChild.RightChild = null;
        }

        internal void RotateRightLeft()
        {
            SwapValues(this, RightChild.LeftChild);

            LeftChild = RightChild.LeftChild;
            LeftChild.Parent = this;
            RightChild.LeftChild = null;
        }

        internal void SwapValues(TreeNode<T> left, TreeNode<T> right)
        {
            T temp = left.Value;
            left.Value = right.Value;
            right.Value = temp;
        }

        internal TreeNode<T> Find(T value)
        {
            return Find(new TreeNode<T>(value));
        }

        internal TreeNode<T> Find(TreeNode<T> value)
        {
            if (this == value && !IsDeleted) return this;

            if (this > value && LeftChild != null)
            {
                return LeftChild.Find(value);
            }

            if (this < value && RightChild != null)
            {
                return RightChild.Find(value);
            }

            return null;
        }

        internal TreeNode<T> Min()
        {
            if (LeftChild == null)
            {
                return this;
            }

            return LeftChild.Min();
        }

        internal TreeNode<T> Max()
        {
            if (RightChild == null)
            {
                return this;
            }

            return RightChild.Max();
        }

        internal int Height() //Note: Cannot memoize height because it's always changing...
        {
            if (IsLeafNode) return 1;

            int left = LeftChild != null ? LeftChild.Height() : 0;
            int right = RightChild != null ? RightChild.Height() : 0;

            return left > right ? left + 1 : right + 1;
        }

        internal int LeafCount()
        {
            int leftLeafCount = LeftChild != null ? 1 : 0;
            int rightLeafCount = RightChild != null ? 1 : 0;

            return leftLeafCount + rightLeafCount;
        }

        internal void TraverseInOrder(Action<TreeNode<T>> action)
        {
            LeftChild?.TraverseInOrder(action);

            action(this);

            RightChild?.TraverseInOrder(action);
        }

        internal void TraversePreOrder(Action<TreeNode<T>> action)
        {
            action(this);

            LeftChild?.TraversePreOrder(action);
            RightChild?.TraversePreOrder(action);
        }

        internal void TraversePostOrder(Action<TreeNode<T>> action)
        {
            LeftChild?.TraversePreOrder(action);
            RightChild?.TraversePreOrder(action);

            action(this);
        }

        public static bool operator >(TreeNode<T> left, TreeNode<T> right)
        {
            return left.Value.CompareTo(right.Value) > 0;
        }

        public static bool operator <(TreeNode<T> left, TreeNode<T> right)
        {
            return left.Value.CompareTo(right.Value) < 0;
        }

        public static bool operator <=(TreeNode<T> left, TreeNode<T> right)
        {
            return left.Value.CompareTo(right.Value) <= 0; ;
        }

        public static bool operator >=(TreeNode<T> left, TreeNode<T> right)
        {
            return left.Value.CompareTo(right.Value) >= 0;
        }

        public static bool operator ==(TreeNode<T> left, TreeNode<T> right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;

            return left.Value.CompareTo(right.Value) == 0; ;
        }

        public static bool operator !=(TreeNode<T> left, TreeNode<T> right)
        {
            if (left == null && right == null)
                return false;
            if (left == null || right == null)
                return true;

            return left.Value.CompareTo(right.Value) != 0; ;
        }

        public override bool Equals(object other)
        {
            if (other is TreeNode<T> node)
            {
                return Value.Equals(node.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}