using System;
using System.IO;
using System.Net;
using System.Text;

namespace Git.Framework.Encrypt
{
    public class Base64
    {
        public static bool DecodingFileFromFile(string strBase64FileName, string strSaveFileName)
        {
            StreamReader reader = new StreamReader(strBase64FileName, Encoding.ASCII);
            char[] buffer = new char[reader.BaseStream.Length];
            reader.Read(buffer, 0, (int) reader.BaseStream.Length);
            string str = new string(buffer);
            reader.Close();
            return DecodingFileFromString(str, strSaveFileName);
        }

        public static bool DecodingFileFromString(string Base64String, string strSaveFileName)
        {
            FileStream output = new FileStream(strSaveFileName, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(Convert.FromBase64String(Base64String));
            writer.Close();
            output.Close();
            return true;
        }

        public static string DecodingString(string Base64String)
        {
            return DecodingString(Base64String, Encoding.Default);
        }

        public static string DecodingString(string Base64String, Encoding Ens)
        {
            return Ens.GetString(Convert.FromBase64String(Base64String));
        }

        public static bool EncodingFileToFile(string strSourceFileName, string strSaveFileName)
        {
            string str = EncodingFileToString(strSourceFileName);
            StreamWriter writer = new StreamWriter(strSaveFileName);
            writer.Write(str);
            writer.Close();
            return true;
        }

        public static string EncodingFileToString(string strFileName)
        {
            FileStream input = System.IO.File.OpenRead(strFileName);
            BinaryReader reader = new BinaryReader(input);
            string str = Convert.ToBase64String(reader.ReadBytes((int) input.Length));
            reader.Close();
            input.Close();
            return str;
        }

        public static string EncodingString(string SourceString)
        {
            return EncodingString(SourceString, Encoding.Default);
        }

        public static string EncodingString(string SourceString, Encoding Ens)
        {
            return Convert.ToBase64String(Ens.GetBytes(SourceString));
        }

        public static string EncodingWebFile(string strURL)
        {
            return EncodingWebFile(strURL, new WebClient());
        }

        public static string EncodingWebFile(string strURL, WebClient objWebClient)
        {
            return Convert.ToBase64String(objWebClient.DownloadData(strURL));
        }
    }
}

