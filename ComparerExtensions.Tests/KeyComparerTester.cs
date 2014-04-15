using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace ComparerExtensions.Tests
{
    /// <summary>
    /// Tests the KeyComparer class.
    /// </summary>
    [TestClass]
    public class KeyComparerTester
    {
        #region Real World Example

        /// <summary>
        /// We can use KeyComparer to compare strings by their lengths.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_SortStringsByLength()
        {
            Random random = new Random();

            string[] names = { "Bob", "George", "Mary", "Jessica", "David" };

            IComparer<string> comparer = KeyComparer<string>.OrderBy(item => item.Length);
            Array.Sort(names, comparer);

            string[] expected = { "Bob", "Mary", "David", "George", "Jessica" };
            CollectionAssert.AreEqual(expected, names, "The names were not sorted by length.");
        }

        /// <summary>
        /// We can use KeyComparer to compare strings by their lengths, in reverse order.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_Descending_SortStringsByLength()
        {
            Random random = new Random();

            string[] names = { "Bob", "George", "Mary", "Jessica", "David" };

            IComparer<string> comparer = KeyComparer<string>.OrderByDescending(item => item.Length);
            Array.Sort(names, comparer);

            string[] expected = { "Jessica", "George", "David", "Mary", "Bob" };
            CollectionAssert.AreEqual(expected, names, "The names were not sorted by length.");
        }

        #endregion

        #region Argument Checking

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderBy_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            KeyComparer<int>.OrderBy<int>(keySelector);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderBy_WithComparer_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            Comparer<int> keyComparer = Comparer<int>.Default;
            KeyComparer<int>.OrderBy<int>(keySelector, keyComparer);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key comparison, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderBy_NullKeyComparer_Throws()
        {
            Func<int, int> keySelector = i => i;
            Comparer<int> keyComparer = null;
            KeyComparer<int>.OrderBy<int>(keySelector, keyComparer);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderBy_WithComparison_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            KeyComparer<int>.OrderBy<int>(keySelector, keyComparison);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key comparison, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderBy_NullKeyComparison_Throws()
        {
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = null;
            KeyComparer<int>.OrderBy<int>(keySelector, keyComparison);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderByDescending_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            KeyComparer<int>.OrderByDescending<int>(keySelector);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderByDescending_WithComparer_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            Comparer<int> keyComparer = Comparer<int>.Default;
            KeyComparer<int>.OrderByDescending<int>(keySelector, keyComparer);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key comparison, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderByDescending_NullKeyComparer_Throws()
        {
            Func<int, int> keySelector = i => i;
            Comparer<int> keyComparer = null;
            KeyComparer<int>.OrderByDescending<int>(keySelector, keyComparer);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key selector, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderByDescending_WithComparison_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            KeyComparer<int>.OrderByDescending<int>(keySelector, keyComparison);
        }

        /// <summary>
        /// If we try to define a KeyComparer using a null key comparison, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestKeyComparer_OrderByDescending_NullKeyComparison_Throws()
        {
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = null;
            KeyComparer<int>.OrderByDescending<int>(keySelector, keyComparison);
        }

        #endregion

        /// <summary>
        /// If we need to use the key comparer for pre-.NET 2.0 code, we can use the non-generic IComparer interface.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_Explicit_NonGeneric()
        {
            IComparer comparer = KeyComparer<int>.OrderBy(i => i).Untyped();
            int result = comparer.Compare(1, 2);
            Assert.IsTrue(result < 0, "Failed to compare values explicitly.");
        }

        /// <summary>
        /// If the key we pick is too complex to be sorted by default, we can use another comparer to sort it as well.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_OrderBy_WithKeyComparer()
        {
            // build a week, starting on a Sunday
            DateTime firstDate = new DateTime(2013, 1, 6);
            DateTime[] week = new DateTime[]
            {
                new DateTime(2013, 1, 6),
                new DateTime(2013, 1, 7),
                new DateTime(2013, 1, 8),
                new DateTime(2013, 1, 9),
                new DateTime(2013, 1, 10),
                new DateTime(2013, 1, 11),
                new DateTime(2013, 1, 12),
            };

            // force Mondays to come first!
            IComparer<DayOfWeek> dayComparer = NullComparer<DayOfWeek>.Default.ThenBy(day => (DayOfWeek)((int)(day + 6) % 7));
            Array.Sort(week, KeyComparer<DateTime>.OrderBy(d => d.DayOfWeek, dayComparer));

            Assert.AreEqual(firstDate, week[week.Length - 1], "Sunday did not get moved to the end of the week.");
        }

        /// <summary>
        /// If the key we pick is too complex to be sorted by default, we can use another comparison to sort it as well.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_OrderBy_WithKeyComparison()
        {
            // build a week, starting on a Sunday
            DateTime firstDate = new DateTime(2013, 1, 6);
            DateTime[] week = new DateTime[]
            {
                new DateTime(2013, 1, 6),
                new DateTime(2013, 1, 7),
                new DateTime(2013, 1, 8),
                new DateTime(2013, 1, 9),
                new DateTime(2013, 1, 10),
                new DateTime(2013, 1, 11),
                new DateTime(2013, 1, 12),
            };

            // force Mondays to come first!
            IComparer<DayOfWeek> dayComparer = NullComparer<DayOfWeek>.Default.ThenBy(day => (DayOfWeek)((int)(day + 6) % 7));
            Array.Sort(week, KeyComparer<DateTime>.OrderBy(d => d.DayOfWeek, dayComparer.Compare));

            Assert.AreEqual(firstDate, week[week.Length - 1], "Sunday did not get moved to the end of the week.");
        }

        /// <summary>
        /// If the key we pick is too complex to be sorted by default, we can use another comparer to sort it as well.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_OrderByDescending_WithKeyComparer()
        {
            // build a week, starting on a Sunday
            DateTime firstDate = new DateTime(2013, 1, 6);
            DateTime[] week = new DateTime[]
            {
                new DateTime(2013, 1, 6),
                new DateTime(2013, 1, 7),
                new DateTime(2013, 1, 8),
                new DateTime(2013, 1, 9),
                new DateTime(2013, 1, 10),
                new DateTime(2013, 1, 11),
                new DateTime(2013, 1, 12),
            };

            // force Mondays to come last!
            IComparer<DayOfWeek> dayComparer = NullComparer<DayOfWeek>.Default.ThenBy(day => (DayOfWeek)((int)(day + 6) % 7));
            Array.Sort(week, KeyComparer<DateTime>.OrderByDescending(d => d.DayOfWeek, dayComparer));

            Assert.AreEqual(firstDate.AddDays(1), week[week.Length - 1], "Monday did not appear at the end of the week.");
        }

        /// <summary>
        /// If the key we pick is too complex to be sorted by default, we can use another comparison to sort it as well.
        /// </summary>
        [TestMethod]
        public void TestKeyComparer_OrderByDescending_WithKeyComparison()
        {
            // build a week, starting on a Sunday
            DateTime firstDate = new DateTime(2013, 1, 6);
            DateTime[] week = new DateTime[]
            {
                new DateTime(2013, 1, 6),
                new DateTime(2013, 1, 7),
                new DateTime(2013, 1, 8),
                new DateTime(2013, 1, 9),
                new DateTime(2013, 1, 10),
                new DateTime(2013, 1, 11),
                new DateTime(2013, 1, 12),
            };

            // force Mondays to come last!
            IComparer<DayOfWeek> dayComparer = NullComparer<DayOfWeek>.Default.ThenBy(day => (DayOfWeek)((int)(day + 6) % 7));
            Array.Sort(week, KeyComparer<DateTime>.OrderByDescending(d => d.DayOfWeek, dayComparer.Compare));

            Assert.AreEqual(firstDate.AddDays(1), week[week.Length - 1], "Monday did not appear at the end of the week.");
        }
    }
}
