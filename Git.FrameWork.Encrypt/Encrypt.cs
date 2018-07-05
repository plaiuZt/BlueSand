using Git.Framework.Resource;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Git.Framework.Encrypt
{


    public class Encrypt
    {
        public static string CreateMD5(string source)
        {
            MD5 md = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            byte[] buffer2 = md.ComputeHash(bytes);
            md.Clear();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer2.Length; i++)
            {
                builder.Append(buffer2[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string MD5Decrypt(string Text)
        {
            return MD5Decrypt(Text, ResourceManager.GetSettingEntity("ENCRYPT_KEY").Value);
        }

        public static string MD5Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int num = Text.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                int num3 = Convert.ToInt32(Text.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte) num3;
            }
            provider.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Encoding.Default.GetString(stream.ToArray());
        }

        public static string MD5Encrypt(string Text)
        {
            return MD5Encrypt(Text, ResourceManager.GetSettingEntity("ENCRYPT_KEY").Value);
        }

        public static string MD5Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(Text);
            provider.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            provider.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            return builder.ToString();
        }

        public static string TripleDESDecrypting(string Source)
        {
            string str;
            try
            {
                byte[] buffer = Convert.FromBase64String(Source);
                byte[] buffer2 = new byte[] { 
                    0x2a, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20, 15, 0xa7, 0x2c, 80, 0x1a, 20, 0x9b, 0x70, 
                    2, 0x5e, 11, 0xcc, 0x77, 0x23, 0xb8, 0xc5
                 };
                byte[] buffer3 = new byte[] { 0x37, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3 };
                ICryptoTransform transform = new TripleDESCryptoServiceProvider { IV = buffer3, Key = buffer2 }.CreateDecryptor();
                MemoryStream stream = new MemoryStream(buffer, 0, buffer.Length);
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                str = new StreamReader(stream2, Encoding.Default).ReadToEnd();
            }
            catch (Exception exception)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + exception.Message);
            }
            return str;
        }

        public static string TripleDESEncrypting(string strSource)
        {
            string str;
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(strSource);
                byte[] buffer2 = new byte[] { 
                    0x2a, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20, 15, 0xa7, 0x2c, 80, 0x1a, 20, 0x9b, 0x70, 
                    2, 0x5e, 11, 0xcc, 0x77, 0x23, 0xb8, 0xc5
                 };
                byte[] buffer3 = new byte[] { 0x37, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3 };
                ICryptoTransform transform = new TripleDESCryptoServiceProvider { IV = buffer3, Key = buffer2 }.CreateEncryptor();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
                stream2.Write(bytes, 0, bytes.Length);
                stream2.FlushFinalBlock();
                str = Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception exception)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + exception.Message);
            }
            return str;
        }
    }
}

