using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Probably not useful, just an implementation.
	/// </summary>
	/// <typeparam name="T">Boxed type</typeparam>
	public sealed class Box<T>
		where T : struct
	{
		/// <summary>
		/// Value stored in the box
		/// </summary>
		public T Value
		{ get; set; }

		public Box(T val)
		{
			Value = val;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public override bool Equals(object obj)
		{
			return Value.Equals(obj);
		}
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool operator==(Box<T> a, T b)
		{
			return object.Equals(a.Value, b);
		}
		public static bool operator!=(Box<T> a, T b)
		{
			return !object.Equals(a.Value, b);
		}
	}
}
