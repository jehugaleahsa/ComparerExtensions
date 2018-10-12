using System.Collections;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class TypedComparer<T> : IComparer<T>, IComparer
    {
        public static IComparer<T> GetComparer(IComparer comparer)
        {
            if (comparer is UntypedComparer<T> untyped)
            {
                return untyped.Comparer;
            }
            var typed = comparer as IComparer<T>;
            return typed ?? new TypedComparer<T>(comparer);
        }

        private TypedComparer(IComparer comparer)
        {
            Comparer = comparer;
        }

        public IComparer Comparer { get; }

        public int Compare(T x, T y) => Comparer.Compare(x, y);

        int IComparer.Compare(object x, object y) => Comparer.Compare(x, y);
    }
}