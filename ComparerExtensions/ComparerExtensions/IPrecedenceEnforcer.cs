using System;
using System.Collections.Generic;

namespace ComparerExtensions
{
    internal interface IPrecedenceEnforcer<T>
    {
        IComparer<T> CreateUnkeyedComparer(bool nullsFirst);

        IComparer<T> CreateKeyedComparer<TKey>(Func<T, TKey> keySelector, bool nullsFirst);

        IComparer<T> CreateCompoundComparer(IComparer<T> comparer);
    }
}