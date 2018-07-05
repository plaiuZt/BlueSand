using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Git.Framework.DataTypes.ExtensionMethods
{
    public static class GenericObjectExtensions
    {
        public static T Chain<T>(this T Object, Action<T> Action)
        {
            Action(Object);
            return Object;
        }

        public static R Chain<T, R>(this T Object, Func<T, R> Function)
        {
            return Function(Object);
        }

        public static T Check<T>(this T Object, T DefaultValue)
        {
            return Object.Check<T>((<>c__11<T>.<>9__11_0 ?? (<>c__11<T>.<>9__11_0 = new Predicate<T>(<>c__11<T>.<>9.<Check>b__11_0))), DefaultValue);
        }

        public static T Check<T>(this T Object, Func<T> DefaultValue)
        {
            return Object.Check<T>((<>c__12<T>.<>9__12_0 ?? (<>c__12<T>.<>9__12_0 = new Predicate<T>(<>c__12<T>.<>9.<Check>b__12_0))), DefaultValue);
        }

        public static T Check<T>(this T Object, Predicate<T> Predicate, T DefaultValue)
        {
            return (Predicate(Object) ? Object : DefaultValue);
        }

        public static T Check<T>(this T Object, Predicate<T> Predicate, Func<T> DefaultValue)
        {
            return (Predicate(Object) ? Object : DefaultValue());
        }

        public static T Do<T>(this T Object, Action<T> Action, T DefaultValue)
        {
            if (Object.IsNull() || Action.IsNull())
            {
                return DefaultValue;
            }
            return Object.Chain<T>(Action);
        }

        public static R Do<T, R>(this T Object, Func<T, R> Function, R DefaultValue)
        {
            if (Object.IsNull() || Function.IsNull())
            {
                return DefaultValue;
            }
            return Object.Chain<T, R>(Function);
        }

        public static void Execute(this Action Action, int Attempts = 3, int RetryDelay = 0, int TimeOut = 0x7fffffff)
        {
            Action.ThrowIfNull("Action");
            Exception exception = null;
            long tickCount = Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    Action();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
                if ((Environment.TickCount - tickCount) > TimeOut)
                {
                    break;
                }
                Thread.Sleep(RetryDelay);
                Attempts--;
            }
            if (exception.IsNotNull())
            {
                throw exception;
            }
        }

        public static T Execute<T>(this Func<T> Function, int Attempts = 3, int RetryDelay = 0, int TimeOut = 0x7fffffff)
        {
            Function.ThrowIfNull("Function");
            Exception exception = null;
            long tickCount = Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    return Function();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
                if ((Environment.TickCount - tickCount) > TimeOut)
                {
                    break;
                }
                Thread.Sleep(RetryDelay);
                Attempts--;
            }
            throw exception;
        }

        public static T If<T>(this T Object, Predicate<T> Predicate, T DefaultValue)
        {
            if (Object.IsNull())
            {
                return DefaultValue;
            }
            return (Predicate(Object) ? Object : DefaultValue);
        }

        public static T NotIf<T>(this T Object, Predicate<T> Predicate, T DefaultValue)
        {
            if (Object.IsNull())
            {
                return DefaultValue;
            }
            return (Predicate(Object) ? DefaultValue : Object);
        }

        public static R Return<T, R>(this T Object, Func<T, R> Function, R DefaultValue)
        {
            if (Object.IsNull())
            {
                return DefaultValue;
            }
            R local = Function(Object);
            return (local.IsNull() ? DefaultValue : local);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__11<T>
        {
            public static readonly GenericObjectExtensions.<>c__11<T> <>9;
            public static Predicate<T> <>9__11_0;

            static <>c__11()
            {
                GenericObjectExtensions.<>c__11<T>.<>9 = new GenericObjectExtensions.<>c__11<T>();
            }

            internal bool <Check>b__11_0(T x)
            {
                return (x > null);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__12<T>
        {
            public static readonly GenericObjectExtensions.<>c__12<T> <>9;
            public static Predicate<T> <>9__12_0;

            static <>c__12()
            {
                GenericObjectExtensions.<>c__12<T>.<>9 = new GenericObjectExtensions.<>c__12<T>();
            }

            internal bool <Check>b__12_0(T x)
            {
                return (x > null);
            }
        }
    }
}

