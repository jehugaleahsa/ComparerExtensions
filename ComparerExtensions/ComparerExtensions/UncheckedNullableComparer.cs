using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class UncheckedNullableComparer<T> : Comparer<T?>
        where T : struct
    {
        private readonly IComparer<T> _comparer;

        public UncheckedNullableComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(T? x, T? y)
        {
            return _comparer.Compare(x.Value, y.Value);
        }
    }
}