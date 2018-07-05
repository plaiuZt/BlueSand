namespace Git.Framework.DataTypes.ExtensionMethods
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class StreamExtensions
    {
        public static string ReadAll(this Stream Input, Encoding EncodingUsing = null)
        {
            return Input.ReadAllBinary().ToString(EncodingUsing, 0, -1);
        }

        public static byte[] ReadAllBinary(this Stream Input)
        {
            MemoryStream stream = Input as MemoryStream;
            if (stream > null)
            {
                return stream.ToArray();
            }
            byte[] buffer = new byte[0x400];
            using (MemoryStream stream2 = new MemoryStream())
            {
                int num;
                bool flag3;
                goto Label_0065;
            Label_0031:
                num = Input.Read(buffer, 0, buffer.Length);
                if (num <= 0)
                {
                    return stream2.ToArray();
                }
                stream2.Write(buffer, 0, num);
            Label_0065:
                flag3 = true;
                goto Label_0031;
            }
        }
    }
}

