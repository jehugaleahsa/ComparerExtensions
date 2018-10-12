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
                return NullsFirst ? 1 : -1;
            }
            if (y.HasValue)
            {
                return NullsFirst ? -1 : 1;
            }
            return 0;
        }
    }
}
