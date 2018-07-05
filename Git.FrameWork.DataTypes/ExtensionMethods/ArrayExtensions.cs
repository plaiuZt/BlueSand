using System;
using System.Runtime.CompilerServices;

namespace Git.Framework.DataTypes.ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static Array Clear(this Array Array)
        {
            if (Array.IsNull())
            {
                return null;
            }
            Array.Clear(Array, 0, Array.Length);
            return Array;
        }

        public static ArrayType[] Clear<ArrayType>(this ArrayType[] Array)
        {
            return (ArrayType[]) Array.Clear();
        }

        public static ArrayType[] Combine<ArrayType>(this ArrayType[] Array1, ArrayType[] Array2)
        {
            if (Array1.IsNull() && Array2.IsNull())
            {
                return null;
            }
            int num = (Array1.IsNull() ? 0 : Array1.Length) + (Array2.IsNull() ? 0 : Array2.Length);
            ArrayType[] destinationArray = new ArrayType[num];
            int destinationIndex = 0;
            if (Array1.IsNotNull())
            {
                Array.Copy(Array1, destinationArray, Array1.Length);
                destinationIndex = Array1.Length;
            }
            if (Array2.IsNotNull())
            {
                Array.Copy(Array2, 0, destinationArray, destinationIndex, Array2.Length);
            }
            return destinationArray;
        }
    }
}

