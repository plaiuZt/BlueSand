namespace Git.Framework.DataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class PriorityQueue<T> : ListMapping<int, T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <HighestKey>k__BackingField;

        public PriorityQueue()
        {
            this.HighestKey = -2147483648;
        }

        public override void Add(KeyValuePair<int, GList<T>> item)
        {
            if (item.Key > this.HighestKey)
            {
                this.HighestKey = item.Key;
            }
            base.Add(item);
        }

        public override void Add(int Priority, GList<T> Value)
        {
            if (Priority > this.HighestKey)
            {
                this.HighestKey = Priority;
            }
            base.Add(Priority, Value);
        }

        public override void Add(int Priority, T Value)
        {
            if (Priority > this.HighestKey)
            {
                this.HighestKey = Priority;
            }
            base.Add(Priority, Value);
        }

        public virtual T Peek()
        {
            if (this.Items.ContainsKey(this.HighestKey))
            {
                return this.Items[this.HighestKey][0];
            }
            return default(T);
        }

        public virtual T Pop()
        {
            T local = default(T);
            if (this.Items.ContainsKey(this.HighestKey) && (this.Items[this.HighestKey].Count > 0))
            {
                local = this.Items[this.HighestKey][0];
                this.Remove(this.HighestKey, local);
                if (this.ContainsKey(this.HighestKey))
                {
                    return local;
                }
                this.HighestKey = -2147483648;
                foreach (int num in this.Items.Keys)
                {
                    if (num > this.HighestKey)
                    {
                        this.HighestKey = num;
                    }
                }
            }
            return local;
        }

        protected virtual int HighestKey { get; set; }
    }
}

