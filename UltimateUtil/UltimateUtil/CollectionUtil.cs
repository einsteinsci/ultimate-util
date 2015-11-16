using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateUtil.Fluid;

namespace UltimateUtil
{
	/// <summary>
	/// Various utilities involving classes and interfaces that extend <see cref="IEnumerable"/>
	/// </summary>
	public static class CollectionUtil
	{
		/// <summary>
		/// Creates a more readable string for collections, showing the contents of the
		/// collection rather than the count. Do not use for large collections.
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="collection">Collection to be expanded</param>
		/// <param name="separator">String to separate items</param>
		/// <param name="includeBraces">Whether to include braces on each before and after the contents</param>
		/// <returns>A string showing the items within <paramref name="collection"/></returns>
		public static string ToReadableString<T>(this IEnumerable<T> collection, 
			string separator = ", ", bool includeBraces = true)
		{
			List<string> elements = new List<string>();
			foreach (T t in collection)
			{
				elements.Add(t.ToString());
			}

			string res = string.Join(separator, elements);

			if (includeBraces)
			{
				res = "{ " + res + " }";
			}

			return res;
		}

		/// <summary>
		/// Creates a string based on combining the results of a conversion function applied
		/// to a collection. Essentially, converts a collection to strings and joins them together.
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="ien">Collection to convert</param>
		/// <param name="toString">Converter function</param>
		/// <param name="separator">Separator string inserted between items</param>
		/// <param name="includeBraces">
		/// Whether to add braces before the first element and after the last, to match C# array literals
		/// </param>
		/// <returns>A combined string from all the results of <paramref name="toString"/></returns>
		public static string ToReadableString<T>(this IEnumerable<T> ien, Func<T, string> toString,
			string separator = ", ", bool includeBraces = true)
		{
			List<string> converted = new List<string>();
			foreach (T t in ien)
			{
				converted.Add(toString(t));
			}

			string res = string.Join(separator, converted);

			if (includeBraces)
			{
				res = "{ " + res + " }";
			}

			return res;
		}

		/// <summary>
		/// Whether the collection is empty, without needing <c>Count() == 0</c>
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="collection">Collection to check</param>
		/// <returns><c>true</c> if the collection has zero items, <c>false</c> if not</returns>
		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			return collection.Count() == 0;
		}

