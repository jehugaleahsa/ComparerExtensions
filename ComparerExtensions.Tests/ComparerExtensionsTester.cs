using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerExtensions.Tests
{
    /// <summary>
    /// Tests the ComparerExtensions methods.
    /// </summary>
    [TestClass]
    public class ComparerExtensionsTester
    {
        #region Real World Examples

        // also see NullComparerTester for example of building a comparer at runtime

        /// <summary>
        /// We can sort names alphabetically, such that case is only relavent within matching names.
        /// </summary>
        [TestMethod]
        public void TestThenBy_SortByName_NaturalSort()
        {
            Random random = new Random();

            string[] names = { "Bob", "Mary", "David", "George", "Jessica", "bob", "ken", "jessica", "david" };

            // sort alphabetically, then by case
            IComparer<string> comparer = StringComparer.CurrentCultureIgnoreCase.ThenBy(StringComparer.CurrentCulture);
            Array.Sort(names, comparer.Compare);

            string[] expected = { "bob", "Bob", "david", "David", "George", "jessica", "Jessica", "ken", "Mary" };
            CollectionAssert.AreEqual(expected, names, "The items weren't sorted as expected.");
        }

        /// <summary>
        /// We can sort by the length of a string and then by value.
        /// </summary>
        [TestMethod]
        public void TestThenBy_SortByLength_ThenByValue()
        {
            Random random = new Random();

            string[] names = { "Bob", "Mary", "David", "George", "Jessica", "Ken", "Michael" };

            // sort by length, then alphabetically
            IComparer<string> comparer = KeyComparer<string>.OrderBy(i => i.Length).ThenBy(i => i);
            Array.Sort(names, comparer.Compare);

            string[] expected = { "Bob", "Ken", "Mary", "David", "George", "Jessica", "Michael" };
            CollectionAssert.AreEqual(expected, names, "The items weren't sorted as expected.");
        }

        /// <summary>
        /// We can sort descending using Reversed.
        /// </summary>
        [TestMethod]
        public void TestReversed_SortDescending()
        {
            int[] values = { 1, 2, 3, 4, 5 };

            Array.Sort(values, Comparer<int>.Default.Reversed().Compare);

            int[] expected = { 5, 4, 3, 2, 1 };
            CollectionAssert.AreEqual(expected, values, "The list was not sorted as expected.");
        }

        /// <summary>
        /// When working with nullables, the default comparison moves nulls to the front of the list.
        /// We can extend a non-nullable comparison and tell it to handle nulls by moving them to the end of the list.
        /// </summary>
        [TestMethod]
        public void TestToNullableComparer_MoveNullsToFrontOfList()
        {
            int?[] values = { 5, null, 2, null, 3, 1, null, 4 };

            IComparer<int?> comparer = Comparer<int>.Default.ToNullableComparer();
            Array.Sort(values, comparer);

            int?[] expected = { null, null, null, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The list was not sorted as expected.");
        }

        /// <summary>
        /// When working with reference types, the default comparison moves nulls to the front of the list.
        /// We can extend a comparison and tell it to handle nulls by moving them to the end of the list.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_MoveNullsToEndOfList()
        {
            string[] values = { "e", null, "b", null, "c", "a", null, "d" };

            IComparer<string> comparer = StringComparer.Ordinal.NullsLast();
            Array.Sort(values, comparer);

            string[] expected = { "a", "b", "c", "d", "e", null, null, null };
            CollectionAssert.AreEqual(expected, values, "The list was not sorted as expected.");
        }

        /// <summary>
        /// Even if someone creates a comparer, specifying nulls come last before
        /// they specify how to sort the non-null values, the nulls are still moved
        /// correctly.
        /// </summary>
        [TestMethod]
        public void TestNullPlacement_MultipleCalls_AppliesMostRecent()
        {
            string[] values = { "a", null, "b", "c", null, "d", "e" };

            IComparer<string> comparer = Comparer<string>.Default.NullsFirst().NullsLast();
            Array.Sort(values, comparer);

            string[] expected = { "a", "b", "c", "d", "e", null, null };
            CollectionAssert.AreEqual(expected, values, "The list was not in the expected order.");
        }

        /// <summary>
        /// A null placement comparer can appear anywhere within a comparison building expression.
        /// </summary>
        [TestMethod]
        public void TestNullPlacement_NullComparerFollowedByAdditionalOrdering()
        {
            string[] values = { "a", null, "b", "c", null, "d", "e" };

            IComparer<string> comparer = NullComparer<string>.Default.NullsLast().ThenBy(s => s);
            Array.Sort(values, comparer);

            string[] expected = { "a", "b", "c", "d", "e", null, null };
            CollectionAssert.AreEqual(expected, values, "The list was not in the expected order.");
        }

        /// <summary>
        /// If we create a keyed null comparer, it should be run after the top-level
        /// null comparer is run. It doesn't make sense to check for null keys if
        /// the object itself is a null reference.
        /// </summary>
        [TestMethod]
        public void TestNullPlacement_KeyedAfterTopLevel_TopLevelPrecedence()
        {
            Person[] peope = new Person[]
            {
                new Person() { LastName = "Anderson" },
                new Person() { LastName = "Carlson" },
                null,
                new Person() { LastName = null },
            };

            IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName).NullsLast().NullsLast(p => p.LastName);
            Array.Sort(peope, comparer);

            Assert.AreEqual("Anderson", peope[0].LastName, "Attempt 1: The first record was in the wrong position.");
            Assert.AreEqual("Carlson", peope[1].LastName, "Attempt 1: The second record was in the wrong position.");
            Assert.IsNull(peope[2].LastName, "Attempt 1: The third record was in the wrong position.");
            Assert.IsNull(peope[3], "Attempt 1: The forth record was in the wrong position.");

            comparer = KeyComparer<Person>.OrderBy(p => p.LastName).NullsLast(p => p.LastName).NullsLast();
            Array.Sort(peope, comparer);

            Assert.AreEqual("Anderson", peope[0].LastName, "Attempt 2: The first record was in the wrong position.");
            Assert.AreEqual("Carlson", peope[1].LastName, "Attempt 2: The second record was in the wrong position.");
            Assert.IsNull(peope[2].LastName, "Attempt 2: The third record was in the wrong position.");
            Assert.IsNull(peope[3], "Attempt 2: The forth record was in the wrong position.");
        }

        /// <summary>
        /// The null placement functions apply to objects being compared. When working with
        /// user-defined types, this can be confusing. For instance, you want to sort by
        /// last name, but some of those names can be null. Code like the following won't work:
        /// 
        /// KeyComparer&lt;Person&gt;.OrderBy(p => p.LastName).NullsLast()
        /// 
        /// The call to NullsLast applies to instances of Person, not Person.LastName. One
        /// solution is to pass another KeyComparer to the OrderBy method:
        /// 
        /// KeyComparer&lt;Person&gt;.OrderBy(p => p.LastName, StringComparer.CurrentCulture.NullsLast())
        /// 
        /// This approach is pretty long-winded because you can't default which comparison is used. Since
        /// we are just trying to handle nulls up-front, being this explicit is overkill. An
        /// overload of the NullsFirst and NullsLast methods are provided to make it easier
        /// to move nulls without needing to go down to the key level.
        /// </summary>
        [TestMethod]
        public void TestNullPlacement_SortKeyIsNull_HandleTopLevel()
        {
            Person[] people = new Person[]
            {
                new Person() { LastName = "Flint" },
                new Person() { LastName = null },  // this null would normally cause problems
                new Person() { LastName = "Hammer" },
                new Person() { LastName = "Yost" },
            };
            Person[] expected = new Person[]
            {
                people[3],
                people[0],
                people[2],
                people[1],
            };
            // sort by length of last name
            IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName.Length).NullsLast(p => p.LastName);
            Array.Sort(people, comparer);
            CollectionAssert.AreEqual(expected, people, "The people weren't sorted as expected.");
        }

        private sealed class Person
        {
            public string LastName { get; set; }
        }

        #endregion

        #region Argument Checking

        #region ThenBy

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithComparer_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            IComparer<int> comparer = Comparer<int>.Default;
            ComparerExtensions.ThenBy(baseComparer, comparer);
        }

        /// <summary>
        /// An exception should be thrown if the comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithComparer_NullComparer_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            IComparer<int> comparer = null;
            ComparerExtensions.ThenBy(baseComparer, comparer);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithComparison_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int, int> comparison = Comparer<int>.Default.Compare;
            ComparerExtensions.ThenBy(baseComparer, comparison);
        }

        /// <summary>
        /// An exception should be thrown if the comparison is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithComparison_NullComparison_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int, int> comparison = null;
            ComparerExtensions.ThenBy(baseComparer, comparison);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeySelector_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            ComparerExtensions.ThenBy(baseComparer, keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeySelector_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            ComparerExtensions.ThenBy(baseComparer, keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparer_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            IComparer<int> keyComparer = Comparer<int>.Default;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparer_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            IComparer<int> keyComparer = Comparer<int>.Default;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the key comparison is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparer_NullKeyComparison_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = i => i;
            IComparer<int> keyComparer = null;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparison_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparison);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparison_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparison);
        }

        /// <summary>
        /// An exception should be thrown if the key comparison is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenBy_WithKeyComparison_NullKeyComparison_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = null;
            ComparerExtensions.ThenBy(baseComparer, keySelector, keyComparison);
        }

        #endregion

        #region ThenByDescending

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeySelector_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeySelector_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparer_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            IComparer<int> keyComparer = Comparer<int>.Default;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparer_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            IComparer<int> keyComparer = Comparer<int>.Default;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the key comparison is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparer_NullKeyComparison_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = i => i;
            IComparer<int> keyComparer = null;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparer);
        }

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparison_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparison);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparison_NullKeySelector_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = null;
            Func<int, int, int> keyComparison = Comparer<int>.Default.Compare;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparison);
        }

        /// <summary>
        /// An exception should be thrown if the key comparison is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestThenByDescending_WithKeyComparison_NullKeyComparison_Throws()
        {
            IComparer<int> baseComparer = Comparer<int>.Default;
            Func<int, int> keySelector = i => i;
            Func<int, int, int> keyComparison = null;
            ComparerExtensions.ThenByDescending(baseComparer, keySelector, keyComparison);
        }

        #endregion

        #region Reversed

        /// <summary>
        /// An exception should be thrown if the base comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReversed_NullBaseComparer_Throws()
        {
            IComparer<int> baseComparer = null;
            ComparerExtensions.Reversed(baseComparer);
        }

        #endregion

        #region ToNullableComparer

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestToNullableComparer_NullComparer_Throws()
        {
            IComparer<int> comparer = null;
            comparer.ToNullableComparer();
        }

        #endregion

        #region NullsFirst

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsFirst_Nullable_NullComparer_Throws()
        {
            IComparer<int?> comparer = null;
            comparer.NullsFirst();
        }

        #endregion

        #region NullsLast

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsLast_Nullable_NullComparer_Throws()
        {
            IComparer<int?> comparer = null;
            comparer.NullsLast();
        }

        #endregion

        #region NullsFirst

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsFirst_NullComparer_Throws()
        {
            IComparer<string> comparer = null;
            comparer.NullsFirst();
        }

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsFirst_KeySelector_NullComparer_Throws()
        {
            IComparer<string> comparer = null;
            Func<string, string> keySelector = s => s;
            comparer.NullsFirst(keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsFirst_NullKeySelector_Throws()
        {
            IComparer<string> comparer = Comparer<string>.Default;
            Func<string, string> keySelector = null;
            comparer.NullsFirst(keySelector);
        }

        #endregion

        #region NullsLast

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsLast_NullComparer_Throws()
        {
            IComparer<string> comparer = null;
            comparer.NullsLast();
        }

        /// <summary>
        /// If we try to extend a null comparer, an exception should be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsLast_KeySelector_NullComparer_Throws()
        {
            IComparer<string> comparer = null;
            Func<string, string> keySelector = s => s;
            comparer.NullsLast(keySelector);
        }

        /// <summary>
        /// An exception should be thrown if the key selector is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullsLast_NullKeySelector_Throws()
        {
            IComparer<string> comparer = Comparer<string>.Default;
            Func<string, string> keySelector = null;
            comparer.NullsLast(keySelector);
        }

        #endregion

        #endregion

        #region ThenBy

        /// <summary>
        /// We can perform subsequent sorting with a comparer.
        /// </summary>
        [TestMethod]
        public void TestThenBy_WithComparer_Sorts()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = Comparer<int>.Default.ThenBy(Comparer<int>.Default);
            Array.Sort(values, comparer);

            int[] expected = { 1, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting with a comparison.
        /// </summary>
        [TestMethod]
        public void TestThenBy_WithComparison_Sorts()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = Comparer<int>.Default.ThenBy(Comparer<int>.Default.Compare);
            Array.Sort(values, comparer);

            int[] expected = { 1, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting by key.
        /// </summary>
        [TestMethod]
        public void TestThenBy_WithKeySelector_Sorts()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = Comparer<int>.Default.ThenBy(i => i);
            Array.Sort(values, comparer);

            int[] expected = { 1, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting by key, using a key comparer.
        /// </summary>
        [TestMethod]
        public void TestThenBy_WithKeyComparer_Sorts()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = Comparer<int>.Default.ThenBy(i => i, Comparer<int>.Default);
            Array.Sort(values, comparer);

            int[] expected = { 1, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting by key, using a key comparison.
        /// </summary>
        [TestMethod]
        public void TestThenBy_WithKeyComparison_Sorts()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = Comparer<int>.Default.ThenBy(i => i, Comparer<int>.Default.Compare);
            Array.Sort(values, comparer);

            int[] expected = { 1, 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// Everytime we add another comparison, it should leave the previous comparison alone.
        /// </summary>
        [TestMethod]
        public void TestThenBy_CompoundedComparers_EachComparerStaysIntact()
        {
            IComparer<int> baseComparer = NullComparer<int>.Default;
            IComparer<int> nextComparer = baseComparer.ThenBy(Comparer<int>.Default);
            IComparer<int> finalComparer = nextComparer.ThenBy(Comparer<int>.Default);
            Assert.AreNotSame(baseComparer, nextComparer, "The first comparer was modified or replaced.");
            Assert.AreNotSame(nextComparer, finalComparer, "The second comparer was modified or replaced.");
        }

        /// <summary>
        /// If someone tried to get smart and create a compound NullComparer,
        /// they'll just get a NullComparer back.
        /// </summary>
        [TestMethod]
        public void TestThenBy_TwoNullComparers_ReturnsNullComparer()
        {
            IComparer<int> comparer = NullComparer<int>.Default.ThenBy(NullComparer<int>.Default);
            Assert.AreSame(comparer, NullComparer<int>.Default, "The NullComparer was not returned.");
        }

        /// <summary>
        /// If someone tries to get smart and create a compound comparer including
        /// a NullComparer, the other comparer will be returned.
        /// </summary>
        [TestMethod]
        public void TestThenBy_TrailingNullComparers_ReturnsLeadingComparer()
        {
            IComparer<int> original = Comparer<int>.Default;
            IComparer<int> comparer = original.ThenBy(NullComparer<int>.Default);
            Assert.AreSame(comparer, original, "The original comparer was not returned.");
        }

        /// <summary>
        /// If someone tries to get smart and create a compound comparer including
        /// a NullComparer, the other comparer will be returned.
        /// </summary>
        [TestMethod]
        public void TestThenBy_LeadingNullComparers_ReturnsTrailingComparer()
        {
            IComparer<int> original = Comparer<int>.Default;
            IComparer<int> comparer = NullComparer<int>.Default.ThenBy(original);
            Assert.AreSame(comparer, original, "The original comparer was not returned.");
        }

        #endregion

        #region ThenByDescending

        /// <summary>
        /// We can perform subsequent sorting by key.
        /// </summary>
        [TestMethod]
        public void TestThenByDescending_WithKeySelector_SortsDescending()
        {
            int[] values = { 1, 4, 5, 3, 2, 1 };

            IComparer<int> comparer = NullComparer<int>.Default.ThenByDescending(i => i);
            Array.Sort(values, comparer);

            int[] expected = { 5, 4, 3, 2, 1, 1 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting by key, using a key comparer.
        /// </summary>
        [TestMethod]
        public void TestThenByDescending_WithKeyComparer_SortsDescending()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = NullComparer<int>.Default.ThenByDescending(i => i, Comparer<int>.Default);
            Array.Sort(values, comparer);

            int[] expected = { 5, 4, 3, 2, 1, 1 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        /// <summary>
        /// We can perform subsequent sorting by key, using a key comparison.
        /// </summary>
        [TestMethod]
        public void TestThenByDescending_WithKeyComparison_SortsDescending()
        {
            int[] values = { 1, 5, 4, 3, 2, 1 };

            IComparer<int> comparer = NullComparer<int>.Default.ThenByDescending(i => i, Comparer<int>.Default.Compare);
            Array.Sort(values, comparer);

            int[] expected = { 5, 4, 3, 2, 1, 1 };
            CollectionAssert.AreEqual(expected, values, "The values weren't sorted as expected.");
        }

        #endregion

        #region ToNullableComparer

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestToNullableComparer_TwoNulls_ReturnsZero()
        {
            IComparer<int?> comparer = NullComparer<int>.Default.ToNullableComparer();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a null to a value, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestToNullableComparer_NullAndValue_ReturnsNegative()
        {
            IComparer<int?> comparer = NullComparer<int>.Default.ToNullableComparer();
            int result = comparer.Compare(null, 0);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestToNullableComparer_NullAndValue_ReturnsPositive()
        {
            IComparer<int?> comparer = NullComparer<int>.Default.ToNullableComparer();
            int result = comparer.Compare(0, null);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestToNullableComparer_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<int, int, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<int?> comparer = NullComparer<int>.Default.ThenBy(watcher).ToNullableComparer();
            int result = comparer.Compare(0, 0);
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        #endregion

        #region NullsFirst Nullables

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_Nullable_TwoNulls_ReturnsZero()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsFirst();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a null to a value, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_Nullable_NullAndValue_ReturnsNegative()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsFirst();
            int result = comparer.Compare(null, 0);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_Nullable_NullAndValue_ReturnsPositive()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsFirst();
            int result = comparer.Compare(0, null);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_Nullable_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<int?, int?, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<int?> comparer = NullComparer<int?>.Default.ThenBy(watcher).NullsFirst();
            int result = comparer.Compare(0, 0);
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        #endregion

        #region NullsLast Nullables

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_Nullable_TwoNulls_ReturnsZero()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsLast();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a value to a null, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_Nullable_ValueAndNull_ReturnsNegative()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsLast();
            int result = comparer.Compare(0, null);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_Nullable_NullAndValue_ReturnsPositive()
        {
            IComparer<int?> comparer = NullComparer<int?>.Default.NullsLast();
            int result = comparer.Compare(null, 0);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_Nullable_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<int?, int?, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<int?> comparer = NullComparer<int?>.Default.ThenBy(watcher).NullsLast();
            int result = comparer.Compare(0, 0);
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        #endregion

        #region NullsFirst

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_TwoNulls_ReturnsZero()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a null to a value, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_NullAndValue_ReturnsNegative()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_NullAndValue_ReturnsPositive()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst();
            int result = comparer.Compare(String.Empty, null);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<string, string, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<string> comparer = NullComparer<string>.Default.ThenBy(watcher).NullsFirst();
            int result = comparer.Compare(String.Empty, String.Empty);
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        /// <summary>
        /// If we call NullsFirst followed by NullsFirst, it should
        /// have no affect on the results.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_TwoCalls_SameDirection_IgnoresSecondCall()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst().NullsFirst();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we call NullsFirst followed by NullsLast, the second
        /// call should take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_TwoCalls_DifferentDirection_ChangesDirection()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst().NullsLast();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If we call NullsFirst followed by a keyed NullsLast, the top-level
        /// comparer (NullsFirst) will take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_TwoCalls_DifferentLevels_TopLevelTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst().NullsLast(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// NullsFirst can appear anywhere in a comparison expression.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_FollowedByComparer_TakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst().ThenBy(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// There's nothing to prevent someone from trying to specifying where nulls go
        /// when creating a comparer for value types (even if it doesn't make sense).
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_ValueType_IgnoredNullSpecifier()
        {
            IComparer<int> comparer = Comparer<int>.Default.NullsFirst();
            int result = comparer.Compare(0, 1);
            Assert.IsTrue(result < 0, "The result was not positive.");
        }

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_TwoNulls_ReturnsZero()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsFirst(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = null }, new Person() { LastName = null });
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a null to a value, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_NullAndValue_ReturnsNegative()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsFirst(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = null }, new Person() { LastName = String.Empty });
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_NullAndValue_ReturnsPositive()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsFirst(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = String.Empty }, new Person() { LastName = null });
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<Person, Person, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<Person> comparer = NullComparer<Person>.Default.ThenBy(watcher).NullsFirst(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = String.Empty }, new Person() { LastName = String.Empty });
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        /// <summary>
        /// If we call NullsFirst followed by NullsLast, we give precedence to the second.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_TwoCalls_SameDirection_SecondCallTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst(s => s).NullsLast(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If we call NullsFirst followed by a keyed NullsLast, the top-level
        /// comparer (NullsLast) will take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_TwoCalls_DifferentLevels_TopLevelTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst(s => s).NullsLast();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// NullsFirst can appear anywhere in a comparison expression.
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_FollowedByComparer_TakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsFirst(s => s).ThenBy(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// There's nothing to prevent someone from trying to specifying where nulls go
        /// when creating a comparer for value types (even if it doesn't make sense).
        /// </summary>
        [TestMethod]
        public void TestNullsFirst_KeySelector_ValueType_IgnoredNullSpecifier()
        {
            IComparer<int> comparer = Comparer<int>.Default.NullsFirst(i => i);
            int result = comparer.Compare(0, 1);
            Assert.IsTrue(result < 0, "The result was not positive.");
        }

        #endregion

        #region NullsLast

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_TwoNulls_ReturnsZero()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast();
            int result = comparer.Compare(null, null);
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a value to a null, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_ValueAndNull_ReturnsNegative()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast();
            int result = comparer.Compare(String.Empty, null);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_NullAndValue_ReturnsPositive()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<string, string, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<string> comparer = NullComparer<string>.Default.ThenBy(watcher).NullsLast();
            int result = comparer.Compare(String.Empty, String.Empty);
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        /// <summary>
        /// If we call NullsLast followed by NullsLast, it should
        /// have no affect on the results.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_TwoCalls_SameDirection_IgnoresSecondCall()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast().NullsLast();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// If we call NullsLast followed by NullsFirst, the second
        /// call should take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_TwoCalls_DifferentDirection_ChangesDirection()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast().NullsFirst();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we call NullsLast followed by a keyed NullsFirst, the top-level
        /// comparer (NullsLast) will take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_TwoCalls_DifferentLevels_TopLevelTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast().NullsFirst(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// NullsLast can appear anywhere in a comparison expression.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_FollowedByComparer_TakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast().ThenBy(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// There's nothing to prevent someone from trying to specifying where nulls go
        /// when creating a comparer for value types (even if it doesn't make sense).
        /// </summary>
        [TestMethod]
        public void TestNullsLast_ValueType_IgnoreNullSpecifier()
        {
            IComparer<int> comparer = Comparer<int>.Default.NullsLast();
            int result = comparer.Compare(0, 1);
            Assert.IsTrue(result < 0, "The result was not positive.");
        }

        /// <summary>
        /// If we compare two nulls, a zero should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_TwoNulls_ReturnsZero()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsLast(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = null }, new Person() { LastName = null });
            Assert.AreEqual(0, result, "The result was not zero.");
        }

        /// <summary>
        /// If we compare a null to a value, a positive number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_NullAndValue_ReturnsNegative()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsLast(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = null }, new Person() { LastName = String.Empty });
            Assert.IsTrue(result > 0, "The result was not negative.");
        }

        /// <summary>
        /// If we compare a value to a null, a negative number should be returned.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_NullAndValue_ReturnsPositive()
        {
            IComparer<Person> comparer = NullComparer<Person>.Default.NullsLast(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = String.Empty }, new Person() { LastName = null });
            Assert.IsTrue(result < 0, "The result was not positive.");
        }

        /// <summary>
        /// If neither of the values are null, then the underlying comparer should be called.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_TwoValues_CallsComparer()
        {
            bool wasCalled = false;
            Func<Person, Person, int> watcher = (x, y) => { wasCalled = true; return 0; };
            IComparer<Person> comparer = NullComparer<Person>.Default.ThenBy(watcher).NullsLast(p => p.LastName);
            int result = comparer.Compare(new Person() { LastName = String.Empty }, new Person() { LastName = String.Empty });
            Assert.IsTrue(wasCalled, "The underlying comparer was not called.");
        }

        /// <summary>
        /// If we call NullsLast followed by NullsFirst, we give precedence to the second.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_TwoCalls_SameDirection_SecondCallTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast(s => s).NullsFirst(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// If we call a keyed NullsLast followed by a NullsFirst, the top-level
        /// comparer (NullsFirst) will take precedence.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_TwoCalls_DifferentLevels_TopLevelTakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast(s => s).NullsFirst();
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result < 0, "The result was not negative.");
        }

        /// <summary>
        /// NullsLast can appear anywhere in a comparison expression.
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_FollowedByComparer_TakesPrecedence()
        {
            IComparer<string> comparer = NullComparer<string>.Default.NullsLast(s => s).ThenBy(s => s);
            int result = comparer.Compare(null, String.Empty);
            Assert.IsTrue(result > 0, "The result was not positive.");
        }

        /// <summary>
        /// There's nothing to prevent someone from trying to specifying where nulls go
        /// when creating a comparer for value types (even if it doesn't make sense).
        /// </summary>
        [TestMethod]
        public void TestNullsLast_KeySelector_ValueType_IgnoreNullSpecifier()
        {
            IComparer<int> comparer = Comparer<int>.Default.NullsLast(i => i);
            int result = comparer.Compare(0, 1);
            Assert.IsTrue(result < 0, "The result was not positive.");
        }

        #endregion

        #region Typed

        /// <summary>
        /// An exception should be thrown if the comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTyped_ComparerNull_Throws()
        {
            IComparer original = null;
            ComparerExtensions.Typed<int>(original);
        }

        /// <summary>
        /// If the comparer is already a typed comparer, it should just be cast and returned.
        /// </summary>
        [TestMethod]
        public void TestTyped_IsTyped_ReturnsComparer()
        {
            IComparer original = Comparer<int>.Default;
            IComparer<int> actual = original.Typed<int>();
            Assert.AreSame(original, actual, "The original comparer was not returned.");
        }

        /// <summary>
        /// If the comparer is truly not typed, it should just be wrapped.
        /// </summary>
        [TestMethod]
        public void TestTyped_IsNotTyped_WrapsComparer()
        {
            IComparer original = Comparer.Default;
            IComparer<int> actual = original.Typed<int>();
            Assert.AreNotSame(original, actual, "The original comparer should have been wrapped.");
            int result = actual.Compare(0, 1);
            Assert.IsTrue(result < 0, "The comparer did not act as expected.");
        }

        /// <summary>
        /// We can convert a typed wrapper to an untyped comparer.
        /// </summary>
        [TestMethod]
        public void TestUntyped_UntypedWrapper_IsComparer()
        {
            IComparer original = Comparer.Default;
            IComparer<int> wrapped = original.Typed<int>();
            IComparer untyped = (IComparer)wrapped;
            int result = untyped.Compare(0, 1);
            Assert.IsTrue(result < 0, "The comparer did not act as expected.");
        }

        /// <summary>
        /// If we are trying to untype a typed comparer, we should unwrap it.
        /// </summary>
        [TestMethod]
        public void TestTyped_IsWrapped_Unwraps()
        {
            IComparer original = Comparer.Default;
            IComparer<int> wrapped = original.Typed<int>();
            IComparer untyped = wrapped.Untyped();
            Assert.AreSame(original, untyped, "The comparer was not unwrapped.");
        }

        #endregion

        #region Untyped

        /// <summary>
        /// An exception should be thrown if the comparer is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUntyped_ComparerNull_Throws()
        {
            IComparer<int> original = null;
            ComparerExtensions.Untyped<int>(original);
        }

        /// <summary>
        /// If the comparer is already an untyped comparer, it should just be cast and returned.
        /// </summary>
        [TestMethod]
        public void TestUntyped_IsUntyped_ReturnsComparer()
        {
            IComparer<int> original = Comparer<int>.Default;
            IComparer actual = original.Untyped();
            Assert.AreSame(original, actual, "The original comparer was not returned.");
        }

        /// <summary>
        /// If the comparer is truly not untyped, it should just be wrapped.
        /// </summary>
        [TestMethod]
        public void TestUntyped_IsTyped_WrapsComparer()
        {
            IComparer<int> original = new TypelessComparer<int>();
            IComparer actual = original.Untyped();
            Assert.AreNotSame(original, actual, "The original comparer should have been wrapped.");
            int result = actual.Compare(0, 1);
            Assert.IsTrue(result < 0, "The comparer did not act as expected.");
        }

        /// <summary>
        /// We can convert a typed wrapper to an untyped comparer.
        /// </summary>
        [TestMethod]
        public void TestUntyped_IsWrapped_Unwraps()
        {
            IComparer<int> original = new TypelessComparer<int>();
            IComparer untyped = original.Untyped();
            IComparer<int> unwrapped = untyped.Typed<int>();
            Assert.AreSame(original, unwrapped);
        }

        /// <summary>
        /// We can convert a untyped wrapper to a typed comparer.
        /// </summary>
        [TestMethod]
        public void TestUntyped_TypedWrapper_SupportsComparer()
        {
            IComparer<int> original = new TypelessComparer<int>();
            IComparer wrapped = original.Untyped();
            IComparer<int> typed = (IComparer<int>)wrapped;
            int result = typed.Compare(0, 1);
            Assert.IsTrue(result < 0, "The comparer did not act as expected.");
        }

        private sealed class TypelessComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(x, y);
            }
        }


        #endregion

        #region ToComparer

        /// <summary>
        /// An exception should be thrown if the comparison function is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestToComparer_ComparisonNull_Throws()
        {
            Func<int, int, int> comparison = null;
            ComparerExtensions.ToComparer(comparison);
        }

        /// <summary>
        /// If the comparison is based on an IComparer.Compare function, the comparer will be returned.
        /// </summary>
        [TestMethod]
        public void TestToComparer_IsComparer_ReturnsTarget()
        {
            Func<int, int, int> comparison = Comparer<int>.Default.Compare;
            IComparer<int> comparer = comparison.ToComparer();
            Assert.AreSame(Comparer<int>.Default, comparer, "The wrong comparer was returned.");
        }

        /// <summary>
        /// If the comparison is not based on an IComparer, a wrapper will be returned.
        /// </summary>
        [TestMethod]
        public void TestToComparer_IsNotComparer_Wraps()
        {
            bool isCalled = false;
            Func<int, int, int> comparison = (x, y) =>
            {
                isCalled = true;
                return Comparer<int>.Default.Compare(x, y);
            };
            IComparer<int> comparer = comparison.ToComparer();
            Assert.AreNotSame(Comparer<int>.Default, comparer, "The wrong comparer was returned.");
            comparer.Compare(0, 1);
            Assert.IsTrue(isCalled, "The comparison function was not called.");
        }

        #endregion
    }
}
