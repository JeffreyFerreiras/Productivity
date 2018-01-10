using System;

namespace Tools.DataStructures
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
                return this.LeftChild == null && this.RightChild == null;
            }
        }

        private TreeNode()
        {
        }

        internal TreeNode(T value)
        {
            Value = value;
        }

        internal static TreeNode<T> Create(T[] data)
        {
            return new TreeNode<T>().AddSorted(data, 0, data.Length - 1);
        }

        internal TreeNode<T> AddSorted(T[] data, int low, int high)
        {
            if(low <= high)
            {
                int mid = (low + high) / 2;

                TreeNode<T> node = new TreeNode<T>(data[mid]);

                node.LeftChild = node.AddSorted(data, low, mid - 1);
                node.RightChild = node.AddSorted(data, mid + 1, high);

                return node;
            }

            return null;
        }

        internal void Add(T value)
        {
            this.Add(new TreeNode<T>(value));
        }

        internal void Add(TreeNode<T> node)
        {
            node.Parent = this;

            if(node > this)
            {
                if(this.RightChild == null)
                    this.RightChild = node;
                else
                    this.RightChild.Add(node);
            }
            else if(node < this)
            {
                if(this.LeftChild == null)
                    this.LeftChild = node;
                else
                    this.LeftChild.Add(node);
            }

            Balance();
        }

        internal void Balance(int tension = 2)
        {
            int balance = GetBalance();

            if(balance <= -tension)
            {
                if(this.LeftChild == null)
                {
                    if(this.RightChild.LeftChild != null)
                    {
                        RotateRightLeft();
                    }
                    else if(this.RightChild.RightChild != null)
                    {
                        RotateRight();
                    }
                }
                else
                {
                    RotateRight();
                }
            }
            else if(balance >= tension)
            {
                if(this.RightChild == null)
                {
                    if(this.LeftChild.RightChild != null)
                    {
                        RotateLeftRight();
                    }
                    else if(this.LeftChild.LeftChild != null)
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
            int leftHeight = this.LeftChild?.Height() ?? 0;
            int rightHeight = this.RightChild?.Height() ?? 0;

            int balance = leftHeight - rightHeight;
            return balance;
        }

        private void RotateRight()
        {
            TreeNode<T> rightTemp = this.RightChild;

            SwapValues(this, rightTemp);

            this.RightChild = this.RightChild.RightChild;

            rightTemp.RightChild = rightTemp.LeftChild;
            rightTemp.LeftChild = this.LeftChild;

            this.LeftChild = rightTemp;
        }

        private void RotateLeft()
        {
            TreeNode<T> leftTemp = this.LeftChild;

            SwapValues(this, leftTemp);

            this.LeftChild = this.LeftChild.LeftChild;

            leftTemp.LeftChild = leftTemp.RightChild;
            leftTemp.RightChild = this.RightChild;

            this.RightChild = leftTemp;
        }

        private void RotateLeftRight()
        {
            SwapValues(this, this.LeftChild.RightChild);

            this.RightChild = this.LeftChild.RightChild;
            this.LeftChild.RightChild = null;
        }

        private void RotateRightLeft()
        {
            SwapValues(this, this.RightChild.LeftChild);

            this.LeftChild = this.RightChild.LeftChild;
            this.RightChild.LeftChild = null;
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
            if(this == value && !this.IsDeleted) return this;

            if(this > value && this.LeftChild != null)
            {
                return this.LeftChild.Find(value);
            }

            if(this < value && this.RightChild != null)
            {
                return this.RightChild.Find(value);
            }

            return null;
        }

        internal TreeNode<T> Min()
        {
            if(this.LeftChild == null)
            {
                return this;
            }

            return LeftChild.Min();
        }

        internal TreeNode<T> Max()
        {
            if(this.RightChild == null)
            {
                return this;
            }

            return RightChild.Max();
        }

        internal int Height() //Note: Cannot memoize height because it's always changing...
        {
            if(this.IsLeafNode) return 1;

            int left = this.LeftChild != null ? this.LeftChild.Height() : 0;
            int right = this.RightChild != null ? this.RightChild.Height() : 0;

            return left > right ? left + 1 : right + 1;
        }

        internal int LeafCount()
        {
            if(this.IsLeafNode) return 1;

            int leftLeafCount = LeftChild != null ? LeftChild.LeafCount() : 0;
            int rightLeafCount = RightChild != null ? RightChild.LeafCount() : 0;

            return leftLeafCount + rightLeafCount;
        }

        internal void TraverseInOrder(Action<TreeNode<T>> action)
        {
            this.LeftChild?.TraverseInOrder(action);

            action(this);

            this.RightChild?.TraverseInOrder(action);
        }

        internal void TraversePreOrder(Action<TreeNode<T>> action)
        {
            action(this);

            this.LeftChild?.TraversePreOrder(action);
            this.RightChild?.TraversePreOrder(action);
        }

        internal void TraversePostOrder(Action<TreeNode<T>> action)
        {
            this.LeftChild?.TraversePreOrder(action);
            this.RightChild?.TraversePreOrder(action);

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
            if(left is null && right is null)
                return true;
            if(left is null || right is null)
                return false;

            return left.Value.CompareTo(right.Value) == 0; ;
        }

        public static bool operator !=(TreeNode<T> left, TreeNode<T> right)
        {
            if(left == null && right == null)
                return false;
            if(left == null || right == null)
                return true;

            return left.Value.CompareTo(right.Value) != 0; ;
        }

        public override bool Equals(object other)
        {
            if(other is TreeNode<T> node)
            {
                return this.Value.Equals(node.Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}