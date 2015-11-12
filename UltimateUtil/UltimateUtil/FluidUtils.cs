using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class FluidUtils
	{
		/// <summary>
		/// [FLUID] Runs a <c>foreach</c> loop on an <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type of <paramref name="iterated"/></typeparam>
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
		public static void ForEach<T>(this IEnumerable<T> iterated, Action<T> action)
		{
			IEnumerator<T> i = iterated.GetEnumerator();
			while (i.MoveNext())
			{
				action(i.Current);
			}
		}

		public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
		{
			foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
			{
				action(kvp.Key, kvp.Value);
			}
		}

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
		public static T Cast<T>(this object obj) where T : class
		{
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj));
			}

			return obj as T;
		}

		public static bool Is<T>(this object obj)
		{
			return obj is T;
		}

		public static void ThrowIfNull(this object obj, string varName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(varName);
			}
		}

		public static void With<T>(this T obj, Action<T> applied) where T : class
		{
			applied(obj);
		}
		public static void With<T>(ref T obj, Action<T> applied) where T : struct
		{
			applied(obj);
		}

		public static bool IsNullOrDefault<T>(this T? t) where T : struct
		{
			return t == null || t.Value.Equals(default(T));
		}
		public static bool IsNull<T>(this T t) where T : class
		{
			return t == null;
		}

		public static void Raise<T>(this EventHandler<T> handler, object sender, T e)
		{
			if (handler != null)
			{
				handler(sender, e);
			}
		}
		public static void Raise(this EventHandler handler, object sender, EventArgs e)
		{
			if (handler != null)
			{
				handler(sender, e);
			}
		}
	}
}
