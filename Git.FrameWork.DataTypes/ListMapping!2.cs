namespace Git.Framework.DataTypes
{
    using Git.Framework.Events;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ListMapping<T1, T2> : IDictionary<T1, GList<T2>>, ICollection<KeyValuePair<T1, GList<T2>>>, IEnumerable<KeyValuePair<T1, GList<T2>>>, IEnumerable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ChangedEventArgs> <Changed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<T1, GList<T2>> <Items>k__BackingField;

        public ListMapping()
        {
            this.Items = new Dictionary<T1, GList<T2>>();
        }

        public virtual void Add(KeyValuePair<T1, GList<T2>> item)
        {
            this.Add(item.Key, item.Value);
        }

        public virtual void Add(T1 Key, T2 Value)
        {
            if (!this.Items.ContainsKey(Key))
            {
                GList<T2> list = new GList<T2> {
                    Changed = this.Changed
                };
                this.Items.Add(Key, list);
            }
            this.Items[Key].Add(Value);
        }

        public virtual void Add(T1 Key, GList<T2> Value)
        {
            if (!this.Items.ContainsKey(Key))
            {
                GList<T2> list = new GList<T2> {
                    Changed = this.Changed
                };
                this.Items.Add(Key, list);
            }
            this.Items[Key].AddRange(Value);
        }

        public virtual void Clear()
        {
            this.Items.Clear();
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
        }

        public virtual bool Contains(KeyValuePair<T1, GList<T2>> item)
        {
            if (!this.ContainsKey(item.Key))
            {
                return false;
            }
            if (!this.Contains(item.Key, item.Value))
            {
                return false;
            }
            return true;
        }

        public virtual bool Contains(T1 Key, GList<T2> Values)
        {
            if (!this.ContainsKey(Key))
            {
                return false;
            }
            foreach (T2 local in Values)
            {
                if (!this.Contains(Key, local))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Contains(T1 Key, T2 Value)
        {
            if (!this.ContainsKey(Key))
            {
                return false;
            }
            if (!this[Key].Contains(Value))
            {
                return false;
            }
            return true;
        }

        public virtual bool ContainsKey(T1 key)
        {
            return this.Items.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<T1, GList<T2>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<T1, GList<T2>>> GetEnumerator()
        {
            using (this.<>s__1 = this.Keys.GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<Key>5__2 = this.<>s__1.Current;
                    yield return new KeyValuePair<T1, GList<T2>>(this.<Key>5__2, this[this.<Key>5__2]);
                    this.<Key>5__2 = default(T1);
                }
            }
            this.<>s__1 = null;
        }

        public virtual bool Remove(T1 key)
        {
            bool flag = this.Items.Remove(key);
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
            return flag;
        }

        public virtual bool Remove(KeyValuePair<T1, GList<T2>> item)
        {
            if (!this.Contains(item))
            {
                return false;
            }
            foreach (T2 local in item.Value)
            {
                if (!this.Remove(item.Key, local))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool Remove(T1 Key, T2 Value)
        {
            if (!this.Contains(Key, Value))
            {
                return false;
            }
            this[Key].Remove(Value);
            if (this[Key].Count == 0)
            {
                this.Remove(Key);
            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            using (this.<>s__1 = this.Keys.GetEnumerator())
            {
                while (this.<>s__1.MoveNext())
                {
                    this.<Key>5__2 = this.<>s__1.Current;
                    yield return this[this.<Key>5__2];
                    this.<Key>5__2 = default(T1);
                }
            }
            this.<>s__1 = null;
        }

        public virtual bool TryGetValue(T1 Key, out GList<T2> Value)
        {
            if (this.ContainsKey(Key))
            {
                Value = this[Key];
                return true;
            }
            Value = new GList<T2>();
            return false;
        }

        public virtual EventHandler<ChangedEventArgs> Changed { get; set; }

        public virtual int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual GList<T2> this[T1 key]
        {
            get
            {
                return this.Items[key];
            }
            set
            {
                this.Items[key] = value;
                EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
            }
        }

        protected virtual Dictionary<T1, GList<T2>> Items { get; set; }

        public virtual ICollection<T1> Keys
        {
            get
            {
                return this.Items.Keys;
            }
        }

        public ICollection<GList<T2>> Values
        {
            get
            {
                GList<GList<T2>> list = new GList<GList<T2>>();
                foreach (T1 local in this.Keys)
                {
                    list.Add(this[local]);
                }
                return list;
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__22 : IEnumerator<KeyValuePair<T1, GList<T2>>>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private KeyValuePair<T1, GList<T2>> <>2__current;
            public ListMapping<T1, T2> <>4__this;
            private IEnumerator<T1> <>s__1;
            private T1 <Key>5__2;

            [DebuggerHidden]
            public <GetEnumerator>d__22(int <>1__state)
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
                            this.<>s__1 = this.<>4__this.Keys.GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<Key>5__2 = this.<>s__1.Current;
                                this.<>2__current = new KeyValuePair<T1, GList<T2>>(this.<Key>5__2, this.<>4__this[this.<Key>5__2]);
                                this.<>1__state = 1;
                                return true;
                            Label_0084:
                                this.<>1__state = -3;
                                this.<Key>5__2 = default(T1);
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

            KeyValuePair<T1, GList<T2>> IEnumerator<KeyValuePair<T1, GList<T2>>>.Current
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
        private sealed class <System-Collections-IEnumerable-GetEnumerator>d__23 : IEnumerator<object>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private object <>2__current;
            public ListMapping<T1, T2> <>4__this;
            private IEnumerator<T1> <>s__1;
            private T1 <Key>5__2;

            [DebuggerHidden]
            public <System-Collections-IEnumerable-GetEnumerator>d__23(int <>1__state)
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
                            this.<>s__1 = this.<>4__this.Keys.GetEnumerator();
                            this.<>1__state = -3;
                            while (this.<>s__1.MoveNext())
                            {
                                this.<Key>5__2 = this.<>s__1.Current;
                                this.<>2__current = this.<>4__this[this.<Key>5__2];
                                this.<>1__state = 1;
                                return true;
                            Label_0079:
                                this.<>1__state = -3;
                                this.<Key>5__2 = default(T1);
                            }
                            this.<>m__Finally1();
                            this.<>s__1 = null;
                            return false;

                        case 1:
                            goto Label_0079;
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

