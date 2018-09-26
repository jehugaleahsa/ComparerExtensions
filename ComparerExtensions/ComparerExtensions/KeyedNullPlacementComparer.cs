using System;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class KeyedNullPlacementComparer<T, TKey> : NullPlacementComparer<T, TKey>, IPrecedenceEnforcer<T>
    {
        private readonly Func<T, TKey> _keySelector;

        public static IComparer<T> GetComparer(IComparer<T> comparer, Func<T, TKey> keySelector, bool nullsFirst)
        {
            NullFilter<TKey> filter = NullFilterFactory.GetNullFilter<TKey>(nullsFirst);
            if (filter == null)
            {
                return comparer;
            }
            return new KeyedNullPlacementComparer<T, TKey>(comparer, keySelector, filter);
        }

        internal KeyedNullPlacementComparer(IComparer<T> comparer, Func<T, TKey> keySelector, NullFilter<TKey> filter)
            : base(comparer, filter)
        {
            _keySelector = keySelector;
        }

        public override int Compare(T x, T y)
        {
            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);
            return NullFilter.Filter(xKey, yKey) ?? Comparer.Compare(x, y);
        }

        public IComparer<T> CreateUnkeyedComparer(bool nullsFirst)
        {
            // top-level comparers should be evaluated before keyed comparers
            return UnkeyedNullPlacementComparer<T>.GetComparer(this, nullsFirst);
        }

        public IComparer<T> CreateKeyedComparer<TOtherKey>(Func<T, TOtherKey> keySelector, bool nullsFirst)
        {
            // we don't know if the key selector is identical, so we must wrap ourselves
            return KeyedNullPlacementComparer<T, TOtherKey>.GetComparer(this, keySelector, nullsFirst);
        }

        public IComparer<T> CreateCompoundComparer(IComparer<T> comparer)
        {
            CompoundComparer<T> compoundComparer = new CompoundComparer<T>();
            compoundComparer.AppendComparison(Comparer);
            compoundComparer.AppendComparison(comparer);
            return GetComparer(compoundComparer.Normalize(), _keySelector, NullFilter.NullsFirst);
        }
    }
}