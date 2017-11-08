using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleTools.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
