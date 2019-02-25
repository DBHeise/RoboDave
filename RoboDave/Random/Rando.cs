using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RoboDave
{
    public static class Rando
    {
        private static System.Random rnd;

        static Rando()
        {
            rnd = new System.Random();
        }

        public static Boolean RandomBoolean()
        {
            return (GetBytes(1)[0] % 2 == 0);
        }
        public static Boolean RandomBoolean(byte OutOf)
        {
            return (GetBytes(1)[0] % (OutOf + 1) == 0);
        }
        public static Double RandomDouble()
        {
            return Generate<Double>();
        }

        public static Double RandomDouble(double min, double max)
        {
            return (min + ((double)Generate<UInt64>() / UInt64.MaxValue) * (max - min));
        }

        public static float RandomFloat()
        {
            return Generate<Single>();
        }
        public static float RandomFloat(float min, float max)
        {
            return (min + ((float)Generate<UInt32>() / UInt32.MaxValue) * (max - min));
        }

        public static int RandomInt(int min, int max)
        {
            return (Int32)(min + ((double)Generate<UInt64>() / UInt64.MaxValue) * (max - min));
        }

        public static T RandomPick<T>(IEnumerable<T> array)
        {
            var index = RandomInt(0, array.Count());
            return array.ElementAt(index);
        }

        public static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = RandomInt(0, n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static object GetBasicType(Type basicType)
        {
            switch (basicType.Name)
            {
                case "bool":
                case "Boolean":
                    return RandomBoolean();

                case "byte":
                case "Byte":
                    return GetBytes(1)[0];

                case "sbyte":
                case "SByte":
                    return Convert.ToSByte((byte)GetBasicType(typeof(byte)) - Byte.MaxValue / 2);

                case "short":
                case "Int16":
                    return BitConverter.ToInt16(GetBytes(2), 0);

                case "ushort":
                case "UInt16":
                    return BitConverter.ToUInt16(GetBytes(2), 0);

                case "int":
                case "Int32":
                    return BitConverter.ToInt32(GetBytes(4), 0);

                case "uint":
                case "UInt32":
                    return BitConverter.ToUInt32(GetBytes(4), 0);

                case "long":
                case "Int64":
                    return BitConverter.ToInt64(GetBytes(8), 0);

                case "ulong":
                case "UInt64":
                    return BitConverter.ToUInt64(GetBytes(8), 0);

                case "float":
                case "Single":
                    return BitConverter.ToSingle(GetBytes(4), 0);

                case "double":
                case "Double":
                    return BitConverter.ToDouble(GetBytes(8), 0);

                case "string":
                case "String":
                    return BitConverter.ToString(GetBytes(RandomInt(0, 10)));

                case "char":
                case "Char":
                    return BitConverter.ToChar(GetBytes(2), 0);
            }
            throw new NotSupportedException();
        }

        public static byte[] GetBytes(UInt64 length)
        {
            RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[length];
            randomNumGen.GetBytes(randomBytes);
            return randomBytes;
        }
        public static byte[] GetBytes(int length)
        {
            RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[length];
            randomNumGen.GetBytes(randomBytes);
            return randomBytes;
        }
        public static T Generate<T>()
        {
            Type itemType = typeof(T);
            return (T)Generate(itemType);
        }

        public static object Generate(Type itemType)
        {
            return GetBasicType(itemType);
        }
    }
}
