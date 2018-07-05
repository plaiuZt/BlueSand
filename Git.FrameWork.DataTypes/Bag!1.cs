namespace Git.Framework.DataTypes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Bag<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<T, int> <Items>k__BackingField;

        public Bag()
        {
            this.Items = new Dictionary<T, int>();
        }

        public virtual void Add(T item)
        {
            if (this.Items.ContainsKey(item))
            {
                T local = item;
                Dictionary<T, int> items = this.Items;
                int num = items[local] + 1;
                items[local] = num;
            }
            else
            {
                this.Items.Add(item, 1);
            }
        }

        public virtual void Clear()
        {
            this.Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this.Items.ContainsKey(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            using (this.<>s__1 = this.Items.Keys.GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<Key>5__2 = this.<>s__1.Current;
                    yield return this.<Key>5__2;
                    this.<Key>5__2 = default(T);
                }
            }
            this.<>s__1 = new Dictionary<T, int>.KeyCollection.Enumerator();
        }

        public virtual bool Remove(T item)
        {
            return this.Items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (this.<>s__1 = this.Items.Keys.GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<Key>5__2 = this.<>s__1.Current;
                    yield return this.<Key>5__2;
                    this.<Key>5__2 = default(T);
                }
            }
            this.<>s__1 = new Dictionary<T, int>.KeyCollection.Enumerator();
        }

        public virtual int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual int this[T index]
        {
            get
            {
                return this.Items[index];
            }
            set
            {
                this.Items[index] = value;
            }
        }

        protected virtual Dictionary<T, int> Items { get; set; }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__10 : IEnumerator<T>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private T <>2__current;
            public Bag<T> <>4__this;
            private Dictionary<T, int>.KeyCollection.Enumerator <>s__1;
            private T <Key>5__2;

            [DebuggerHidden]
            public <GetEnumerator>d__10(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                this.<>s__1.Dispose();
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>s__1 = this.<>4__this.Items.Keys.GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<Key>5__2 = this.<>s__1.Current;
                                this.<>2__current = this.<Key>5__2;
                                this.<>1__state = 1;
                                return true;
                            Label_0073:
                                this.<>1__state = -3;
                                this.<Key>5__2 = default(T);
                            }
                            this.<>m__Finally1();
                            this.<>s__1 = new Dictionary<T, int>.KeyCollection.Enumerator();
                            return false;

                        case 1:
                            goto Label_0073;
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
        private sealed class <System-Collections-IEnumerable-GetEnumerator>d__11 : IEnumerator<object>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private object <>2__current;
            public Bag<T> <>4__this;
            private Dictionary<T, int>.KeyCollection.Enumerator <>s__1;
            private T <Key>5__2;

            [DebuggerHidden]
            public <System-Collections-IEnumerable-GetEnumerator>d__11(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                this.<>s__1.Dispose();
            }

            private bool MoveNext()
            {
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>s__1 = this.<>4__this.Items.Keys.GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<Key>5__2 = this.<>s__1.Current;
                                this.<>2__current = this.<Key>5__2;
                                this.<>1__state = 1;
                                return true;
                            Label_0078:
                                this.<>1__state = -3;
                                this.<Key>5__2 = default(T);
                            }
                            this.<>m__Finally1();
                            this.<>s__1 = new Dictionary<T, int>.KeyCollection.Enumerator();
                            return false;

                        case 1:
                            goto Label_0078;
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
    }
}

