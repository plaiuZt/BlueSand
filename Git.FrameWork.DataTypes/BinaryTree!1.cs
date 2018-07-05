namespace Git.Framework.DataTypes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class BinaryTree<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T: IComparable<T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NumberOfNodes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TreeNode<T> <Root>k__BackingField;

        public BinaryTree(TreeNode<T> Root = null)
        {
            if (Root == null)
            {
                this.NumberOfNodes = 0;
            }
            else
            {
                this.Root = Root;
                foreach (TreeNode<T> node in this.Traversal(Root))
                {
                    int num = this.NumberOfNodes + 1;
                    this.NumberOfNodes = num;
                }
            }
        }

        public virtual void Add(T item)
        {
            if (this.Root == null)
            {
                this.Root = new TreeNode<T>(item, null, null, null);
                int num = this.NumberOfNodes + 1;
                this.NumberOfNodes = num;
            }
            else
            {
                this.Insert(item);
            }
        }

        public virtual void Clear()
        {
            this.Root = null;
            this.NumberOfNodes = 0;
        }

        public virtual bool Contains(T item)
        {
            if (!this.IsEmpty)
            {
                TreeNode<T> root = this.Root;
                while (root > null)
                {
                    int num = root.Value.CompareTo(item);
                    if (num == 0)
                    {
                        return true;
                    }
                    if (num < 0)
                    {
                        root = root.Left;
                    }
                    else
                    {
                        root = root.Right;
                    }
                }
            }
            return false;
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            T[] sourceArray = new T[this.NumberOfNodes];
            int index = 0;
            foreach (T local in this)
            {
                sourceArray[index] = local;
                index++;
            }
            Array.Copy(sourceArray, 0, array, arrayIndex, this.NumberOfNodes);
        }

        protected virtual TreeNode<T> Find(T item)
        {
            foreach (TreeNode<T> node in this.Traversal(this.Root))
            {
                if (node.Value.Equals(item))
                {
                    return node;
                }
            }
            return null;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            using (this.<>s__1 = this.Traversal(this.Root).GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<TempNode>5__2 = this.<>s__1.Current;
                    yield return this.<TempNode>5__2.Value;
                    this.<TempNode>5__2 = null;
                }
            }
            this.<>s__1 = null;
        }

        protected virtual void Insert(T item)
        {
            TreeNode<T> root = this.Root;
            bool flag = false;
            while (!flag)
            {
                int num2;
                int num = root.Value.CompareTo(item);
                if (num > 0)
                {
                    if (root.Left == null)
                    {
                        root.Left = new TreeNode<T>(item, root, null, null);
                        num2 = this.NumberOfNodes + 1;
                        this.NumberOfNodes = num2;
                        break;
                    }
                    root = root.Left;
                }
                else if (num < 0)
                {
                    if (root.Right == null)
                    {
                        root.Right = new TreeNode<T>(item, root, null, null);
                        num2 = this.NumberOfNodes + 1;
                        this.NumberOfNodes = num2;
                        break;
                    }
                    root = root.Right;
                }
                else
                {
                    root = root.Right;
                }
            }
        }

        public virtual bool Remove(T item)
        {
            TreeNode<T> node = this.Find(item);
            if (node == null)
            {
                return false;
            }
            int num = this.NumberOfNodes - 1;
            this.NumberOfNodes = num;
            GList<T> list = new GList<T>();
            foreach (TreeNode<T> node2 in this.Traversal(node.Left))
            {
                list.Add(node2.Value);
            }
            foreach (TreeNode<T> node3 in this.Traversal(node.Right))
            {
                list.Add(node3.Value);
            }
            if (node.Parent > null)
            {
                if (node.Parent.Left == node)
                {
                    node.Parent.Left = null;
                }
                else
                {
                    node.Parent.Right = null;
                }
                node.Parent = null;
            }
            else
            {
                this.Root = null;
            }
            foreach (T local in list)
            {
                this.Add(local);
            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (this.<>s__1 = this.Traversal(this.Root).GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<TempNode>5__2 = this.<>s__1.Current;
                    yield return this.<TempNode>5__2.Value;
                    this.<TempNode>5__2 = null;
                }
            }
            this.<>s__1 = null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (T local in this)
            {
                builder.Append(local.ToString() + " ");
            }
            return builder.ToString();
        }

        protected virtual IEnumerable<TreeNode<T>> Traversal(TreeNode<T> Node)
        {
            if (Node > null)
            {
                if (Node.Left > null)
                {
                    using (this.<>s__1 = this.Traversal(Node.Left).GetEnumerator())
                    {
                        while (this.<>s__1.MoveNext())
                        {
                            this.<LeftNode>5__2 = this.<>s__1.Current;
                            yield return this.<LeftNode>5__2;
                            this.<LeftNode>5__2 = null;
                        }
                    }
                    this.<>s__1 = null;
                }
                yield return Node;
                if (Node.Right > null)
                {
                    using (this.<>s__3 = this.Traversal(Node.Right).GetEnumerator())
                    {
                        while (this.<>s__3.MoveNext())
                        {
                            this.<RightNode>5__4 = this.<>s__3.Current;
                            yield return this.<RightNode>5__4;
                            this.<RightNode>5__4 = null;
                        }
                    }
                    this.<>s__3 = null;
                }
            }
        }

        public virtual int Count
        {
            get
            {
                return this.NumberOfNodes;
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return (this.Root == null);
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual T MaxValue
        {
            get
            {
                if (this.IsEmpty)
                {
                    throw new Exception("The tree is empty");
                }
                TreeNode<T> root = this.Root;
                while (root.Right > null)
                {
                    root = root.Right;
                }
                return root.Value;
            }
        }

        public virtual T MinValue
        {
            get
            {
                if (this.IsEmpty)
                {
                    throw new Exception("The tree is empty");
                }
                TreeNode<T> root = this.Root;
                while (root.Left > null)
                {
                    root = root.Left;
                }
                return root.Value;
            }
        }

        protected virtual int NumberOfNodes { get; set; }

        public virtual TreeNode<T> Root { get; set; }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__15 : IEnumerator<T>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private T <>2__current;
            public BinaryTree<T> <>4__this;
            private IEnumerator<TreeNode<T>> <>s__1;
            private TreeNode<T> <TempNode>5__2;

            [DebuggerHidden]
            public <GetEnumerator>d__15(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                if (this.<>s__1 != null)
                {
                    this.<>s__1.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>s__1 = this.<>4__this.Traversal(this.<>4__this.Root).GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<TempNode>5__2 = this.<>s__1.Current;
                                this.<>2__current = this.<TempNode>5__2.Value;
                                this.<>1__state = 1;
                                return true;
                            Label_007F:
                                this.<>1__state = -3;
                                this.<TempNode>5__2 = null;
                            }
                            this.<>m__Finally1();
                            this.<>s__1 = null;
                            return false;

                        case 1:
                            goto Label_007F;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case -3:
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally1();
                        }
                        break;
                }
            }

            T IEnumerator<T>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <System-Collections-IEnumerable-GetEnumerator>d__16 : IEnumerator<object>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private object <>2__current;
            public BinaryTree<T> <>4__this;
            private IEnumerator<TreeNode<T>> <>s__1;
            private TreeNode<T> <TempNode>5__2;

            [DebuggerHidden]
            public <System-Collections-IEnumerable-GetEnumerator>d__16(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                if (this.<>s__1 != null)
                {
                    this.<>s__1.Dispose();
                }
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>s__1 = this.<>4__this.Traversal(this.<>4__this.Root).GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<TempNode>5__2 = this.<>s__1.Current;
                                this.<>2__current = this.<TempNode>5__2.Value;
                                this.<>1__state = 1;
                                return true;
                            Label_0084:
                                this.<>1__state = -3;
                                this.<TempNode>5__2 = null;
                            }
                            this.<>m__Finally1();
                            this.<>s__1 = null;
                            return false;

                        case 1:
                            goto Label_0084;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case -3:
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally1();
                        }
                        break;
                }
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Traversal>d__27 : IEnumerable<TreeNode<T>>, IEnumerable, IEnumerator<TreeNode<T>>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private TreeNode<T> <>2__current;
            public TreeNode<T> <>3__Node;
            public BinaryTree<T> <>4__this;
            private int <>l__initialThreadId;
            private IEnumerator<TreeNode<T>> <>s__1;
            private IEnumerator<TreeNode<T>> <>s__3;
            private TreeNode<T> <LeftNode>5__2;
            private TreeNode<T> <RightNode>5__4;
            private TreeNode<T> Node;

            [DebuggerHidden]
            public <Traversal>d__27(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                if (this.<>s__1 != null)
                {
                    this.<>s__1.Dispose();
                }
            }

            private void <>m__Finally2()
            {
                this.<>1__state = -1;
                if (this.<>s__3 != null)
                {
                    this.<>s__3.Dispose();
                }
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.Node <= null)
                            {
                                goto Label_01A1;
                            }
                            if (this.Node.Left > null)
                            {
                                this.<>s__1 = this.<>4__this.Traversal(this.Node.Left).GetEnumerator();
                                this.<>1__state = -3;
                                while (this.<>s__1.MoveNext())
                                {
                                    this.<LeftNode>5__2 = this.<>s__1.Current;
                                    this.<>2__current = this.<LeftNode>5__2;
                                    this.<>1__state = 1;
                                    return true;
                                Label_00BD:
                                    this.<>1__state = -3;
                                    this.<LeftNode>5__2 = null;
                                }
                                this.<>m__Finally1();
                                this.<>s__1 = null;
                            }
                            this.<>2__current = this.Node;
                            this.<>1__state = 2;
                            return true;

                        case 1:
                            goto Label_00BD;

                        case 2:
                            break;

                        case 3:
                            goto Label_0175;

                        default:
                            return false;
                    }
                    this.<>1__state = -1;
                    if (this.Node.Right > null)
                    {
                        this.<>s__3 = this.<>4__this.Traversal(this.Node.Right).GetEnumerator();
                        this.<>1__state = -4;
                        while (this.<>s__3.MoveNext())
                        {
                            this.<RightNode>5__4 = this.<>s__3.Current;
                            this.<>2__current = this.<RightNode>5__4;
                            this.<>1__state = 3;
                            return true;
                        Label_0175:
                            this.<>1__state = -4;
                            this.<RightNode>5__4 = null;
                        }
                        this.<>m__Finally2();
                        this.<>s__3 = null;
                    }
                Label_01A1:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TreeNode<T>> IEnumerable<TreeNode<T>>.GetEnumerator()
            {
                BinaryTree<T>.<Traversal>d__27 d__;
                if ((this.<>1__state == -2) && (this.<>l__initialThreadId == Thread.CurrentThread.ManagedThreadId))
                {
                    this.<>1__state = 0;
                    d__ = (BinaryTree<T>.<Traversal>d__27) this;
                }
                else
                {
                    d__ = new BinaryTree<T>.<Traversal>d__27(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.Node = this.<>3__Node;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Git.Framework.DataTypes.TreeNode<T>>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case -3:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally1();
                        }
                        break;

                    case 3:
                    case -4:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally2();
                        }
                        break;
                }
            }

            TreeNode<T> IEnumerator<TreeNode<T>>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

