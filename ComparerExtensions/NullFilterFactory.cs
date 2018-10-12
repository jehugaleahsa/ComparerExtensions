using System;
using System.Collections.Generic;
using System.Reflection;

namespace ComparerExtensions
{
    internal static class NullFilterFactory
    {
        private static readonly Dictionary<Type, object[]> filterTypeLookup = new Dictionary<Type, object[]>();

        public static NullFilter<T> GetNullFilter<T>(bool nullsFirst)
        {
            Type comparedType = typeof(T);
            if (!filterTypeLookup.TryGetValue(comparedType, out object[] filters))
            {
                filters = GetTypeFilters<T>(comparedType, filters);
                filterTypeLookup.Add(comparedType, filters);
            }
            return (NullFilter<T>)filters[nullsFirst ? 1 : 0];
        }

        private static object[] GetTypeFilters<T>(Type comparedType, object[] filters)
        {
            var underlyingType = Nullable.GetUnderlyingType(comparedType);
            if (underlyingType != null)
            {
                var genericFilterType = typeof(NullableNullFilter<>);
                var filterType = genericFilterType.MakeGenericType(underlyingType);
                return new[]
                {
                    Activator.CreateInstance(filterType, false),
                    Activator.CreateInstance(filterType, true),
                };
            }
            if (comparedType.GetTypeInfo().IsValueType)
            {
                return new object[2];
            }
            return new object[]
            {
                new ReferenceNullFilter<T>(false),
                new ReferenceNullFilter<T>(true),
            };
        }
    }
}