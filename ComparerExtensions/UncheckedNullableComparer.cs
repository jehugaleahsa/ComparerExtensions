using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class UncheckedNullableComparer<T> : Comparer<T?>
        where T : struct
    {
        private readonly IComparer<T> comparer;

        public UncheckedNullableComparer(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public override int Compare(T? x, T? y) => comparer.Compare(x.Value, y.Value);
    }
}