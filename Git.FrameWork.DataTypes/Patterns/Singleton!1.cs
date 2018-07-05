namespace Git.Framework.DataTypes.Patterns
{
    using System;
    using System.Reflection;

    public class Singleton<T> where T: class
    {
        private static T _Instance;

        static Singleton()
        {
            Singleton<T>._Instance = default(T);
        }

        protected Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (Singleton<T>._Instance == null)
                {
                    Type type = typeof(T);
                    lock (type)
                    {
                        ConstructorInfo info = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                        if ((info == null) || info.IsAssembly)
                        {
                            throw new Exception("Constructor is not private or protected for type " + typeof(T).Name);
                        }
                        Singleton<T>._Instance = (T) info.Invoke(null);
                    }
                }
                return Singleton<T>._Instance;
            }
        }
    }
}

