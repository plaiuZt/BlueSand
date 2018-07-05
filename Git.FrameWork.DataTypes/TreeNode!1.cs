namespace Git.Framework.DataTypes
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TreeNode<T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TreeNode<T> <Left>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TreeNode<T> <Parent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TreeNode<T> <Right>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T <Value>k__BackingField;

        public TreeNode(T Value = null, TreeNode<T> Parent = null, TreeNode<T> Left = null, TreeNode<T> Right = null)
        {
            this.Value = Value;
            this.Right = Right;
            this.Left = Left;
            this.Parent = Parent;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public virtual bool IsLeaf
        {
            get
            {
                return ((this.Left == null) && (this.Right == null));
            }
        }

        public virtual bool IsRoot
        {
            get
            {
                return (this.Parent == null);
            }
        }

        public virtual TreeNode<T> Left { get; set; }

        public virtual TreeNode<T> Parent { get; set; }

        public virtual TreeNode<T> Right { get; set; }

        public virtual T Value { get; set; }
    }
}

