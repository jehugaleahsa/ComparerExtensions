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

        public override int Compare(T x, T y) => NullFilter.Filter(x, y) ?? Comparer.Compare(x, y);

        public IComparer<T> CreateUnkeyedComparer(bool nullsFirst)
        {
            // we're duplicating ourselves
            if (nullsFirst == NullFilter.NullsFirst)
            {
                return this;
            }
            // we're replacing ourselves with another top-level comparer with a different sort order
            return GetComparer(Comparer, nullsFirst);
        }

        public IComparer<T> CreateKeyedComparer<TKey>(Func<T, TKey> keySelector, bool nullsFirst)
        {
            // we want to take precedence over the new keyed comparer
            var comparer = KeyedNullPlacementComparer<T, TKey>.GetComparer(Comparer, keySelector, nullsFirst);
            return GetComparer(comparer, NullFilter.NullsFirst);
        }

        public IComparer<T> CreateCompoundComparer(IComparer<T> comparer)
        {
            var compoundComparer = new CompoundComparer<T>();
            compoundComparer.AppendComparison(Comparer);
            compoundComparer.AppendComparison(comparer);
            return GetComparer(compoundComparer.Normalize(), NullFilter.NullsFirst);
        }
    }
}