using System;

namespace IrrationalEngineCore.Utils.Extentions
{
    public static class ArrayExtentions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
