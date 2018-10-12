namespace ComparerExtensions
{
    internal abstract class NullFilter<T>
    {
        protected NullFilter(bool nullsFirst)
        {
            NullsFirst = nullsFirst;
        }

        public bool NullsFirst { get; }

        public abstract int? Filter(T x, T y);
    }
}