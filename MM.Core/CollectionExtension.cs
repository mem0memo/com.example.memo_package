using System.Collections.Generic;
using System.Linq;

namespace mm.core
{
    public static class CollectionExtension
    {
        public static T ElementAtOrDefault<T>(this IEnumerable<T> collection, int index)
        {
            var count = collection.Count();
            var clampedIndex = System.Math.Clamp(index, 0, count - 1);
            return count < 1 ? default : collection.ElementAt(clampedIndex);
        }
    }
}
