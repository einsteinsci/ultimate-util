using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class CollectionUtil
	{
		public static string ToReadableString<T>(this IEnumerable<T> collection)
		{
			List<string> elements = new List<string>();
			foreach (T t in collection)
			{
				elements.Add(t.ToString());
			}

			return "{ " + string.Join(", ", elements) + " }";
		}

		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			return collection.Count() == 0;
		}

		public static void AddRange<T>(this IList<T> list, params T[] added)
		{
			foreach (T t in added)
			{
				list.Add(t);
			}
		}

		public static bool InsertIfMissing<T>(this IList<T> list, int index, T item)
		{
			if (list.Contains(item))
			{
				return false;
			}

			list.Insert(index, item);
			return true;
		}

		public static bool AddIfMissing<T>(this IList<T> list, T item)
		{
			if (list.Contains(item))
			{
				return false;
			}

			list.Add(item);
			return true;
		}

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

		public static T SelectRandom<T>(this IList<T> list, Random random)
		{
			return list[random.Next(list.Count)];
		}
		public static T SelectRandom<T>(this T[] array, Random random)
		{
			return array[random.Next(array.Length)];
		}

		public static T SelectRandom<T>(this IList<T> list, int seed)
		{
			return list.SelectRandom(new Random(seed));
		}
		public static T SelectRandom<T>(this T[] array, int seed)
		{
			return array.SelectRandom(new Random(seed));
		}

		public static T SelectRandom<T>(this IList<T> list)
		{
			return list.SelectRandom(new Random());
		}
		public static T SelectRandom<T>(this T[] array)
		{
			return array.SelectRandom(new Random());
		}

		public static T OneOf<T>(this Random rand, params T[] options)
		{
			return options.SelectRandom(rand);
		}
		public static T OneOf<T>(this Random rand, IList<T> options)
		{
			return options.SelectRandom(rand);
		}

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

		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
		{
			if (dictionary.ContainsKey(key))
			{
				return dictionary[key];
			}

			return default(TValue);
		}

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

		public static bool IsAnyOf<TValue>(this TValue tested, params TValue[] possibleValues)
		{
			if (possibleValues == null)
			{
				throw new ArgumentNullException(nameof(possibleValues));
			}

			return possibleValues.Contains(tested);
		}

		public static string ToDelimitedString<T>(this IEnumerable<T> ien, Func<T, string> toString, string separator)
		{
			List<string> converted = new List<string>();
			foreach (T t in ien)
			{
				converted.Add(toString(t));
			}

			return string.Join(separator, converted);
		}

		public static IEnumerable<TResult> ConvertAll<TInput, TResult>(this IEnumerable<TInput> input, Func<TInput, TResult> converter)
		{
			foreach (TInput i in input)
			{
				TResult converted = converter(i);

				yield return converted;
			}
		}

		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> input)
		{
			return input ?? Enumerable.Empty<T>();
		}

		public static List<T> List<T>(params T[] items)
		{
			return new List<T>(items);
		}
	}
}
