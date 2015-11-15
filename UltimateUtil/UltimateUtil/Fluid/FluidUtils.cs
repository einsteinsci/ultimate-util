using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil.Fluid
{
	/// <summary>
	/// Various utility methods to aid in "fluid" programming: avoiding going back when writing code,
	/// instead using extension methods when possible
	/// </summary>
	public static class FluidUtils
	{
		/// <summary>
		/// [FLUID] Runs a <c>foreach</c> loop on an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="iterated"><see cref="IEnumerable{T}"/> to iterate over</param>
		/// <param name="action">
		/// Action to apply to each element. Return <c>true</c> to <c>break</c>, 
		/// <c>false</c> to <c>continue</c>.
		/// </param>
		/// <returns><c>true</c> if the iteration covered all elements, <c>false</c> if not</returns>
		public static bool ForEach<T>(this IEnumerable<T> iterated, Predicate<T> action)
		{
			IEnumerator<T> i = iterated.GetEnumerator();
			while (i.MoveNext())
			{
				if (action(i.Current))
				{
					return false;
				}
			}

			return true;
		}
		/// <summary>
		/// [FLUID] Runs a <c>foreach</c> loop on an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">Collection type</typeparam>
		/// <param name="iterated"><see cref="IEnumerable{T}"/> to iterate over</param>
		/// <param name="action">Action to apply to each element. Return to <c>continue</c>.</param>
		public static void ForEach<T>(this IEnumerable<T> iterated, Action<T> action)
		{
			IEnumerator<T> i = iterated.GetEnumerator();
			while (i.MoveNext())
			{
				action(i.Current);
			}
		}

		/// <summary>
		/// [FLUID] Runs a <c>foreach</c> loop over an <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <typeparam name="TKey">Dictionary key type</typeparam>
		/// <typeparam name="TValue">Dictionary value type</typeparam>
		/// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/> to iterate over</param>
		/// <param name="action">Action to apply to each element. Return to <c>continue</c>.</param>
		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
			{
				action(kvp.Key, kvp.Value);
			}
		}
		/// <summary>
		/// [FLUID] Runs a <c>foreach</c> loop over an <see cref="IDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <typeparam name="TKey">Dictionary key type</typeparam>
		/// <typeparam name="TValue">Dictionary value type</typeparam>
		/// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/> to iterate over</param>
		/// <param name="action">
		/// Action to apply to each element. Return <c>true</c> to <c>break</c>,
		/// <c>false</c> to <c>continue</c>.
		/// </param>
		/// <returns><c>true</c> if the iteration covered all elements, <c>false</c> if not</returns>
		public static bool ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue, bool> action)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
			{
				if (action(kvp.Key, kvp.Value))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// [FLUID] Casts an object to a type in the traditional way, throwing an exception
		/// if it cannot be cast.
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns><c>(<typeparamref name="T"/>)<paramref name="obj"/></c></returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is <c>null</c></exception>
		/// <exception cref="InvalidCastException">
		/// Thrown if <paramref name="obj"/> cannot be cast to <typeparamref name="T"/>
		/// </exception>
		public static T CastThrow<T>(this object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			if (obj is T)
			{
				return (T)obj;
			}

			throw new InvalidCastException("Cannot cast {0} to {1}."
				.Fmt(obj.GetType().FullName, typeof(T).FullName));
		}
		/// <summary>
		/// [FLUID] Casts an object to a type using the <c>as</c> keyword, returning <c>null</c>
		/// if it cannot be cast.
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns><c><paramref name="obj"/> as <typeparamref name="T"/></c></returns>
		public static T Cast<T>(this object obj) where T : class
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			return obj as T;
		}
		/// <summary>
		/// [FLUID] Casts an object to a type in the traditional way, but returns the default value
		/// if it cannot be cast.
		/// </summary>
		/// <typeparam name="T">Type to cast to</typeparam>
		/// <param name="obj">Object to cast</param>
		/// <returns>
		/// <c>(<typeparamref name="T"/>)<paramref name="obj"/></c>, or <c>default(<typeparamref name="T"/>)</c>
		/// if casting fails
		/// </returns>
		public static T CastOrDefault<T>(this object obj) where T : struct
		{
			try
			{
				return obj.CastThrow<T>();
			}
			catch (InvalidCastException)
			{
				return default(T);
			}
		}

		/// <summary>
		/// [FLUID] Tests whether an object is of a specified type using the <c>is</c> keyword
		/// </summary>
		/// <typeparam name="T">Type to test for</typeparam>
		/// <param name="obj">Object to test</param>
		/// <returns><c><paramref name="obj"/> is <typeparamref name="T"/></c></returns>
		public static bool Is<T>(this object obj)
		{
			return obj is T;
		}

		/// <summary>
		/// [FLUID] Returns if the value is equal to any of the supplied values. Useful for large
		/// <c>if</c> predicates.
		/// </summary>
		/// <typeparam name="TValue">Type of value to test</typeparam>
		/// <param name="tested">Value to test</param>
		/// <param name="possibleValues">Possible options to test against</param>
		/// <returns>
		/// <c>true</c> if <paramref name="tested"/> is any one of <paramref name="possibleValues"/>,
		/// <c>false</c> if not
		/// </returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="possibleValues"/> is null</exception>
		public static bool IsAnyOf<TValue>(this TValue tested, params TValue[] possibleValues)
		{
			if (possibleValues == null)
			{
				throw new ArgumentNullException(nameof(possibleValues));
			}

			return possibleValues.Contains(tested);
		}

		/// <summary>
		/// [FLUID] Creates a list from the given items. Useful for unit testing.
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="items">Items to create list out of</param>
		/// <returns></returns>
		public static List<T> List<T>(params T[] items)
		{
			return new List<T>(items);
		}

		/// <summary>
		/// [FLUID] Throws an <see cref="ArgumentNullException"/> if the object is <c>null</c>.
		/// </summary>
		/// <param name="obj">Object to test</param>
		/// <param name="varName">
		/// Name of variable to supply exception. Supply <c>null</c> to not provide a
		/// variable name.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if <paramref name="obj"/> is <c>null</c>.</exception>
		public static void ThrowIfNull(this object obj, string varName = null)
		{
			if (obj == null)
			{
				if (varName == null)
				{
					throw new ArgumentNullException();
				}
				else
				{
					throw new ArgumentNullException(varName);
				}
			}
		}

		/// <summary>
		/// [FLUID] Uses a lambda expression to help instantiate objects with very
		/// long names, particularly UI controls. Recreates the <c>With</c> structure
		/// from Visual Basic.
		/// </summary>
		/// <typeparam name="T">Type of object to affect</typeparam>
		/// <param name="obj">Object to affect</param>
		/// <param name="applied">Things to do with <paramref name="obj"/></param>
		public static void With<T>(this T obj, Action<T> applied) where T : class
		{
			applied(obj);
		}
		/// <summary>
		/// [FLUID] Uses a lambda expression to help instantiate objects with very
		/// long names, particularly UI controls. Recreates the <c>With</c> structure
		/// from Visual Basic. This method is designed for <c>struct</c> types
		/// </summary>
		/// <typeparam name="T">Type of object to affect</typeparam>
		/// <param name="obj">Object to affect, passed by reference</param>
		/// <param name="applied">Things to do with <paramref name="obj"/></param>
		public static void With<T>(ref T obj, Action<T> applied) where T : struct
		{
			applied(obj);
		}

		/// <summary>
		/// [FLUID] Returns whether an nullable type is <c>null</c> or the default value.
		/// </summary>
		/// <typeparam name="T">Inner type of nullable</typeparam>
		/// <param name="t">Nullable type object to test</param>
		/// <returns>
		/// <c>true</c> if <paramref name="t"/> is <c>null</c> or 
		/// <c>default(<typeparamref name="T"/>)</c>, <c>false</c> if not
		/// </returns>
		public static bool IsNullOrDefault<T>(this T? t) where T : struct
		{
			return t == null || t.Value.Equals(default(T));
		}
		/// <summary>
		/// [FLUID] Returns whether an object is <c>null</c>
		/// </summary>
		/// <typeparam name="T">Type of object to test</typeparam>
		/// <param name="t">Object to test</param>
		/// <returns><c><paramref name="t"/> == null</c></returns>
		public static bool IsNull<T>(this T t) where T : class
		{
			return t == null;
		}

		/// <summary>
		/// [FLUID] Raises an <see cref="EventHandler{TEventArgs}"/> if it's not <c>null</c>
		/// with a given sender and event args
		/// </summary>
		/// <typeparam name="T">Event args type</typeparam>
		/// <param name="handler">Event handler to raise</param>
		/// <param name="sender">Sending object</param>
		/// <param name="e">Event args</param>
		public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
		{
			if (handler != null)
			{
				handler(sender, e);
			}
		}
		/// <summary>
		/// [FLUID] Raises an <see cref="EventHandler"/> if it's not <c>null</c>
		/// with a given sender and event args
		/// </summary>
		/// <param name="handler">Event handler to raise</param>
		/// <param name="sender">Sending object</param>
		/// <param name="e">Event args</param>
		public static void Raise(this EventHandler handler, object sender, EventArgs e)
		{
			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/// <summary>
		/// Inverts a <see cref="bool"/> for fluid programming.
		/// </summary>
		/// <param name="b">Value to invert</param>
		/// <returns><c>!<paramref name="b"/></c></returns>
		public static bool Not(this bool b)
		{
			return !b;
		}
	}
}
