using System;
using System.Collections;
using System.Collections.Generic;

namespace ComparerExtensions
{
    /// <summary>
    /// Compares items such that they are always equal.
    /// </summary>
    /// <typeparam name="T">The type of the items to compare.</typeparam>
    public sealed class NullComparer<T> : IComparer<T>, IComparer
    {
        private readonly static NullComparer<T> _instance = new NullComparer<T>();

        private NullComparer()
        {
        }

        /// <summary>
        /// Gets the default instance of a NullComparer.
        /// </summary>
        public static NullComparer<T> Default
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Always returns zero, indicating that the two values are equal.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>Zero, indicating that the two values are equal.</returns>
        public int Compare(T x, T y)
        {
            return 0;
        }

        int IComparer.Compare(object x, object y)
        {
            return Compare((T)x, (T)y);
        }
    }
}
