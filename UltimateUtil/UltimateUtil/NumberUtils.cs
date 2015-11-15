using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Various utilities involving numeric types
	/// </summary>
	public static class NumberUtils
	{
		/// <summary>
		/// Returns whether a number is between a range of values, inclusively
		/// </summary>
		/// <typeparam name="T">Type of value to compare</typeparam>
		/// <param name="value">Value to compare</param>
		/// <param name="lower">Lower bound</param>
		/// <param name="upper">Upper bound</param>
		/// <returns>
		/// <c>true</c> if <paramref name="value"/> is between <paramref name="lower"/> and
		/// <paramref name="upper"/> inclusively, <c>false</c> if not
		/// </returns>
		public static bool IsBetween<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
		}
		/// <summary>
		/// Returns whether a number is between a range of values, exclusively
		/// </summary>
		/// <typeparam name="T">Type of value to compare</typeparam>
		/// <param name="value">Value to compare</param>
		/// <param name="lower">Lower bound</param>
		/// <param name="upper">Upper bound</param>
		/// <returns>
		/// <c>true</c> if <paramref name="value"/> is between <paramref name="lower"/> and
		/// <paramref name="upper"/> exclusively, <c>false</c> if not
		/// </returns>
		public static bool IsBetweenExclusive<T>(this T value, T lower, T upper) where T : IComparable<T>
		{
			return value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0;
		}

		/// <summary>
		/// Converts a fractional value to a percent
		/// </summary>
		/// <param name="multiplier">Value to convert</param>
		/// <returns>The number as a percent</returns>
		public static double ToPercent(this double multiplier)
		{
			return multiplier * 100.0;
		}
		/// <summary>
		/// Converts a fractional value to an integer percent
		/// </summary>
		/// <param name="multiplier">Value to convert</param>
		/// <returns>The number as a percent</returns>
		public static int ToPercentInt(this double multiplier)
		{
			return (int)(multiplier * 100.0);
		}

		/// <summary>
		/// Returns a value x1024
		/// </summary>
		/// <param name="value">Value to multiply</param>
		/// <returns><c><paramref name="value"/> * 1024</c></returns>
		public static int K(this int value)
		{
			return value * 1024;
		}
		/// <summary>
		/// Returns a value x1,048,576 (1024^2)
		/// </summary>
		/// <param name="value">Value to multiply</param>
		/// <returns><c><paramref name="value"/> * 1048576</c></returns>
		public static int M(this int value)
		{
			return value.K() * 1024;
		}
		/// <summary>
		/// Returns a value x1,073,741,824 (1024^3)
		/// </summary>
		/// <param name="value">Value to multiply</param>
		/// <returns><c><paramref name="value"/> * 1073741824</c></returns>
		public static long G(this int value)
		{
			return value.M() * 1024L;
		}

		/// <summary>
		/// Formats a number to a currency by culture
		/// </summary>
		/// <param name="value">Value to format</param>
		/// <param name="cultureName">Culture to format to. Defaults to USD ($).</param>
		/// <returns><paramref name="value"/> formatted into a currency</returns>
		public static string ToCurrency(this double value, string cultureName = "en-US")
		{
			CultureInfo culture = new CultureInfo(cultureName);
			return (string.Format(culture, "{0:C}", value));
		}

		/// <summary>
		/// Returns whether an integer is an odd number
		/// </summary>
		/// <param name="value">Value to test</param>
		/// <returns><c>true</c> if <paramref name="value"/> is odd, <c>false</c> if even</returns>
		public static bool IsOdd(this int value)
		{
			return value % 2 == 1;
		}

		/// <summary>
		/// Returns whether an integer is an even number
		/// </summary>
		/// <param name="value">Value to test</param>
		/// <returns><c>true</c> if <paramref name="value"/> is even, <c>false</c> if odd</returns>
		
		public static bool IsEven(this int value)
		{
			return value % 2 == 0;
		}

		/// <summary>
		/// Returns whether an integer is a multiple of a given number
		/// </summary>
		/// <param name="value">Value to test</param>
		/// <param name="factor">Number to test if <paramref name="value"/> is a multiple of</param>
		/// <returns>
		/// <c>true</c> if <paramref name="value"/> is a multiple of <paramref name="factor"/>,
		/// <c>false</c> if not.
		/// </returns>
		public static bool IsMultipleOf(this int value, int factor)
		{
			return value % factor == 0;
		}
	}
}
