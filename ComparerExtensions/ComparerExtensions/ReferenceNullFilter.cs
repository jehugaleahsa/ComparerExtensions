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
            if (x != null)
            {
                if (y != null) return null;
                if (NullsFirst)
                {
                    return 1;
                }
                return -1;
            }
            if (y == null)
            {
                return 0;
            }
            if (NullsFirst)
            {
                return -1;
            }
            return 1;
        }
    }
}