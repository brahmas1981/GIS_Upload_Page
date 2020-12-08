using System;

namespace GIS_Upload_Page.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] SubArray<T>(this T[] values, int startIndex, int endIndex)
        {
            if (startIndex == 0 && endIndex >= values.Length)
                return values;
            int num_items = endIndex - startIndex + 1;
            T[] result = new T[num_items];
            Array.Copy(values, startIndex, result, 0, num_items);
            return result;
        }

        public static T[] SubArray<T>(this T[] values, double startIndex, double endIndex)
        {
            return values.SubArray<T>((int)startIndex, (int)endIndex);
        }
    }
}
