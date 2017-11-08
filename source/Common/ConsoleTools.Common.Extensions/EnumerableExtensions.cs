using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTools.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public delegate bool EqualityComparerDelegate<in T>(T a, T b);
        public delegate int HashCodeGeneratorDelegate<in T>(T obj);

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> items, EqualityComparerDelegate<T> comparer)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            return items.Distinct(new AnonymousEqualityComparer<T>(comparer));
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> items, HashCodeGeneratorDelegate<T> hashGenerator)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (hashGenerator == null) throw new ArgumentNullException(nameof(hashGenerator));

            return items.Distinct(new AnonymousEqualityComparer<T>(hashGenerator));
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items) action(item);
        }

        public static T FirstOfType<T>(this IEnumerable baseItems)
        {
            if (baseItems == null) throw new ArgumentNullException(nameof(baseItems));
            return baseItems.OfType<T>().First();
        }

        public static T FirstOfTypeOrDefault<T>(this IEnumerable baseItems)
        {
            if (baseItems == null) throw new ArgumentNullException(nameof(baseItems));
            return baseItems.OfType<T>().FirstOrDefault();
        }

        private struct AnonymousEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly EqualityComparerDelegate<T> _equalsDelegate;
            private readonly HashCodeGeneratorDelegate<T> _hashCodeGeneratorDelegate;

            public AnonymousEqualityComparer(EqualityComparerDelegate<T> equals) : this(equals, obj => obj.GetHashCode()) { }
            public AnonymousEqualityComparer(HashCodeGeneratorDelegate<T> hashGenerator) : this((a, b) => hashGenerator(a) == hashGenerator(b), hashGenerator) { }
            public AnonymousEqualityComparer(EqualityComparerDelegate<T> equals, HashCodeGeneratorDelegate<T> hashGenerator)
            {
                this._equalsDelegate = equals;
                this._hashCodeGeneratorDelegate = hashGenerator;
            }

            #region Implementation of IEqualityComparer<in T>
            public bool Equals(T x, T y) => this._equalsDelegate(x, y);
            public int GetHashCode(T obj) => this._hashCodeGeneratorDelegate(obj);
            #endregion Implementation of IEqualityComparer<in T>
        }
    }
}
