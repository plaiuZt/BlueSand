using System;

namespace Git.Framework.Resource
{
    [Serializable]
    public class MessageEntity
    {
        private string _name;
        private string _value;

        public MessageEntity()
        {
        }

        public MessageEntity(string name, string value)
        {
            this._name = name;
            this._value = value;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
    }
}

