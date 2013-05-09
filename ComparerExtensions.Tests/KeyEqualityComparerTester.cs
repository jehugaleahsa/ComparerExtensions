using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace ComparerExtensions.Tests
{
    /// <summary>
    /// Tests the KeyEqualityComparer class.
    /// </summary>
    [TestClass]
    public class KeyEqualityComparerTester
    {
        #region Real World Example

        /// <summary>
        /// We can use KeyEqualityComparer to compare the values of properties and method calls within a complex type.
        /// </summary>
        [TestMethod]
        public void TestKeyEqualityComparer_FindByDayOfWeek()
        {
            // find the day with the same day of week as today
            IEqualityComparer<DateTime> comparer = KeyEqualityComparer<DateTime>.Using(date => date.DayOfWeek);
            HashSet<DateTime> dates = new HashSet<DateTime>(comparer);
            DateTime today = new DateTime(2013, 01, 05);
            Assert.IsTrue(dates.Add(today), "The date should have been added.");
            DateTime weekFromToday = today.AddDays(7);
            Assert.IsFalse(dates.Add(weekFromToday), "This day of the week has already been added.");
        }

        /// <summary>
        /// We can use KeyEqualityComparer with Sets or Dictionaries.
        /// </summary>
        [TestMethod]
        public void TestKeyEqualityComparer_CaseInsensitiveSet()
        {
            string[] names = { "Bob", "Tom", "Gary", "Joe", "bOB", "bob", "TOM" };

            KeyEqualityComparer<string> comparer = KeyEqualityComparer<string>.Using(name => name.ToUpperInvariant());
            HashSet<string> nameSet = new HashSet<string>(names, comparer);
            Assert.AreEqual(4, nameSet.Count, "The set did not see some values as duplicates.");
        }

        /// <summary>
        /// We can specify multiple keys for determining if two objects are equal.
        /// </summary>
        [TestMethod]
        public void TestKeyEqualityComparer_MultipleKeys()
        {
            IEqualityComparer<Person> comparer = KeyEqualityComparer<Person>.Using(p => p.LastName).And(p => p.FirstName);
            Person joeSmith = new Person()
            {
                FirstName = "Joe",
                LastName = "Smith",
            };
            Person johnSmith = new Person()
            {
                FirstName = "John",
                LastName = "Smith",
            };
            Assert.IsTrue(comparer.Equals(johnSmith, johnSmith), "The same object did not equal itself.");
            Assert.IsFalse(comparer.Equals(joeSmith, johnSmith), "The first name was not used in the comparison.");
        }

        private sealed class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        #endregion

        #region Argument Checking

        /// <summary>
        /// If we try to create a new KeyEqualityComparer with a null key selector,
        /// an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUsing_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            KeyEqualityComparer<int>.Using(keySelector);
        }

        /// <summary>
        /// If we try to create a new KeyEqualityComparer with a null key selector,
        /// an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUsing_WithKeyComparer_NullKeySelector_Throws()
        {
            Func<int, int> keySelector = null;
            IEqualityComparer<int> keyComparer = EqualityComparer<int>.Default;
            KeyEqualityComparer<int>.Using(keySelector, keyComparer);
        }

        /// <summary>
        /// If we try to create a new KeyEqualityComparer with a null key comparer,
        /// an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUsing_NullKeyComparer_Throws()
        {
            Func<int, int> keySelector = i => i;
            IEqualityComparer<int> keyComparer = null;
            KeyEqualityComparer<int>.Using(keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if we try to pass a null key selector
        /// to And.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAnd_KeySelectorNull_Throws()
        {
            Func<int, int> keySelector = null;
            KeyEqualityComparer<int>.Using(i => i).And(keySelector);
        }

        /// <summary>
        /// An exception should be thrown if we try to pass a null key selector
        /// to And.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAnd_Comparer_KeySelectorNull_Throws()
        {
            Func<int, int> keySelector = null;
            IEqualityComparer<int> keyComparer = EqualityComparer<int>.Default;
            KeyEqualityComparer<int>.Using(i => i).And(keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if we try to pass a null key comparer
        /// to And.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAnd_KeyComparerNull_Throws()
        {
            Func<int, int> keySelector = i => i;
            IEqualityComparer<int> keyComparer = null;
            KeyEqualityComparer<int>.Using(i => i).And(keySelector, keyComparer);
        }

        /// <summary>
        /// Since KeyEqualityComparer implements the non-generic IEqualityComparer interface,
        /// it is possible that non-T instances can be passed to the Equals method. This
        /// should cause an exception to be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestEquals_Explicit_FirstValueWrongType_Throws()
        {
            IEqualityComparer comparer = KeyEqualityComparer<int>.Using(i => i);
            comparer.Equals("Hello", 0);
        }

        /// <summary>
        /// Since KeyEqualityComparer implements the non-generic IEqualityComparer interface,
        /// it is possible that non-T instances can be passed to the Equals method. This
        /// should cause an exception to be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestEquals_Explicit_SecondValueWrongType_Throws()
        {
            IEqualityComparer comparer = KeyEqualityComparer<int>.Using(i => i);
            comparer.Equals(0, DateTime.Now);
        }

        /// <summary>
        /// Since KeyEqualityComparer implements the non-generic IEqualityComparer interface,
        /// it is possible that non-T instances can be passed to the GetHashCode method. This
        /// should cause an exception to be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestGetHashCode_Explicit_ValueWrongType_Throws()
        {
            IEqualityComparer comparer = KeyEqualityComparer<int>.Using(i => i);
            comparer.GetHashCode(DateTime.Now);
        }

        #endregion

        /// <summary>
        /// If the two generated keys are equal, true should be returned.
        /// </summary>
        [TestMethod]
        public void TestEquals_MatchingByKey_ReturnsTrue()
        {
            // ignore time when comparing dates
            IEqualityComparer<DateTime> comparer = KeyEqualityComparer<DateTime>.Using(d => d.Date);
            DateTime first = new DateTime(2011, 1, 11, 4, 53, 0);
            DateTime second = new DateTime(2011, 1, 11, 9, 18, 0);
            bool result = comparer.Equals(first, second);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// If the two generated keys are equal, true should be returned.
        /// </summary>
        [TestMethod]
        public void TestEquals_KeyDoNotMatch_ReturnsFalse()
        {
            // ignore time when comparing dates
            IEqualityComparer<DateTime> comparer = KeyEqualityComparer<DateTime>.Using(d => d.Date);
            DateTime first = new DateTime(2011, 1, 11, 4, 53, 0);
            DateTime second = new DateTime(2011, 1, 12, 9, 18, 0);
            bool result = comparer.Equals(first, second);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// If the two generated keys are equal, true should be returned.
        /// </summary>
        [TestMethod]
        public void TestEquals_WithKeyComparer_KeysMatch_ReturnsTrue()
        {
            // ignore time when comparing dates
            IEqualityComparer<DateTime> comparer = KeyEqualityComparer<DateTime>.Using(d => d.ToString(), StringComparer.OrdinalIgnoreCase);
            DateTime first = new DateTime(2011, 1, 11);
            DateTime second = new DateTime(2011, 1, 11);
            bool result = comparer.Equals(first, second);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// If the two generated keys are equal, true should be returned.
        /// </summary>
        [TestMethod]
        public void TestEquals_WithKeyComparer_KeysDoNotMatch_ReturnsFalse()
        {
            // ignore time when comparing dates
            IEqualityComparer<DateTime> comparer = KeyEqualityComparer<DateTime>.Using(d => d.ToString(), StringComparer.OrdinalIgnoreCase);
            DateTime first = new DateTime(2011, 1, 11);
            DateTime second = new DateTime(2011, 1, 12);
            bool result = comparer.Equals(first, second);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// If we use the non-generic interface and both arguments are the correct type,
        /// comparing them should work normally.
        /// </summary>
        [TestMethod]
        public void TestEquals_Explicit_KeysMatch_ReturnsTrue()
        {
            // ignore time when comparing dates
            IEqualityComparer comparer = KeyEqualityComparer<DateTime>.Using(d => d);
            DateTime first = new DateTime(2011, 1, 11);
            DateTime second = new DateTime(2011, 1, 11);
            bool result = comparer.Equals(first, second);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// If we use the non-generic interface and the type is correct,
        /// we should get the key's hash code.
        /// </summary>
        [TestMethod]
        public void TestGetHashCode_Explicit_GetsKeysHashCode()
        {
            // ignore time when comparing dates
            IEqualityComparer comparer = KeyEqualityComparer<DateTime>.Using(d => d.DayOfYear);
            DateTime dateTime = new DateTime(2011, 1, 11);
            int actual = comparer.GetHashCode(dateTime);
            int expected = dateTime.DayOfYear.GetHashCode();
            Assert.AreEqual(expected, actual, "The key's hash code was not returned.");
        }

        /// <summary>
        /// If we have multiple And statements, we should flatten the compound
        /// comparer to avoid any unnecessary overhead.
        /// </summary>
        [TestMethod]
        public void TestAdd_MultipleAnds_FlattensCompoundComparers()
        {
            IEqualityComparer<int> comparer = KeyEqualityComparer<int>.Using(i => i).And(i => i).And(i => i);
            Assert.IsTrue(comparer.Equals(0, 0), "The comparer did see identical values as equal.");
            Assert.IsFalse(comparer.Equals(0, 1), "The compare saw different values as equal.");
        }

        /// <summary>
        /// If we do a case insensitive comparison, objects differing only base case should compare equal.
        /// </summary>
        [TestMethod]
        public void TestAdd_CaseInsensitiveComparer_IgnoresCase()
        {
            IEqualityComparer<Person> comparer = KeyEqualityComparer<Person>
                .Using(p => p.LastName, StringComparer.InvariantCultureIgnoreCase)
                .And(p => p.FirstName, StringComparer.InvariantCultureIgnoreCase);
            Person person1 = new Person() { FirstName = "Bob", LastName = "Hamburger" };
            Person person2 = new Person() { FirstName = "bob", LastName = "hamburger" };
            Assert.AreEqual(comparer.GetHashCode(person1), comparer.GetHashCode(person2), "The hash codes were not the same.");
            Assert.IsTrue(comparer.Equals(person1, person2), "The two objects were not considered equal.");
        }
    }
}
