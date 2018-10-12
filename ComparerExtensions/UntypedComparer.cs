using System.Collections;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class UntypedComparer<T> : IComparer<T>, IComparer
    {
        public static IComparer GetComparer(IComparer<T> comparer)
        {
            switch (comparer)
            {
                case TypedComparer<T> typed: return typed.Comparer;
                case IComparer untyped: return untyped;
                default: return new UntypedComparer<T>(comparer);
            }
        }

        private UntypedComparer(IComparer<T> comparer)
        {
            Comparer = comparer;
        }

        public IComparer<T> Comparer { get; }

        public int Compare(T x, T y) => Comparer.Compare(x, y);

        int IComparer.Compare(object x, object y) => Comparer.Compare((T) x, (T) y);
    }
}
