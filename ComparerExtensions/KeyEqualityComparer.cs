﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComparerExtensions
{
    /// <summary>
    /// Compares values by comparing their extracted key values.
    /// </summary>
    /// <typeparam name="T">The type of the value being compared.</typeparam>
    public abstract class KeyEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        /// <summary>
        /// Initializes a new instances of a KeyEqualityComparer.
        /// </summary>
        protected KeyEqualityComparer()
        {
        }

        /// <summary>
        /// Determines whether the given values are equal.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>True if the given value are equal; otherwise, false.</returns>
        /// <remarks>
        /// This method executes by comparing the keys generated by the underlying key selector.
        /// If one of the items passed to the method are null, the key selector must be made to handle nulls.
        /// </remarks>
        public abstract bool Equals(T x, T y);

        /// <summary>
        /// Gets the hash code for the given value.
        /// </summary>
        /// <param name="obj">The value to get the hash code for.</param>
        /// <returns>The hash code.</returns>
        /// <exception cref="System.ArgumentNullException">The value is null.</exception>
        public abstract int GetHashCode(T obj);

        bool IEqualityComparer.Equals(object x, object y) => Equals((T)x, (T)y);

        int IEqualityComparer.GetHashCode(object obj) => GetHashCode((T)obj);

        /// <summary>
        /// Creates a KeyEqualityComparer that compares values by the key returned by the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The delegate to use to select the key.</param>
        /// <returns>A KeyEqualityComparer that compares values by the key returned by the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector delegate is null.</exception>
        public static KeyEqualityComparer<T> Using<TKey>(Func<T, TKey> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            return new TypedKeyEqualityComparer<T, TKey>(keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a KeyEqualityComparer that compares values by the key returned by the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The delegate used to select the key.</param>
        /// <param name="keyComparer">The equality comparer to use to compare the keys.</param>
        /// <returns>A KeyEqualityComparer that compares values by the key returned by the key selector.</returns>
        /// <exception cref="System.ArgumentNullException">The key selector delegate is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key equality comparer is null.</exception>
        public static KeyEqualityComparer<T> Using<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }
            return new TypedKeyEqualityComparer<T, TKey>(keySelector, keyComparer);
        }

        /// <summary>
        /// Creates a KeyEqualityComparer based on the current key comparer that
        /// in addition checks for equality based the value returned by the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the value returned by the key selector.</typeparam>
        /// <param name="keySelector">The delegate used to select the next key.</param>
        /// <returns>
        /// A KeyEqualityComparer based on the current key comparer that
        /// in addition checks for equality based the value returned by the key selector.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        public KeyEqualityComparer<T> And<TKey>(Func<T, TKey> keySelector)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            var comparer = new CompoundKeyEqualityComparer<T>();
            comparer.Add(this);
            comparer.Add(new TypedKeyEqualityComparer<T, TKey>(keySelector, EqualityComparer<TKey>.Default));
            return comparer;
        }

        /// <summary>
        /// Creates a KeyEqualityComparer based on the current key comparer that
        /// in addition checks for equality based the value returned by the key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the value returned by the key selector.</typeparam>
        /// <param name="keySelector">The delegate used to select the next key.</param>
        /// <param name="keyComparer">The equality comparer to use to compare the keys.</param>
        /// <returns>
        /// A KeyEqualityComparer based on the current key comparer that
        /// in addition checks for equality based the value returned by the key selector.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The key selector is null.</exception>
        /// <exception cref="System.ArgumentNullException">The key comparer is null.</exception>
        public KeyEqualityComparer<T> And<TKey>(Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }
            var comparer = new CompoundKeyEqualityComparer<T>();
            comparer.Add(this);
            comparer.Add(new TypedKeyEqualityComparer<T, TKey>(keySelector, keyComparer));
            return comparer;
        }
    }

    internal sealed class TypedKeyEqualityComparer<T, TKey> : KeyEqualityComparer<T>
    {
        private readonly Func<T, TKey> keySelector;
        private readonly IEqualityComparer<TKey> keyComparer;

        public TypedKeyEqualityComparer(Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
        {
            this.keySelector = keySelector;
            this.keyComparer = keyComparer;
        }

        public override bool Equals(T x, T y)
        {
            var key1 = keySelector(x);
            var key2 = keySelector(y);
            return keyComparer.Equals(key1, key2);
        }

        public override int GetHashCode(T obj)
        {
            var key = keySelector(obj);
            return keyComparer.GetHashCode(key);
        }
    }

    internal sealed class CompoundKeyEqualityComparer<T> : KeyEqualityComparer<T>
    {
        private readonly List<KeyEqualityComparer<T>> comparers;

        public CompoundKeyEqualityComparer()
        {
            comparers = new List<KeyEqualityComparer<T>>();
        }

        public void Add(KeyEqualityComparer<T> comparer)
        {
            if (comparer is CompoundKeyEqualityComparer<T> compoundComparer)
            {
                comparers.AddRange(compoundComparer.comparers);
            }
            else
            {
                comparers.Add(comparer);
            }
        }

        public override bool Equals(T x, T y)
        {
            foreach (var comparer in comparers)
            {
                if (!comparer.Equals(x, y))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode(T obj)
        {
            return comparers.Aggregate(0, (hc, comparer) => GetHashCode(hc, comparer.GetHashCode(obj)));
        }

        private static int GetHashCode(int hashCode1, int hashCode2)
        {
            return (hashCode1 << 5) + hashCode1 ^ hashCode2;
        }
    }
}
