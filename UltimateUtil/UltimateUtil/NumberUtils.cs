using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class NumberUtils
	{
		public static bool IsBetween<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
		}
		public static bool IsBetweenExclusive<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0;
		}

		public static double ToPercent(this double multiplier)
		{
			return multiplier * 100.0;
		}
		public static double ToPercent(this int multiplier)
		{
			return multiplier * 100.0;
		}

		public static int K(this int value)
		{
			return value * 1024;
		}
		public static int M(this int value)
		{
			return value.K() * 1024;
		}
		public static long G(this int value)
		{
			return value.M() * 1024L;
		}

		public static string ToCurrency(this double value, string cultureName = "en-US")
		{
			CultureInfo culture = new CultureInfo(cultureName);
			return (string.Format(culture, "{0:C}", value));
		}

		public static bool IsOdd(this int value)
		{
			return value % 2 == 1;
		}

		public static bool IsEven(this int value)
		{
			return value % 2 == 0;
		}

		public static bool IsMultipleOf(this int value, int factor)
		{
			return value % factor == 0;
		}
	}
}
