namespace Git.Framework.DataTypes.ExtensionMethods
{
    using Git.Framework.DataTypes;
    using Git.Framework.DataTypes.Comparison;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> Enumerable1, params IEnumerable<T>[] Additions)
        {
            List<T> list = new List<T>();
            list.AddRange(Enumerable1);
            for (int i = 0; i < Additions.Length; i++)
            {
                list.AddRange(Additions[i]);
            }
            return list;
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> Enumerable, System.Func<T, T, bool> Predicate)
        {
            List<T> list = new List<T>();
            foreach (T local in Enumerable)
            {
                bool flag = false;
                foreach (T local2 in list)
                {
                    if (Predicate(local, local2))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    list.Add(local);
                }
            }
            return list;
        }

        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> List, int Start, int End)
        {
            if (List == null)
            {
                return List;
            }
            if (End > List.Count<T>())
            {
                End = List.Count<T>();
            }
            if (Start < 0)
            {
                Start = 0;
            }
            List<T> list = new List<T>();
            for (int i = Start; i < End; i++)
            {
                list.Add(List.ElementAt<T>(i));
            }
            return list;
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> Value, Func<T, bool> Predicate)
        {
            if (Value == null)
            {
                return Value;
            }
            return (from x in Value
                where !Predicate(x)
                select x);
        }

        public static bool Exists<T>(this IEnumerable<T> List, Predicate<T> Match)
        {
            Match.ThrowIfNull("Match");
            if (!List.IsNull())
            {
                foreach (T local in List)
                {
                    if (Match(local))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static IEnumerable<T> For<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            int num = 0;
            foreach (T local in List)
            {
                if (num.Between<int>(Start, End, null))
                {
                    Action(local);
                }
                num++;
                if (num > End)
                {
                    return List;
                }
            }
            return List;
        }

        public static IEnumerable<R> For<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            int num = 0;
            GList<R> list = new GList<R>();
            foreach (T local in List)
            {
                if (num.Between<int>(Start, End, null))
                {
                    list.Add(Function(local));
                }
                num++;
                if (num > End)
                {
                    break;
                }
            }
            return (IEnumerable<R>) list;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            foreach (T local in List)
            {
                Action(local);
            }
            return List;
        }

        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            GList<R> list = new GList<R>();
            foreach (T local in List)
            {
                list.Add(Function(local));
            }
            return (IEnumerable<R>) list;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> Value)
        {
            return (Value.IsNull() || (Value.Count<T>() == 0));
        }

        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> Value, IEqualityComparer<T> EqualityComparer = null)
        {
            this.<>8__1 = new <>c__DisplayClass10_0<T>();
            this.<>8__1.EqualityComparer = EqualityComparer;
            if (!Value.IsNull())
            {
                this.<>8__1.EqualityComparer = this.<>8__1.EqualityComparer.NullCheck<IEqualityComparer<T>>(new Git.Framework.DataTypes.Comparison.GenericEqualityComparer<T>());
                using (this.<>s__2 = Enumerable.Where<T>(Value, this.<>8__1.<>9__0 ?? (this.<>8__1.<>9__0 = new Func<T, bool>(this.<>8__1.<RemoveDefaults>b__0))).GetEnumerator())
                {
                    while (this.<>s__2.MoveNext())
                    {
                        this.<Item>5__3 = this.<>s__2.Current;
                        yield return this.<Item>5__3;
                        this.<Item>5__3 = default(T);
                    }
                }
                this.<>s__2 = null;
            }
        }

        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            foreach (T local in List)
            {
                if (!Predicate(local))
                {
                    return List;
                }
            }
            throw Exception;
        }

        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            foreach (T local in List)
            {
                if (Predicate(local))
                {
                    throw Exception;
                }
            }
            return List;
        }

        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Func<Exception> Exception)
        {
            foreach (T local in List)
            {
                if (Predicate(local))
                {
                    throw Exception();
                }
            }
            return List;
        }

        public static DataTable To(this IEnumerable List, params string[] Columns)
        {
            PropertyInfo[] Properties;
            DataTable ReturnValue = new DataTable {
                Locale = CultureInfo.CurrentCulture
            };
            int num = 0;
            IEnumerator enumerator = List.GetEnumerator();
            while (enumerator.MoveNext())
            {
                num++;
            }
            if ((List != null) && (num != 0))
            {
                IEnumerator enumerator2 = List.GetEnumerator();
                enumerator2.MoveNext();
                Properties = enumerator2.Current.GetType().GetProperties();
                if (Columns.Length == 0)
                {
                    Columns = Properties.ToArray<PropertyInfo, string>(<>c.<>9__21_0 ?? (<>c.<>9__21_0 = new Func<PropertyInfo, string>(<>c.<>9.<To>b__21_0)));
                }
                Columns.ForEach<string, DataColumn>(x => ReturnValue.Columns.Add(x, Enumerable.FirstOrDefault<PropertyInfo>(Properties, (Func<PropertyInfo, bool>) (z => (z.Name == x))).PropertyType));
                object[] values = new object[Columns.Length];
                foreach (object obj2 in List)
                {
                    int num2;
                    for (int x = 0; x < values.Length; x = num2)
                    {
                        values[x] = Enumerable.FirstOrDefault<PropertyInfo>(Properties, (Func<PropertyInfo, bool>) (z => (z.Name == Columns[x]))).GetValue(obj2, new object[0]);
                        num2 = x + 1;
                    }
                    ReturnValue.Rows.Add(values);
                }
            }
            return ReturnValue;
        }

        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            List.ThrowIfNull("List");
            ConvertingFunction.ThrowIfNull("ConvertingFunction");
            return List.ForEach<Source, Target>(ConvertingFunction).ToArray<Target>();
        }

        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult: class
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            Array.ForEach<PropertyInfo>(type.GetProperties(), delegate (PropertyInfo p) {
                pList.Add(p);
                dt.Columns.Add(p.Name, p.PropertyType);
            });
            using (IEnumerator<TResult> enumerator = value.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TResult item = enumerator.Current;
                    DataRow row = dt.NewRow();
                    pList.ForEach(delegate (PropertyInfo p) {
                        row[p.Name] = p.GetValue(item, null);
                    });
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> List, params string[] Columns)
        {
            PropertyInfo[] Properties;
            DataTable ReturnValue = new DataTable {
                Locale = CultureInfo.CurrentCulture
            };
            if ((List != null) && (List.Count<T>() != 0))
            {
                Properties = typeof(T).GetProperties();
                if (Columns.Length == 0)
                {
                    Columns = Properties.ToArray<PropertyInfo, string>(<>c__20<T>.<>9__20_0 ?? (<>c__20<T>.<>9__20_0 = new Func<PropertyInfo, string>(<>c__20<T>.<>9.<ToDataTable>b__20_0)));
                }
                Columns.ForEach<string, DataColumn>(x => ReturnValue.Columns.Add(x, Enumerable.FirstOrDefault<PropertyInfo>(Properties, (Func<PropertyInfo, bool>) (z => (z.Name == x))).PropertyType));
                object[] values = new object[Columns.Length];
                foreach (T local in List)
                {
                    int num;
                    for (int x = 0; x < values.Length; x = num)
                    {
                        values[x] = Enumerable.FirstOrDefault<PropertyInfo>(Properties, (Func<PropertyInfo, bool>) (z => (z.Name == Columns[x]))).GetValue(local, new object[0]);
                        num = x + 1;
                    }
                    ReturnValue.Rows.Add(values);
                }
            }
            return ReturnValue;
        }

        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult: class, new()
        {
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            Array.ForEach<PropertyInfo>(typeof(TResult).GetProperties(), delegate (PropertyInfo p) {
                if (dt.Columns.IndexOf(p.Name) != -1)
                {
                    prlist.Add(p);
                }
            });
            List<TResult> list = new List<TResult>();
            using (IEnumerator enumerator = dt.Rows.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    DataRow row = (DataRow) enumerator.Current;
                    TResult ob = Activator.CreateInstance<TResult>();
                    prlist.ForEach(delegate (PropertyInfo p) {
                        if (row[p.Name] != DBNull.Value)
                        {
                            p.SetValue(ob, row[p.Name], null);
                        }
                    });
                    list.Add(ob);
                }
            }
            return list;
        }

        public static string ToString<T>(this IEnumerable<T> List, Func<T, string> ItemOutput = null, string Seperator = ",")
        {
            List.ThrowIfNull("List");
            Seperator = Seperator.NullCheck<string>("");
            ItemOutput = ItemOutput.NullCheck<Func<T, string>>(<>c__12<T>.<>9__12_0 ?? (<>c__12<T>.<>9__12_0 = new Func<T, string>(<>c__12<T>.<>9.<ToString>b__12_0)));
            StringBuilder Builder = new StringBuilder();
            string TempSeperator = "";
            List.ForEach<T>(delegate (T x) {
                Builder.Append(TempSeperator).Append(ItemOutput(x));
                TempSeperator = Seperator;
            });
            return Builder.ToString();
        }

        public static bool TrueForAll<T>(this IEnumerable<T> List, Predicate<T> Predicate)
        {
            List.ThrowIfNull("List");
            Predicate.ThrowIfNull("Predicate");
            return !Enumerable.Any<T>(List, (Func<T, bool>) (x => !Predicate(x)));
        }

        public static IEnumerable<T> TryAll<T>(this IEnumerable<T> List, Action<T> Action, Action<T> CatchAction = null)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            foreach (T local in List)
            {
                try
                {
                    Action(local);
                }
                catch
                {
                    if (CatchAction > null)
                    {
                        CatchAction(local);
                    }
                }
            }
            return List;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly IEnumerableExtensions.<>c <>9 = new IEnumerableExtensions.<>c();
            public static Func<PropertyInfo, string> <>9__21_0;

            internal string <To>b__21_0(PropertyInfo x)
            {
                return x.Name;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__12<T>
        {
            public static readonly IEnumerableExtensions.<>c__12<T> <>9;
            public static Func<T, string> <>9__12_0;

            static <>c__12()
            {
                IEnumerableExtensions.<>c__12<T>.<>9 = new IEnumerableExtensions.<>c__12<T>();
            }

            internal string <ToString>b__12_0(T x)
            {
                return x.ToString();
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__20<T>
        {
            public static readonly IEnumerableExtensions.<>c__20<T> <>9;
            public static Func<PropertyInfo, string> <>9__20_0;

            static <>c__20()
            {
                IEnumerableExtensions.<>c__20<T>.<>9 = new IEnumerableExtensions.<>c__20<T>();
            }

            internal string <ToDataTable>b__20_0(PropertyInfo x)
            {
                return x.Name;
            }
        }

        [CompilerGenerated]
        private sealed class <RemoveDefaults>d__10<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator
        {
            private int <>1__state;
            private T <>2__current;
            public IEqualityComparer<T> <>3__EqualityComparer;
            public IEnumerable<T> <>3__Value;
            private IEnumerableExtensions.<>c__DisplayClass10_0<T> <>8__1;
            private int <>l__initialThreadId;
            private IEnumerator<T> <>s__2;
            private T <Item>5__3;
            private IEqualityComparer<T> EqualityComparer;
            private IEnumerable<T> Value;

            [DebuggerHidden]
            public <RemoveDefaults>d__10(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally1()
            {
                this.<>1__state = -1;
                if (this.<>s__2 != null)
                {
                    this.<>s__2.Dispose();
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
                            this.<>8__1 = new IEnumerableExtensions.<>c__DisplayClass10_0<T>();
                            this.<>8__1.EqualityComparer = this.EqualityComparer;
                            if (!this.Value.IsNull())
                            {
                                this.<>8__1.EqualityComparer = this.<>8__1.EqualityComparer.NullCheck<IEqualityComparer<T>>(new Git.Framework.DataTypes.Comparison.GenericEqualityComparer<T>());
                                this.<>s__2 = Enumerable.Where<T>(this.Value, this.<>8__1.<>9__0 ?? (this.<>8__1.<>9__0 = new Func<T, bool>(this.<>8__1.<RemoveDefaults>b__0))).GetEnumerator();
                                this.<>1__state = -3;
                                while (this.<>s__2.MoveNext())
                                {
                                    this.<Item>5__3 = this.<>s__2.Current;
                                    this.<>2__current = this.<Item>5__3;
                                    this.<>1__state = 1;
                                    return true;
                                Label_00F1:
                                    this.<>1__state = -3;
                                    this.<Item>5__3 = default(T);
                                }
                                this.<>m__Finally1();
                                this.<>s__2 = null;
                            }
                            return false;

                        case 1:
                            goto Label_00F1;
                    }
                    return false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                IEnumerableExtensions.<RemoveDefaults>d__10<T> d__;
                if ((this.<>1__state == -2) && (this.<>l__initialThreadId == Thread.CurrentThread.ManagedThreadId))
                {
                    this.<>1__state = 0;
                    d__ = (IEnumerableExtensions.<RemoveDefaults>d__10<T>) this;
                }
                else
                {
                    d__ = new IEnumerableExtensions.<RemoveDefaults>d__10<T>(0);
                }
                d__.Value = this.<>3__Value;
                d__.EqualityComparer = this.<>3__EqualityComparer;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
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
    }
}