		/// <summary>
		/// Whether the collection is <c>null</c> or empty.
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="collection">Collection to check</param>
		/// <returns><c>true</c> the collection is <c>null</c> or has zero items, <c>false</c> if not</returns>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			return collection == null || collection.IsEmpty();
		}

		/// <summary>
		/// Adds a collection of items to an <see cref="IList{T}"/>, rather than
		/// an implementation of it.
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to add to</param>
		/// <param name="added">Collection of items to add</param>
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> added)
		{
			foreach (T t in added)
			{
				list.Add(t);
			}
		}
		/// <summary>
		/// Adds various items to a list through <c>params</c>, for convenience.
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to add to</param>
		/// <param name="added">Array of items to add</param>
		public static void AddRange<T>(this IList<T> list, params T[] added)
		{
			foreach (T t in added)
			{
				list.Add(t);
			}
		}

		/// <summary>
		/// Inserts an item at a given location if the item is not already present
		/// in the list.
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to insert into</param>
		/// <param name="index">Index to insert the item if it is not found</param>
		/// <param name="item">Item to insert</param>
		/// <returns>
		/// <c>true</c> if insertion was successful, <c>false</c> if the item was already 
		/// found within the list
		/// </returns>
		public static bool InsertIfMissing<T>(this IList<T> list, int index, T item)
		{
			if (list.Contains(item))
			{
				return false;
			}

			list.Insert(index, item);
			return true;
		}

		/// <summary>
		/// Adds an item to the list if the item is not already present within it.
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to add to</param>
		/// <param name="item">Item to add</param>
		/// <returns>
		/// <c>true</c> adding was successful, <c>false</c> if the item was already found 
		/// within the list
		/// </returns>
		public static bool AddIfMissing<T>(this IList<T> list, T item)
		{
			if (list.Contains(item))
			{
				return false;
			}

			list.Add(item);
			return true;
		}

		/// <summary>
		/// Returns the index of the first item to match the given predicate
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to check</param>
		/// <param name="predicate">Predicate for determining a match</param>
		/// <returns>The index of the first match, or <c>-1</c> if none was found</returns>
		public static int IndexOf<T>(this IList<T> list, Predicate<T> predicate)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (predicate(list[i]))
				{
					return i;
				}
			}

			return -1;
		}
		/// <summary>
		/// Returns the index of the last item to match the given predicate
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to check</param>
		/// <param name="predicate">Predicate for determining a match</param>
		/// <returns>The index of the last match, or <c>-1</c> if none was found</returns>
		public static int LastIndexOf<T>(this IList<T> list, Predicate<T> predicate)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (predicate(list[i]))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Randomly selects an item in a list
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to check</param>
		/// <param name="random">Random to use</param>
		/// <returns>A random item from within <paramref name="list"/></returns>
		public static T SelectRandom<T>(this IList<T> list, Random random)
		{
			return list[random.Next(list.Count)];
		}
		/// <summary>
		/// Randomly selects an item in an array
		/// </summary>
		/// <typeparam name="T">Array type</typeparam>
		/// <param name="array">Array to check</param>
		/// <param name="random">Random to use</param>
		/// <returns>A random item from within <paramref name="array"/></returns>
		public static T SelectRandom<T>(this T[] array, Random random)
		{
			return array[random.Next(array.Length)];
		}

		/// <summary>
		/// Randomly selects an item in a list, from a given seed
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to check</param>
		/// <param name="seed">Seed to initialize <see cref="Random"/> with</param>
		/// <returns>A random item from within <paramref name="list"/></returns>
		public static T SelectRandom<T>(this IList<T> list, int seed)
		{
			return list.SelectRandom(new Random(seed));
		}
		/// <summary>
		/// Randomly selects an item in an array, from a given seed
		/// </summary>
		/// <typeparam name="T">Array type</typeparam>
		/// <param name="array">Array to check</param>
		/// <param name="seed">Seed to initialize <see cref="Random"/> with</param>
		/// <returns>A random item from within <paramref name="array"/></returns>
		public static T SelectRandom<T>(this T[] array, int seed)
		{
			return array.SelectRandom(new Random(seed));
		}

		/// <summary>
		/// Randomly selects an item in a list, with the default seed
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list">List to check</param>
		/// <returns>A random item from within <paramref name="list"/></returns>
		public static T SelectRandom<T>(this IList<T> list)
		{
			return list.SelectRandom(new Random());
		}
		/// <summary>
		/// Randomly selects an item in an array, with the default seed
		/// </summary>
		/// <typeparam name="T">Array type</typeparam>
		/// <param name="array">Array to check</param>
		/// <returns>A random item from within <paramref name="array"/></returns>
		public static T SelectRandom<T>(this T[] array)
		{
			return array.SelectRandom(new Random());
		}

		/// <summary>
		/// Randomly selects an item from one of the given options
		/// </summary>
		/// <typeparam name="T">Type of options</typeparam>
		/// <param name="rand">Random to generate from</param>
		/// <param name="options">Options from which to select</param>
		/// <returns>One of the items from <paramref name="options"/></returns>
		public static T NextItem<T>(this Random rand, params T[] options)
		{
			return options.SelectRandom(rand);
		}
		/// <summary>
		/// Randomly selects an item from a list of options
		/// </summary>
		/// <typeparam name="T">Type of options</typeparam>
		/// <param name="rand">Random to generate from</param>
		/// <param name="options">Options from which to select</param>
		/// <returns>One of the items from <paramref name="options"/></returns>
		public static T NextItem<T>(this Random rand, IList<T> options)
		{
			return options.SelectRandom(rand);
		}

		/// <summary>
		/// Sets a key to a value on the dictionary, or adds a value to it under the specified key
		/// if none exists
		/// </summary>
		/// <typeparam name="TKey">Key type in dictionary</typeparam>
		/// <typeparam name="TValue">Value type in dictionary</typeparam>
		/// <param name="dictionary">Dictionary in which to set the value</param>
		/// <param name="key">Key of entry to set</param>
		/// <param name="value">Value to set</param>
		public static void Put<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (dictionary.ContainsKey(key))
			{
				dictionary[key] = value;
			}
			else
			{
				dictionary.Add(key, value);
			}
		}

		/// <summary>
		/// Gets a value from a dictionary, or the default value if no key is found
		/// </summary>
		/// <typeparam name="TKey">Key type in dictionary</typeparam>
		/// <typeparam name="TValue">Value type in dictionary</typeparam>
		/// <param name="dictionary">Dictionary to get from</param>
		/// <param name="key">Key of entry to get</param>
		/// <returns>
		/// The value of whatever is registered under <paramref name="key"/>, or 
		/// <c>default(TValue)</c> if no such key is found
		/// </returns>
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}

			return default(TValue);
		}
		/// <summary>
		/// Gets a the first value from a dictionary to match the given predicate, or
		/// the default value if none matches.
		/// </summary>
		/// <typeparam name="TKey">Key type in dictionary</typeparam>
		/// <typeparam name="TValue">Value type in dictionary</typeparam>
		/// <param name="dictionary">Dictionary to search</param>
		/// <param name="predicate">Predicate to match by</param>
		/// <returns>
		/// The first value within <paramref name="dictionary"/> to match <paramref name="predicate"/>,
		/// or <c>default(TValue)</c> if no pair matches
		/// </returns>
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, 
			Func<TKey, TValue, bool> predicate)
		{
			TValue res = default(TValue);
			dictionary.ForEach((k, v) =>
			{
				if (predicate(k, v))
				{
					res = v;
					return true;
				}

				return false;
			});

			return res;
		}

		/// <summary>
		/// Gets a value from a dictionary, regardless of key case
		/// </summary>
		/// <typeparam name="T">Value type in dictionary</typeparam>
		/// <param name="dictionary">Dictionary to get from</param>
		/// <param name="caseInsensitiveKey">Key of entry to get</param>
		/// <returns>
		/// The value of the first entry to match <paramref name="caseInsensitiveKey"/>, or
		/// <c>default(T)</c> if no matching key is found
		/// </returns>
		public static T GetIgnoreCase<T>(this IDictionary<string, T> dictionary, string caseInsensitiveKey)
		{
			if (caseInsensitiveKey.IsNullOrEmpty())
			{
				throw new ArgumentException("Cannot search for key of empty string");
			}

			T res = default(T);
			dictionary.ForEach((k, v) =>
			{
				if (k.Equals(caseInsensitiveKey, StringComparison.InvariantCultureIgnoreCase))
				{
					res = v;
					return true;
				}

				return false;
			});

			return res;
		}

		/// <summary>
		/// Returns whether a given key exists within the dictionary, regardless of case
		/// </summary>
		/// <typeparam name="TValue">Value type in dictionary</typeparam>
		/// <param name="dict">Dictionary to check</param>
		/// <param name="key">Key of entry to check</param>
		/// <returns>
		/// <c>true</c> if <paramref name="key"/> exists within <paramref name="dict"/>,
		/// <c>false</c> if not
		/// </returns>
		public static bool ContainsKeyIgnoreCase<TValue>(this IDictionary<string, TValue> dict, string key)
		{
			foreach (string k in dict.Keys)
			{
				if (k.EqualsIgnoreCase(key))
				{
					return true;
				}
			}

			return false;
		}
		/// <summary>
		/// Returns whether a given value exists within the dictionary, regardless of case
		/// </summary>
		/// <typeparam name="TKey">Key type in dictionary</typeparam>
		/// <param name="dict">Dictionary to check</param>
		/// <param name="value">Value of entry to check</param>
		/// <returns>
		/// <c>true</c> if <paramref name="value"/> exists within <paramref name="dict"/>,
		/// <c>false</c> if not
		/// </returns>
		public static bool ContainsValueIgnoreCase<TKey>(this IDictionary<TKey, string> dict, string value)
		{
			foreach (string v in dict.Values)
			{
				if (v.EqualsIgnoreCase(value))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Converts a collection to another collection from a converter function
		/// </summary>
		/// <typeparam name="TInput">Type of input collection</typeparam>
		/// <typeparam name="TResult">Type of output collection</typeparam>
		/// <param name="input">Input collection</param>
		/// <param name="converter">Function converting from <typeparamref name="TInput"/> to <typeparamref name="TResult"/></param>
		/// <returns>A new collection from converting all members of <paramref name="input"/></returns>
		public static IEnumerable<TResult> ConvertAll<TInput, TResult>(this IEnumerable<TInput> input, Func<TInput, TResult> converter)
		{
			foreach (TInput i in input)
			{
				TResult converted = converter(i);

				yield return converted;
			}
		}

		/// <summary>
		/// Converts anonymous types into a <see cref="Dictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="o">Anonymous type object to convert</param>
		/// <returns>Dictionary with member names as keys</returns>
		public static Dictionary<string, object> ToDictionary(this object o)
		{
			var dictionary = new Dictionary<string, object>();

			foreach (var propertyInfo in o.GetType().GetProperties())
			{
				if (propertyInfo.GetIndexParameters().Length == 0)
				{
					dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
				}
			}

			return dictionary;
		}

		/// <summary>
		/// Returns an empty collection if it is null
		/// </summary>
		/// <typeparam name="T">Type of collection</typeparam>
		/// <param name="input">Collection to check</param>
		/// <returns>
		/// An empty collection if <paramref name="input"/> is <c>null</c>, or
		/// <paramref name="input"/> itself if not
		/// </returns>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> input)
		{
			return input ?? Enumerable.Empty<T>();
		}

		/// <summary>
		/// Creates an array with this as its only member
		/// </summary>
		/// <typeparam name="T">Type of array</typeparam>
		/// <param name="obj">Object to put in. Must inherit from <typeparamref name="T"/></param>
		/// <returns>An array with <paramref name="obj"/> as its only member</returns>
		public static T[] Once<T>(this object obj)
		{
			return new T[] { (T)obj };
		}
	}
}
