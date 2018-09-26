using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class ReversedComparer<T> : Comparer<T>
    {
        private readonly IComparer<T> _comparer;

        public ReversedComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(T x, T y)
        {
            return _comparer.Compare(y, x);
        }
    }
}