namespace Git.Framework.DataTypes
{
    using System;
    using System.Threading;

    public class Rand
    {
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
            {
                Thread.Sleep(3);
            }
            string str = "";
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                str = str + random.Next(10).ToString();
            }
            return str;
        }

        public static string Str(int Length)
        {
            return Str(Length, false);
        }

        public static string Str(int Length, bool Sleep)
        {
            if (Sleep)
            {
                Thread.Sleep(3);
            }
            char[] chArray = new char[] { 
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 
                'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
                'W', 'X', 'Y', 'Z'
             };
            string str = "";
            int length = chArray.Length;
            Random random = new Random(~((int) DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int index = random.Next(0, length);
                str = str + chArray[index].ToString();
            }
            return str;
        }

        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep)
            {
                Thread.Sleep(3);
            }
            char[] chArray = new char[] { 
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
             };
            string str = "";
            int length = chArray.Length;
            Random random = new Random(~((int) DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int index = random.Next(0, length);
                str = str + chArray[index].ToString();
            }
            return str;
        }
    }
}

