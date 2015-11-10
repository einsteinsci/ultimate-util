using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class NumberUtils
	{
		public static bool IsBetweenInclusive<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
		}
		public static bool IsBetweenExclusive<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0;
		}
	}
}
