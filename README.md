# ComparerExtensions

Build IComparer and IEqualityComparer objects using natural language syntax.

## Overview
LINQ provided a lot of useful operations for working with collections. One of the most interesting features was the ability to sort objects by passing a key selector (`Func<TSource, TKey>`) to `OrderBy`. For example:

    IEnumerable<Person> ordered = people.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    
This made it really easy to sort user-defined types, like `Person` in the example above. While this was really awesome when working with LINQ, it was still really hard to work with the built-in collection types. For instance, `List<T>.Sort` requires you to pass an `IComparer<T>` or a `Comparison<T>`. At times, it makes more sense to sort a list in place than it does to create a whole new list.

ComparerExtensions makes the convenience of LINQ's key-based comparers available to the rest of .NET. It also provides additional helpers for handling nulls and building comparers are runtime.

### Building Compound Comparers
If you need to compare user-defined types one or more fields, you can use the `KeyComparer` class.

    IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
    
You can add additional fields to the comparer by calling `ThenBy` as many times as needed. Just like with LINQ, you can change the sort order using the `OrderByDescending` and `ThenByDescending`.

### Building Compound Equality Comparers
If you are working with a `Dictionary` or a `Set`, you might need to specify a different `IEqualityComparer<T>` instance. This saves you from needing to override `Equals` and `GetHashCode`. Both of these functions can be difficult to implement when multiple fields are involved in determining equality. They also don't allow you to change the conditions for equality at runtime.

    IEqualityComparer<Person> comparer = KeyEqualityComparer<Person>.Using(p => p.LastName).And(p => p.FirstName);
    Dictionary<Person, decimal> incomeLookup = new Dictionary<Person, decimal>(comparer);
    HashSet<Person> uniquePeople = new HashSet<Person>(comparer);
    
You can add additional fields to the comparer by calling `And` as many times as needed.

### Handling Nulls
When sorting types that can be null, you might want to place nulls in the front or the back of the collection. Nulls can also cause problems when working with user-defined types because you'll get a `NullReferenceException` when calling a key selector.

ComparerExtensions provides various extension methods for handling nulls, including `NullsFirst` and `NullsLast`.

    IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName).NullsLast();
    
This code will move null instances of `Person` to the back of the collection. By the time the comparer goes to compare by last name all of the null `Person` instances are handled, preventing `NullReferenceException`s.

Some times you want to sort a collection of user-defined types so that null fields are moved to the front or back. There are two ways to do this with ComparerExtensions:

    IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName, Comparer<string>.Default.NullsLast());
    IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName).NullsLast(p => p.LastName);
    
The first approach requires a little more work. It also forces you to specify the `Comparer<T>` that is being used. This approach might actually be easier if you're doing a case insensitive comparison.

The second approach is more convenient because you can configure everything at the top-level and it's less typing.

ComparerExtensions is smart enough to handle mixing of top-level and field-level null handlers. This way, you don't need to worry about the order you call `NullsFirst` and `NullsLast`. You should never get an unexpected `NullReferenceException`.

Sometimes you want to convert an `IComparer<T>` into an `IComparer<T?>`, where `T` is a value type and `T?` is a `Nullable<T>`. The `Comparer<T>` class already handles most primitive types, like `int?`. However, when working with your of value types (`struct`s), you will need to define your own comparer. The `ToNullable` extension method can make this easier. For the sake of the next example, assume `Person` is a value type:

    IComparer<Person> comparer = KeyComparer<Person>.OrderBy(p => p.LastName).ToNullable().NullsLast();
    
Here, the order of the method calls is important! The call to `ToNullable` converts the `KeyComparer<Person>` into an `IComparer<Person?>`. The call to `NullsLast` will also return a `IComparer<Person?>`. If the calls went the other way around, you'd get surprising results because `NullsLast` on a non-nullable type has no affect.
