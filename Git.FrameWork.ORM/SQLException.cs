namespace Git.Framework.ORM
{
    using System;

    public class SQLException : Exception
    {
        public SQLException()
        {
        }

        public SQLException(string message) : base(message)
        {
        }
    }
}

