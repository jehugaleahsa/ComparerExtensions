using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class ReversedComparer<T> : Comparer<T>
    {
        private readonly IComparer<T> comparer;

        public ReversedComparer(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public override int Compare(T x, T y) => comparer.Compare(y, x);
    }
}