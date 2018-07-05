using Git.Framework.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Git.Framework.MsSql
{

    public class ObjectXmlSerializer
    {
        private static Git.Framework.Log.Log log = Git.Framework.Log.Log.Instance(typeof(ObjectXmlSerializer));
        private const string LogCategory = "Framework.ObjectXmlSerializer";
        private const int LogEventLoadFileException = 1;

        public static T DataContractDeserializer<T>(string xmlData) where T: class
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlData));
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            T local = (T) serializer.ReadObject(reader, true);
            reader.Close();
            stream.Close();
            return local;
        }

        public static string DataContractSerializer<T>(T myObject) where T: class
        {
            MemoryStream stream = new MemoryStream();
            new System.Runtime.Serialization.DataContractSerializer(typeof(T)).WriteObject(stream, myObject);
            stream.Close();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static T LoadFromXml<T>(string fileName) where T: class
        {
            return LoadFromXml<T>(fileName, true);
        }

        public static T LoadFromXml<T>(string fileName, bool loggingEnabled) where T: class
        {
            FileStream stream = null;
            T local;
            try
            {
                log.Info("反序列化:" + fileName);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                local = (T) serializer.Deserialize(stream);
            }
            catch (Exception exception)
            {
                log.Info("反序列化异常:" + exception.Message);
                if (!loggingEnabled)
                {
                    throw;
                }
                LogLoadFileException(fileName, exception);
                local = default(T);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return local;
        }

        [Conditional("TRACE")]
        private static void LogLoadFileException(string fileName, Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Fail to load xml file: ");
            builder.Append(fileName + Environment.NewLine);
            builder.Append(ex.ToString());
        }

        [Conditional("TRACE")]
        private static void LogSaveFileException(string fileName, Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Fail to save xml file: ");
            builder.Append(fileName + Environment.NewLine);
            builder.Append(ex.ToString());
        }

        public static void SaveToXml<T>(string fileName, T data) where T: class
        {
            SaveToXml<T>(fileName, data, true);
        }

        public static void SaveToXml<T>(string fileName, T data, bool loggingEnabled) where T: class
        {
            FileStream stream = null;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                serializer.Serialize((Stream) stream, data);
            }
            catch (Exception exception)
            {
                if (!loggingEnabled)
                {
                    throw;
                }
                LogSaveFileException(fileName, exception);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public static T XmlDeserialize<T>(string str) where T: class
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            StreamReader textReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(str)), Encoding.UTF8);
            return (T) serializer.Deserialize(textReader);
        }

        public static string XmlSerializer<T>(T serialObject) where T: class
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            MemoryStream w = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(w, Encoding.UTF8);
            serializer.Serialize((XmlWriter) writer, serialObject);
            writer.Close();
            return Encoding.UTF8.GetString(w.ToArray());
        }
    }
}

