using System;

namespace Git.Framework.Events
{
    public static class EventHelper
    {
        public static void Raise<T>(T EventArgs, Action<T> Delegate) where T: class
        {
            if (Delegate != null)
            {
                Delegate(EventArgs);
            }
        }

        public static T2 Raise<T1, T2>(T1 EventArgs, Func<T1, T2> Delegate) where T1: class
        {
            if (Delegate != null)
            {
                return Delegate(EventArgs);
            }
            return default(T2);
        }

        public static void Raise<T>(EventHandler<T> Delegate, object Sender, T EventArg) where T: EventArgs
        {
            if (Delegate != null)
            {
                Delegate(Sender, EventArg);
            }
        }
    }
}

