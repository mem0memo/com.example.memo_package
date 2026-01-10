using System.Collections.Generic;

namespace mm.core
{
    public static class ArrayExtensions
    {
        public static T GetValueOrDefault<T>(this IList<T> array, int index)
        {
            if (index >= 0 && index < array.Count)
            {
                return array[index];
            }

            return default(T);
        }
    }
}