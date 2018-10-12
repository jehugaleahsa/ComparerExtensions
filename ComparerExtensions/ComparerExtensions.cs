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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            var wrapper = ComparisonWrapper<T>.GetComparer(comparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, wrapper);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
        public static IComparer<T> ThenBy<T, TKey>(
            this IComparer<T> baseComparer, 
            Func<T, TKey> keySelector, 
            IComparer<TKey> keyComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector, keyComparer);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
        public static IComparer<T> ThenBy<T, TKey>(
            this IComparer<T> baseComparer, 
            Func<T, TKey> keySelector, 
            Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderBy(keySelector, keyComparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
        public static IComparer<T> ThenByDescending<T, TKey>(
            this IComparer<T> baseComparer, 
            Func<T, TKey> keySelector, 
            IComparer<TKey> keyComparer)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparer);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
        public static IComparer<T> ThenByDescending<T, TKey>(
            this IComparer<T> baseComparer,
            Func<T, TKey> keySelector,
            Func<TKey, TKey, int> keyComparison)
        {
            if (baseComparer == null)
            {
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var comparer = KeyComparer<T>.OrderByDescending(keySelector, keyComparison);
            var compoundComparer = CompoundComparer<T>.GetComparer(baseComparer, comparer);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            var uncheckedComparer = new UncheckedNullableComparer<T>(baseComparer);
            return UnkeyedNullPlacementComparer<T?>.GetComparer(uncheckedComparer, true);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (baseComparer is IPrecedenceEnforcer<T> previous)
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
            if (baseComparer is IPrecedenceEnforcer<T> previous)
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (baseComparer is IPrecedenceEnforcer<T> previous)
            {
                return previous.CreateKeyedComparer(keySelector, true);
            }
            return KeyedNullPlacementComparer<T, TKey>.GetComparer(baseComparer, keySelector, true);
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
                throw new ArgumentNullException(nameof(baseComparer));
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (baseComparer is IPrecedenceEnforcer<T> previous)
            {
                return previous.CreateKeyedComparer(keySelector, false);
            }
            return KeyedNullPlacementComparer<T, TKey>.GetComparer(baseComparer, keySelector, false);
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
                throw new ArgumentNullException(nameof(comparer));
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
                throw new ArgumentNullException(nameof(comparer));
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
                throw new ArgumentNullException(nameof(comparer));
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
                throw new ArgumentNullException(nameof(comparison));
            }
            return ComparisonWrapper<T>.GetComparer(comparison);
        }
    }
}
