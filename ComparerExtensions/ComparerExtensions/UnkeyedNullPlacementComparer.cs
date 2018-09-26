using System;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class UnkeyedNullPlacementComparer<T> : NullPlacementComparer<T, T>, IPrecedenceEnforcer<T>
    {
        public static IComparer<T> GetComparer(IComparer<T> comparer, bool nullsFirst)
        {
            var filter = NullFilterFactory.GetNullFilter<T>(nullsFirst);
            return filter == null ? comparer : new UnkeyedNullPlacementComparer<T>(comparer, filter);
        }

        internal UnkeyedNullPlacementComparer(IComparer<T> comparer, NullFilter<T> filter)
            : base(comparer, filter)
        {
        }

        public override int Compare(T x, T y)
        {
            return NullFilter.Filter(x, y) ?? Comparer.Compare(x, y);
        }

        public IComparer<T> CreateUnkeyedComparer(bool nullsFirst)
        {
            // we're duplicating ourselves
            return nullsFirst == NullFilter.NullsFirst ? this : GetComparer(Comparer, nullsFirst);
            // we're replacing ourselves with another top-level comparer with a different sort order
        }

        public IComparer<T> CreateKeyedComparer<TKey>(Func<T, TKey> keySelector, bool nullsFirst)
        {
            // we want to take precedence over the new keyed comparer
            var comparer = KeyedNullPlacementComparer<T, TKey>.GetComparer(Comparer, keySelector, nullsFirst);
            return GetComparer(comparer, NullFilter.NullsFirst);
        }

        public IComparer<T> CreateCompoundComparer(IComparer<T> comparer)
        {
            CompoundComparer<T> compoundComparer = new CompoundComparer<T>();
            compoundComparer.AppendComparison(Comparer);
            compoundComparer.AppendComparison(comparer);
            return GetComparer(compoundComparer.Normalize(), NullFilter.NullsFirst);
        }
    }
}