using System.Collections.Generic;

namespace ComparerExtensions
{
    internal abstract class NullPlacementComparer<T, TCompared> : Comparer<T>
    {
        protected readonly IComparer<T> Comparer;
        protected readonly NullFilter<TCompared> NullFilter;

        protected NullPlacementComparer(IComparer<T> comparer, NullFilter<TCompared> filter)
        {
            Comparer = comparer;
            NullFilter = filter;
        }
    }
}
