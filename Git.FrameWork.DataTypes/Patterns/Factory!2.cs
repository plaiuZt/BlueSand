namespace Git.Framework.DataTypes.Patterns
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class Factory<Key, T>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<Key, Func<T>> <Constructors>k__BackingField;

        public Factory()
        {
            this.Constructors = new Dictionary<Key, Func<T>>();
        }

        public virtual T Create(Key Key)
        {
            if (this.Exists(Key))
            {
                return this.Constructors[Key]();
            }
            return default(T);
        }

        public virtual bool Exists(Key Key)
        {
            return this.Constructors.ContainsKey(Key);
        }

        public virtual void Register(Key Key, T Result)
        {
            if (this.Exists(Key))
            {
                this.Constructors[Key] = () => Result;
            }
            else
            {
                this.Constructors.Add(Key, () => Result);
            }
        }

        public virtual void Register(Key Key, Func<T> Constructor)
        {
            if (this.Exists(Key))
            {
                this.Constructors[Key] = Constructor;
            }
            else
            {
                this.Constructors.Add(Key, Constructor);
            }
        }

        protected virtual Dictionary<Key, Func<T>> Constructors { get; set; }
    }
}

