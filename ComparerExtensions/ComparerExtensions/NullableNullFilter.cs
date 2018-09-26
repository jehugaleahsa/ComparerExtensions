namespace ComparerExtensions
{
    internal sealed class NullableNullFilter<T> : NullFilter<T?>
        where T : struct
    {
        public NullableNullFilter(bool nullsFirst)
            : base(nullsFirst)
        {
        }

        public override int? Filter(T? x, T? y)
        {
            if (x.HasValue)
            {
                if (y.HasValue)
                {
                    return null;
                }
                if (NullsFirst)
                {
                    return 1;
                }
                return -1;
            }
            if (!y.HasValue) return 0;
            return NullsFirst ? -1 : 1;
        }
    }
}
