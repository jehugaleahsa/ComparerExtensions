using System.Collections.Generic;

namespace ComparerExtensions
{
    internal abstract class NullPlacementComparer<T, TCompared> : Comparer<T>
    {
        protected NullPlacementComparer(IComparer<T> comparer, NullFilter<TCompared> filter)
        {
            Comparer = comparer;
            NullFilter = filter;
        }

        protected IComparer<T> Comparer { get; }

        protected NullFilter<TCompared> NullFilter { get; }
    }
}
