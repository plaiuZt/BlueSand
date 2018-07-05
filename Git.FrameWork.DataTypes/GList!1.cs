namespace Git.Framework.DataTypes
{
    using Git.Framework.Events;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class GList<T> : List<T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<ChangedEventArgs> <Changed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PropertyName>k__BackingField;

        public void Add(T value)
        {
            base.Add(value);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public void AddRange(IEnumerable<T> value)
        {
            base.AddRange(value);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public void Clear()
        {
            base.Clear();
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public void Insert(int index, T value)
        {
            base.Insert(index, value);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public bool Remove(T obj)
        {
            bool flag = base.Remove(obj);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
            return flag;
        }

        public int RemoveAll(Predicate<T> match)
        {
            int num = base.RemoveAll(match);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
            return num;
        }

        public void RemoveAt(int index)
        {
            base.RemoveAt(index);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            ChangedEventArgs eventArg = new ChangedEventArgs {
                Content = this.PropertyName
            };
            EventHelper.Raise<ChangedEventArgs>(this.Changed, this, eventArg);
        }

        public virtual EventHandler<ChangedEventArgs> Changed { get; set; }

        public T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                EventHelper.Raise<ChangedEventArgs>(this.Changed, this, new ChangedEventArgs());
            }
        }

        public virtual string PropertyName { get; set; }
    }
}

