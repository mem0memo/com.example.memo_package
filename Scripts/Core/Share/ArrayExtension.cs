
using System.Collections.Generic;

namespace mm
{
    public static class ArrayExtension
    {
        public static T GetValueOrDefault<T>(this ICollection<T> collection, int index)
        {
            if (0 <= index && index < collection.Count)
            {
                var count = 0;
                foreach (var item in collection)
                {
                    if (count == index)
                    {
                        return item;
                    }

                    count++;
                }
            }

            return default;

        }
    }
}
