namespace Git.Framework.DataTypes.ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class ICollectionExtensions
    {
        public static T AddAndReturn<T>(this ICollection<T> Collection, T Item)
        {
            Collection.ThrowIfNull("Collection");
            Item.ThrowIfNull("Item");
            Collection.Add(Item);
            return Item;
        }

        public static bool AddIf<T>(this ICollection<T> Collection, T Item, Predicate<T> Predicate)
        {
            Collection.ThrowIfNull("Collection");
            Predicate.ThrowIfNull("Predicate");
            if (!Predicate(Item))
            {
                return false;
            }
            Collection.Add(Item);
            return true;
        }

        public static bool AddIf<T>(this ICollection<T> Collection, IEnumerable<T> Items, Predicate<T> Predicate)
        {
            Collection.ThrowIfNull("Collection");
            Predicate.ThrowIfNull("Predicate");
            bool flag = false;
            foreach (T local in Items)
            {
                flag |= Collection.AddIf<T>(local, Predicate);
            }
            return flag;
        }

        public static bool AddIfUnique<T>(this ICollection<T> Collection, T Item)
        {
            Collection.ThrowIfNull("Collection");
            return Collection.AddIf<T>(Item, x => !Collection.Contains(x));
        }

        public static bool AddIfUnique<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            return Collection.AddIf<T>(Items, x => !Collection.Contains(x));
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            if (!Items.IsNull())
            {
                Items.ForEach<T>(delegate (T x) {
                    Collection.Add(x);
                });
            }
            return Collection;
        }

        public static void AddRange<T>(this ICollection<T> Collection, params T[] items)
        {
            Collection.ThrowIfNull("List");
            items.ThrowIfNull("items");
            items.ForEach<T>(item => Collection.Add(item));
        }

        public static IList<T> For<T>(this IList<T> List, int Start, int End, Action<int, T> Action)
        {
            if (End >= List.Count)
            {
                End = List.Count - 1;
            }
            if (Start < 0)
            {
                Start = 0;
            }
            for (int i = Start; i <= End; i++)
            {
                Action(i, List[i]);
            }
            return List;
        }

        public static IList<R> For<T, R>(this IList<T> List, int Start, int End, Func<int, T, R> Function)
        {
            List<R> list = new List<R>();
            if (End >= List.Count)
            {
                End = List.Count - 1;
            }
            if (Start < 0)
            {
                Start = 0;
            }
            for (int i = Start; i <= End; i++)
            {
                list.Add(Function(i, List[i]));
            }
            return list;
        }

        public static ICollection<T> Remove<T>(this ICollection<T> Collection, Func<T, bool> Predicate)
        {
            Collection.ThrowIfNull("Collection");
            Enumerable.Where<T>(Collection, Predicate).ToList<T>().ForEach(delegate (T x) {
                Collection.Remove(x);
            });
            return Collection;
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            if (!Items.IsNull())
            {
                Items.ForEach<T>(delegate (T x) {
                    Collection.Remove(x);
                });
            }
            return Collection;
        }
    }
}

