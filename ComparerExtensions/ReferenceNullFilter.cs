namespace ComparerExtensions
{
    internal sealed class ReferenceNullFilter<T> : NullFilter<T>
    {
        public ReferenceNullFilter(bool nullsFirst)
            : base(nullsFirst)
        {
        }

        public override int? Filter(T x, T y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return NullsFirst ? -1 : 1;
            }
            if (y == null)
            {
                return NullsFirst ? 1 : -1;
            }
            return null;
        }
    }
}