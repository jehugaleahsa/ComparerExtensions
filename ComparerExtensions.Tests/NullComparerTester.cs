using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerExtensions.Tests
{
    /// <summary>
    /// Tests the NullComparer class.
    /// </summary>
    [TestClass]
    public class NullComparerTester
    {
        #region Real World Example

        /// <summary>
        /// We can use NullComparer to build a comparison on the fly.
        /// </summary>
        [TestMethod]
        public void TestNullComparer_BuildComparer()
        {
            Random random = new Random();

            // build a list of random dates
            var dates = new List<DateTime>(getRandomDates(random));

            // randomly choose which order to sort the components by
            string[] propertyNames = { "Year", "Month", "Day", "Hour", "Minute", "Second" };
            randomShuffle(propertyNames, random);

            // build a comparer based on the order of the components
            IComparer<DateTime> dateComparer = NullComparer<DateTime>.Default;
            foreach (string propertyName in propertyNames)
            {
                string current = propertyName; // avoids non-local lambda problem
                Func<DateTime, object> getter = (DateTime d) => typeof(DateTime).GetProperty(current).GetValue(d, null);
                bool ascending = random.Next() % 2 == 0;
                if (ascending)
                {
                    dateComparer = dateComparer.ThenBy(getter);
                }
                else
                {
                    dateComparer = dateComparer.ThenByDescending(getter);
                }
            }

            // now we can sort the dates accordingly
            dates.Sort(dateComparer);
        }

        private static IEnumerable<DateTime> getRandomDates(Random random)
        {
            for (int count = 0; count != 100; ++count)
            {
                DateTime dateTime = new DateTime(
                    random.Next(1900, 2020), // year
                    random.Next(1, 13), // month
                    random.Next(1, 28), // day
                    random.Next(0, 24), // hour
                    random.Next(0, 60), // minute
                    random.Next(0, 60) // second
                    );
                yield return dateTime;
            }
        }

        private static void randomShuffle<T>(T[] items, Random random)
        {
            for (int index = 0; index != items.Length; ++index)
            {
                int other = random.Next(items.Length);
                T temp = items[index];
                items[index] = items[other];
                items[other] = temp;
            }
        }

        #endregion

        /// <summary>
        /// In order for the NullComparer to have no affect, it always returns zero.
        /// </summary>
        [TestMethod]
        public void TestCompare_ReturnsZero()
        {
            IComparer<int> comparer = NullComparer<int>.Default;
            int result = comparer.Compare(0, 1);
            Assert.AreEqual(0, result, "The result was non-zero.");
        }

        /// <summary>
        /// In order for the NullComparer to have no affect, it always returns zero.
        /// </summary>
        [TestMethod]
        public void TestCompare_IComparer_ReturnsZero()
        {
            IComparer comparer = NullComparer<int>.Default;
            int result = comparer.Compare(0, 1);
            Assert.AreEqual(0, result, "The result was non-zero.");
        }
    }
}
