using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Git.Framework.Json
{
    public class ObjectSerialization
    {
        private object _entity;
        private string _jsonData;

        public ObjectSerialization()
        {
        }

        public ObjectSerialization(object entity)
        {
            this._entity = entity;
        }

        public string EntityToJson()
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(this.Entity.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, this.Entity);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0L;
            stream.Read(buffer, 0, (int) stream.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        public string EntityToJson(object entity)
        {
            this._entity = entity;
            return this.EntityToJson();
        }

        public T GetObjectJson<T>()
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(this.JsonData));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            return (T) serializer.ReadObject(stream);
        }

        public T GetObjectJson<T>(string jsonData)
        {
            this._jsonData = jsonData;
            return this.GetObjectJson<T>();
        }

        public object Entity
        {
            get
            {
                return this._entity;
            }
            set
            {
                this._entity = value;
            }
        }

        public string JsonData
        {
            get
            {
                return this._jsonData;
            }
            set
            {
                this._jsonData = value;
            }
        }
    }
}

