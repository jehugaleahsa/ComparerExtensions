using System;
using System.Collections;
using System.Collections.Generic;

namespace ComparerExtensions
{
    /// <summary>
    /// Provides methods for composing comparisons of a given type.
    /// </summary>
    public static class ComparerExtensions
    {
        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the comparer.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="comparer">The comparer to use if two items compare as equal using the base comparer.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The comparer is null.</exception>
        public static IComparer<T> ThenBy<T>(this IComparer<T> baseComparer, IComparer<T> comparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="comparison">The comparison to use if two items compare as equal using the base comparer.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T>(this IComparer<T> baseComparer, Func<T, T, int> comparison)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }
            IComparer<T> wrapper = ComparisonWrapper<T>.GetComparer(comparison);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, wrapper);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderBy(keySelector);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparer">The comparer to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderBy(keySelector, keyComparer);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenBy<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector, Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderBy(keySelector, keyComparison);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderByDescending(keySelector);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparer">The comparer to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparer);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Composes a comparer that performs subsequent ordering using the key comparison, in descending order.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="baseComparer">The comparer to extend.</param>
        /// <param name="keySelector">The key of the type to use for comparison.</param>
        /// <param name="keyComparison">The comparison delegate to use to compare the keys.</param>
        /// <returns>A comparer that performs comparisons using both comparison operations.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparison delegate is null.</exception>
        public static IComparer<T> ThenByDescending<T, TKey>(this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T> comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparison);
            IComparer<T> compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
            return compoundComparer;
        }

        /// <summary>
        /// Extends a non-nullable comparer to work against nullable values by specifying
        /// nulls should be treated as the largest possible value.
        /// </summary>
        /// <typeparam name="T">The non-nullable type being compared by the base comparer.</typeparam>
        /// <param name="baseComparer">The comparer used to compare non-nullable values.</param>
        /// <returns>A new comparer capable of comparing a nullable version fo the value type.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        public static IComparer<T?> ToNullableComparer<T>(this IComparer<T> baseComparer)
            where T : struct
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IComparer<T?> uncheckedComparer = new UncheckedNullableComparer<T>(baseComparer);
            return UnkeyedNullPlacementComparer<T?>.GetComparer(uncheckedComparer, nullsFirst: true);
        }

        /// <summary>
        /// Extends a comparer such that null values are handled up-front by treating them
        /// as the smallest possible value.
        /// </summary>
        /// <typeparam name="T">The type of the values being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend to handle nulls.</param>
        /// <returns>A new comparer that treats nulls as the smallest possible value.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <remarks>The returned comparer is guaranteed to never pass a null value to the given base comparer.</remarks>
        public static IComparer<T> NullsFirst<T>(this IComparer<T> baseComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IPrecedenceEnforcer<T> previous = baseComparer as IPrecedenceEnforcer<T>;
            if (previous != null)
            {
                return previous.CreateUnkeyedComparer(nullsFirst: true);
            }
            return UnkeyedNullPlacementComparer<T>.GetComparer(baseComparer, nullsFirst: true);
        }

        /// <summary>
        /// Extends a comparer such that null values are handled up-front by treating them
        /// as the largest possible value.
        /// </summary>
        /// <typeparam name="T">The type of the values being compared.</typeparam>
        /// <param name="baseComparer">The comparer to extend to handle nulls.</param>
        /// <returns>A new comparer that treats nulls as the largest possible value.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <remarks>The returned comparer is guaranteed to never pass a null value to the given base comparer.</remarks>
        public static IComparer<T> NullsLast<T>(this IComparer<T> baseComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            IPrecedenceEnforcer<T> previous = baseComparer as IPrecedenceEnforcer<T>;
            if (previous != null)
            {
                return previous.CreateUnkeyedComparer(nullsFirst: false);
            }
            return UnkeyedNullPlacementComparer<T>.GetComparer(baseComparer, nullsFirst: false);
        }

        /// <summary>
        /// Extends a comparer such that null values are handled up-front by treating them
        /// as the smallest possible value.
        /// </summary>
        /// <typeparam name="T">The type of the values being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the selector.</typeparam>
        /// <param name="baseComparer">The comparer to extend to handle nulls.</param>
        /// <param name="keySelector">Returns the key that will be inspected for null values.</param>
        /// <returns>A new comparer that treats nulls as the smallest possible value.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <remarks>The returned comparer is guaranteed to never pass a null value to the given base comparer.</remarks>
        public static IComparer<T> NullsFirst<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            IPrecedenceEnforcer<T> previous = baseComparer as IPrecedenceEnforcer<T>;
            if (previous != null)
            {
                return previous.CreateKeyedComparer(keySelector, nullsFirst: true);
            }
            return KeyedNullPlacementComparer<T, TKey>.GetComparer(baseComparer, keySelector, nullsFirst: true);
        }

        /// <summary>
        /// Extends a comparer such that null values are handled up-front by treating them
        /// as the largest possible value.
        /// </summary>
        /// <typeparam name="T">The type of the values being compared.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the selector.</typeparam>
        /// <param name="baseComparer">The comparer to extend to handle nulls.</param>
        /// <param name="keySelector">Returns the key that will be inspected for null values.</param>
        /// <returns>A new comparer that treats nulls as the largest possible value.</returns>
        /// <exception cref="System.ArgumentNullException">The base comparer is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <remarks>The returned comparer is guaranteed to never pass a null value to the given base comparer.</remarks>
        public static IComparer<T> NullsLast<T, TKey>(this IComparer<T> baseComparer, Func<T, TKey> keySelector)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException("baseComparer");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            IPrecedenceEnforcer<T> previous = baseComparer as IPrecedenceEnforcer<T>;
            if (previous != null)
            {
                return previous.CreateKeyedComparer(keySelector, nullsFirst: false);
            }
            return KeyedNullPlacementComparer<T, TKey>.GetComparer(baseComparer, keySelector, nullsFirst: false);
        }

        /// <summary>
        /// Creates a comparer that reverses the results of the comparer.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="comparer">The comparer to reverse.</param>
        /// <returns>A comparer that reverses the results of the comparer.</returns>
        /// <exception cref="System.ArgumentNullException">The comparer is null.</exception>
        public static IComparer<T> Reversed<T>(this IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return new ReversedComparer<T>(comparer);
        }

        /// <summary>
        /// Makes a typed comparer from the given comparer.
        /// </summary>
        /// <typeparam name="T">The type of items the comparer should compare.</typeparam>
        /// <param name="comparer">The comparer to convert to a typed comparer.</param>
        /// <returns>A typed comparer.</returns>
        /// <exception cref="System.ArgumentNullException">The comparer is null.</exception>
        public static IComparer<T> Typed<T>(this IComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return TypedComparer<T>.GetComparer(comparer);
        }

        /// <summary>
        /// Makes an un-typed comparer from the given comparer.
        /// </summary>
        /// <typeparam name="T">The type that the given comparer compares.</typeparam>
        /// <param name="comparer">The comparer to make un-typed.</param>
        /// <returns>The untyped comparer.</returns>
        public static IComparer Untyped<T>(this IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return UntypedComparer<T>.GetComparer(comparer);
        }

        /// <summary>
        /// Converts the given comparison function to a comparer object.
        /// </summary>
        /// <typeparam name="T">The type of the items that are compared.</typeparam>
        /// <param name="comparison">The comparison function to convert.</param>
        /// <returns>A new comparer that wraps the comparison function.</returns>
        /// <exception cref="System.ArgumentNullException">The comparison function is null.</exception>
        public static IComparer<T> ToComparer<T>(this Func<T, T, int> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }
            return ComparisonWrapper<T>.GetComparer(comparison);
        }
    }

    internal sealed class TypedComparer<T> : IComparer<T>, IComparer
    {
        private readonly IComparer _comparer;

        public static IComparer<T> GetComparer(IComparer comparer)
        {
            UntypedComparer<T> untyped = comparer as UntypedComparer<T>;
            if (untyped != null)
            {
                return untyped.Comparer;
            }
            IComparer<T> typed = comparer as IComparer<T>;
            if (typed != null)
            {
                return typed;
            }
            return new TypedComparer<T>(comparer);
        }

        private TypedComparer(IComparer comparer)
        {
            _comparer = comparer;
        }

        public IComparer Comparer
        {
            get { return _comparer; }
        }

        public int Compare(T x, T y)
        {
            return _comparer.Compare(x, y);
        }

        int IComparer.Compare(object x, object y)
        {
            return _comparer.Compare(x, y);
        }
    }

    internal sealed class UntypedComparer<T> : IComparer<T>, IComparer
    {
        private readonly IComparer<T> _comparer;

        public static IComparer GetComparer(IComparer<T> comparer)
        {
            TypedComparer<T> typed = comparer as TypedComparer<T>;
            if (typed != null)
            {
                return typed.Comparer;
            }
            IComparer untyped = comparer as IComparer;
            if (untyped != null)
            {
                return untyped;
            }
            return new UntypedComparer<T>(comparer);
        }

        private UntypedComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public IComparer<T> Comparer
        {
            get { return _comparer; }
        }

        public int Compare(T x, T y)
        {
            return _comparer.Compare(x, y);
        }

        int IComparer.Compare(object x, object y)
        {
            return _comparer.Compare((T)x, (T)y);
        }
    }

    internal sealed class ComparisonWrapper<T> : Comparer<T>
    {
        private readonly Func<T, T, int> _comparison;

        public static IComparer<T> GetComparer(Func<T, T, int> comparison)
        {
            IComparer<T> source = comparison.Target as IComparer<T>;
            return source ?? new ComparisonWrapper<T>(comparison);
        }

        private ComparisonWrapper(Func<T, T, int> comparison)
        {
            _comparison = comparison;
        }

        public override int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
    }

    internal sealed class CompoundComparer<T> : Comparer<T>
    {
        private readonly List<IComparer<T>> _comparers;

        public static IComparer<T> GetComparer(IComparer<T> baseComparer, IComparer<T> nextComparer)
        {
            // make sure null comparer stays highest precedence
            IPrecedenceEnforcer<T> nullComparer = baseComparer as IPrecedenceEnforcer<T>;
            if (nullComparer != null)
            {
                return nullComparer.CreateCompoundComparer(nextComparer);
            }
            CompoundComparer<T> comparer = new CompoundComparer<T>();
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
            if (comparer is NullComparer<T>)
            {
                return;
            }
            CompoundComparer<T> other = comparer as CompoundComparer<T>;
            if (other != null)
            {
                _comparers.AddRange(other._comparers);
                return;
            }
            _comparers.Add(comparer);
        }

        public override int Compare(T x, T y)
        {
            foreach (IComparer<T> comparer in _comparers)
            {
                int result = comparer.Compare(x, y);
                if (result != 0)
                {
                    return result;
                }
            }
            return 0;
        }

        public IComparer<T> Normalize()
        {
            if (_comparers.Count == 0)
            {
                return NullComparer<T>.Default;
            }
            if (_comparers.Count == 1)
            {
                return _comparers[0];
            }
            return this;
        }
    }

    internal sealed class ReversedComparer<T> : Comparer<T>
    {
        private readonly IComparer<T> _comparer;

        public ReversedComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(T x, T y)
        {
            return _comparer.Compare(y, x);
        }
    }

    internal sealed class UncheckedNullableComparer<T> : Comparer<T?>
        where T : struct
    {
        private readonly IComparer<T> _comparer;

        public UncheckedNullableComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(T? x, T? y)
        {
            return _comparer.Compare(x.Value, y.Value);
        }
    }

    internal interface IPrecedenceEnforcer<T>
    {
        IComparer<T> CreateUnkeyedComparer(bool nullsFirst);

        IComparer<T> CreateKeyedComparer<TKey>(Func<T, TKey> keySelector, bool nullsFirst);

        IComparer<T> CreateCompoundComparer(IComparer<T> comparer);
    }

    internal abstract class NullPlacementComparer<T, TCompared> : Comparer<T>
    {
        protected readonly IComparer<T> Comparer;
        protected readonly NullFilter<TCompared> NullFilter;

        protected NullPlacementComparer(IComparer<T> comparer, NullFilter<TCompared> filter)
        {
            Comparer = comparer;
            NullFilter = filter;
        }
    }

    internal sealed class UnkeyedNullPlacementComparer<T> : NullPlacementComparer<T, T>, IPrecedenceEnforcer<T>
    {
        public static IComparer<T> GetComparer(IComparer<T> comparer, bool nullsFirst)
        {
            NullFilter<T> filter = NullFilterFactory.GetNullFilter<T>(nullsFirst);
            if (filter == null)
            {
                return comparer;
            }
            return new UnkeyedNullPlacementComparer<T>(comparer, filter);
        }

        internal UnkeyedNullPlacementComparer(IComparer<T> comparer, NullFilter<T> filter)
            : base(comparer, filter)
        {
        }

        public override int Compare(T x, T y)
        {
            return NullFilter.Filter(x, y) ?? Comparer.Compare(x, y);
        }

        public IComparer<T> CreateUnkeyedComparer(bool nullsFirst)
        {
            // we're duplicating ourselves
            if (nullsFirst == NullFilter.NullsFirst)
            {
                return this;
            }
            // we're replacing ourselves with another top-level comparer with a different sort order
            return UnkeyedNullPlacementComparer<T>.GetComparer(Comparer, nullsFirst);
        }

        public IComparer<T> CreateKeyedComparer<TKey>(Func<T, TKey> keySelector, bool nullsFirst)
        {
            // we want to take precedence over the new keyed comparer
            IComparer<T> comparer = KeyedNullPlacementComparer<T, TKey>.GetComparer(Comparer, keySelector, nullsFirst);
            return UnkeyedNullPlacementComparer<T>.GetComparer(comparer, NullFilter.NullsFirst);
        }

        public IComparer<T> CreateCompoundComparer(IComparer<T> comparer)
        {
            CompoundComparer<T> compoundComparer = new CompoundComparer<T>();
            compoundComparer.AppendComparison(Comparer);
            compoundComparer.AppendComparison(comparer);
            return UnkeyedNullPlacementComparer<T>.GetComparer(compoundComparer.Normalize(), NullFilter.NullsFirst);
        }
    }

    internal sealed class KeyedNullPlacementComparer<T, TKey> : NullPlacementComparer<T, TKey>, IPrecedenceEnforcer<T>
    {
        private readonly Func<T, TKey> _keySelector;

        public static IComparer<T> GetComparer(IComparer<T> comparer, Func<T, TKey> keySelector, bool nullsFirst)
        {
            NullFilter<TKey> filter = NullFilterFactory.GetNullFilter<TKey>(nullsFirst);
            if (filter == null)
            {
                return comparer;
            }
            return new KeyedNullPlacementComparer<T, TKey>(comparer, keySelector, filter);
        }

        internal KeyedNullPlacementComparer(IComparer<T> comparer, Func<T, TKey> keySelector, NullFilter<TKey> filter)
            : base(comparer, filter)
        {
            _keySelector = keySelector;
        }

        public override int Compare(T x, T y)
        {
            TKey xKey = _keySelector(x);
            TKey yKey = _keySelector(y);
            return NullFilter.Filter(xKey, yKey) ?? Comparer.Compare(x, y);
        }

        public IComparer<T> CreateUnkeyedComparer(bool nullsFirst)
        {
            // top-level comparers should be evaluated before keyed comparers
            return UnkeyedNullPlacementComparer<T>.GetComparer(this, nullsFirst);
        }

        public IComparer<T> CreateKeyedComparer<TOtherKey>(Func<T, TOtherKey> keySelector, bool nullsFirst)
        {
            // we don't know if the key selector is identical, so we must wrap ourselves
            return KeyedNullPlacementComparer<T, TOtherKey>.GetComparer(this, keySelector, nullsFirst);
        }

        public IComparer<T> CreateCompoundComparer(IComparer<T> comparer)
        {
            CompoundComparer<T> compoundComparer = new CompoundComparer<T>();
            compoundComparer.AppendComparison(Comparer);
            compoundComparer.AppendComparison(comparer);
            return KeyedNullPlacementComparer<T, TKey>.GetComparer(compoundComparer.Normalize(), _keySelector, NullFilter.NullsFirst);
        }
    }

    internal static class NullFilterFactory
    {
        private static readonly Dictionary<Type, object[]> _filterTypeLookup = new Dictionary<Type, object[]>();

        public static NullFilter<T> GetNullFilter<T>(bool nullsFirst)
        {
            Type comparedType = typeof(T);
            object[] filters;
            if (!_filterTypeLookup.TryGetValue(comparedType, out filters))
            {
                filters = getTypeFilters<T>(comparedType, filters);
                _filterTypeLookup.Add(comparedType, filters);
            }
            return (NullFilter<T>)filters[nullsFirst ? 1 : 0];
        }

        private static object[] getTypeFilters<T>(Type comparedType, object[] filters)
        {
            Type underlyingType = Nullable.GetUnderlyingType(comparedType);
            if (underlyingType != null)
            {
                Type genericFilterType = typeof(NullableNullFilter<>);
                Type filterType = genericFilterType.MakeGenericType(underlyingType);
                return new object[]
                { 
                    Activator.CreateInstance(filterType, new object[] { false }),
                    Activator.CreateInstance(filterType, new object[] { true }),
                };
            }
            if (comparedType.IsValueType)
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

    internal abstract class NullFilter<T>
    {
        private readonly bool _nullsFirst;

        protected NullFilter(bool nullsFirst)
        {
            _nullsFirst = nullsFirst;
        }

        public bool NullsFirst
        {
            get { return _nullsFirst; }
        }

        public abstract int? Filter(T x, T y);
    }

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
            if (y.HasValue)
            {
                if (NullsFirst)
                {
                    return -1;
                }
                return 1;
            }
            return 0;
        }
    }

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
                if (NullsFirst)
                {
                    return -1;
                }
                return 1;
            }
            if (y == null)
            {
                if (NullsFirst)
                {
                    return 1;
                }
                return -1;
            }
            return null;
        }
    }
}
