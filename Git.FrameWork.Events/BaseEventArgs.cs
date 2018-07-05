using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.Events
{
    public class BaseEventArgs : EventArgs
    {
        public object Content { get; set; }

        public bool Stop { get; set; }
    }
}

