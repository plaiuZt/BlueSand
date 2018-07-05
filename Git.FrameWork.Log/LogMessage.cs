using System;

namespace Git.Framework.Log
{
    public class LogMessage
    {
        private string _content;
        private DateTime _time;
        private MessageType _type;

        public LogMessage() : this("", MessageType.success)
        {
        }

        public LogMessage(string content, MessageType type) : this(DateTime.Now, content, type)
        {
            this._content = content;
            this._type = type;
        }

        public LogMessage(DateTime time, string content, MessageType type)
        {
            this._time = time;
            this._content = content;
            this._type = type;
        }

        public string Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
            }
        }

        public MessageType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

