using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTools.Common.Extensions
{
    public static class TupleExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey key, TValue value)> tuples)
        {
            if (tuples == null) throw new NullReferenceException(nameof(tuples));
            return tuples.ToDictionary(item => item.key, item => item.value);
        }
    }
}
