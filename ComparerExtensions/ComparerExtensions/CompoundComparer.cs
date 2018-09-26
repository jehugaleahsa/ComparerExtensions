using System.Collections.Generic;

namespace ComparerExtensions
{
    internal sealed class CompoundComparer<T> : Comparer<T>
    {
        private readonly List<IComparer<T>> _comparers;

        public static IComparer<T> GetComparer(IComparer<T> baseComparer, IComparer<T> nextComparer)
        {
            // make sure null comparer stays highest precedence
            if (baseComparer is IPrecedenceEnforcer<T> nullComparer)
            {
                return nullComparer.CreateCompoundComparer(nextComparer);
            }
            var comparer = new CompoundComparer<T>();
            comparer.AppendComparison(baseComparer);
            comparer.AppendComparison(nextComparer);
            return comparer.Normalize();
        }

        public CompoundComparer()
        {
            _comparers = new List<IComparer<T>>();
        }

        public void AppendComparison(IComparer<T> comparer)
        {
            switch (comparer)
            {
                case NullComparer<T> _:
                    return;
                case CompoundComparer<T> other:
                    _comparers.AddRange(other._comparers);
                    return;
            }
            _comparers.Add(comparer);
        }

        public override int Compare(T x, T y)
        {
            foreach (var comparer in _comparers)
            {
                var result = comparer.Compare(x, y);
                if (result != 0)
                {
                    return result;
                }
            }
            return 0;
        }

        public IComparer<T> Normalize()
        {
            switch (_comparers.Count)
            {
                case 0:
                    return NullComparer<T>.Default;
                case 1:
                    return _comparers[0];
                default:
                    return this;
            }
        }
    }
}
