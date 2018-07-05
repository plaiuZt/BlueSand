using System;

namespace Git.Framework.Json
{
    public class JsonShowModel<T>
    {
        public int count;
        public T list;

        public JsonShowModel(int totalCount, T t)
        {
            this.count = totalCount;
            this.list = t;
        }
    }
}

