namespace Git.Framework.DataTypes
{
    using Git.Framework.Events;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class Vector<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ChangedEventArgs> <Changed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <DefaultSize>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <NumberItems>k__BackingField;
        protected T[] Items;

        public Vector()
        {
            this.Items = null;
            this.DefaultSize = 2;
            this.Items = new T[this.DefaultSize];
        }

        public Vector(int InitialSize)
        {
            this.Items = null;
            if (InitialSize < 1)
            {
                throw new ArgumentOutOfRangeException("InitialSize");
            }
            this.DefaultSize = InitialSize;
            this.Items = new T[InitialSize];
        }

        public virtual void Add(T item)
        {
            this.Insert(this.NumberItems, item);
        }

        public virtual void Clear()
        {
            Array.Clear(this.Items, 0, this.Items.Length);
            this.NumberItems = 0;
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
        }

        public virtual bool Contains(T item)
        {
            return (this.IndexOf(item) >= 0);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items, 0, array, arrayIndex, this.NumberItems);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            this.<x>5__1 = 0;
            while (this.<x>5__1 < this.NumberItems)
            {
                yield return this.Items[this.<x>5__1];
                int num2 = this.<x>5__1 + 1;
                this.<x>5__1 = num2;
            }
        }

        public virtual int IndexOf(T item)
        {
            return Array.IndexOf<T>(this.Items, item, 0, this.NumberItems);
        }

        public virtual void Insert(int index, T item)
        {
            if ((index > this.NumberItems) || (index < 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (this.NumberItems == this.Items.Length)
            {
                Array.Resize<T>(ref this.Items, this.Items.Length * 2);
            }
            if (index < this.NumberItems)
            {
                Array.Copy(this.Items, index, this.Items, index + 1, this.NumberItems - index);
            }
            this.Items[index] = item;
            int num = this.NumberItems + 1;
            this.NumberItems = num;
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
        }

        public virtual bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            return false;
        }

        public virtual void RemoveAt(int index)
        {
            if ((index > this.NumberItems) || (index < 0))
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (index < this.NumberItems)
            {
                Array.Copy(this.Items, index + 1, this.Items, index, this.NumberItems - (index + 1));
            }
            this.Items[this.NumberItems - 1] = default(T);
            int num = this.NumberItems - 1;
            this.NumberItems = num;
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.<x>5__1 = 0;
            while (this.<x>5__1 < this.NumberItems)
            {
                yield return this.Items[this.<x>5__1];
                int num2 = this.<x>5__1 + 1;
                this.<x>5__1 = num2;
            }
        }

        public virtual EventHandler<ChangedEventArgs> Changed { get; set; }

        public virtual int Count
        {
            get
            {
                return this.NumberItems;
            }
        }

        protected virtual int DefaultSize { get; set; }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual T this[int index]
        {
            get
            {
                if ((index > this.NumberItems) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return this.Items[index];
            }
            set
            {
                if ((index > this.NumberItems) || (index < 0))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.Items[index] = value;
                EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
            }
        }

        protected virtual int NumberItems { get; set; }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__17 : IEnumerator<T>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private T <>2__current;
            public Vector<T> <>4__this;
            private int <x>5__1;

            [DebuggerHidden]
            public <GetEnumerator>d__17(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<x>5__1 = 0;
                        while (this.<x>5__1 < this.<>4__this.NumberItems)
                        {
                            this.<>2__current = this.<>4__this.Items[this.<x>5__1];
                            this.<>1__state = 1;
                            return true;
                        Label_004E:
                            this.<>1__state = -1;
                            int num2 = this.<x>5__1 + 1;
                            this.<x>5__1 = num2;
                        }
                        return false;

                    case 1:
                        goto Label_004E;
                }
                return false;
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
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
        private sealed class <System-Collections-IEnumerable-GetEnumerator>d__18 : IEnumerator<object>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private object <>2__current;
            public Vector<T> <>4__this;
            private int <x>5__1;

            [DebuggerHidden]
            public <System-Collections-IEnumerable-GetEnumerator>d__18(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private bool MoveNext()
            {
                switch (this.<>1__state)
                {
                    case 0:
                        this.<>1__state = -1;
                        this.<x>5__1 = 0;
                        while (this.<x>5__1 < this.<>4__this.NumberItems)
                        {
                            this.<>2__current = this.<>4__this.Items[this.<x>5__1];
                            this.<>1__state = 1;
                            return true;
                        Label_0053:
                            this.<>1__state = -1;
                            int num2 = this.<x>5__1 + 1;
                            this.<x>5__1 = num2;
                        }
                        return false;

                    case 1:
                        goto Label_0053;
                }
                return false;
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            void IDisposable.Dispose()
            {
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

