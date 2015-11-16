using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Various utility functions involving <see cref="bool"/> values
	/// </summary>
	public static class BooleanUtil
	{
		#region integer to bool
		/// <summary>
		/// Converts a <see cref="long"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="l">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="l"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this long l)
		{
			return l != 0;
		}
		/// <summary>
		/// Converts a <see cref="ulong"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="l">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="l"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this ulong l)
		{
			return l != 0;
		}
		/// <summary>
		/// Converts an <see cref="int"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="n">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="n"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this int n)
		{
			return n != 0;
		}
		/// <summary>
		/// Converts a <see cref="uint"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="n">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="n"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this uint n)
		{
			return n != 0;
		}
		/// <summary>
		/// Converts a <see cref="short"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="s">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="s"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this short s)
		{
			return s != 0;
		}
		/// <summary>
		/// Converts a <see cref="ushort"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="s">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="s"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this ushort s)
		{
			return s != 0;
		}
		/// <summary>
		/// Converts a <see cref="byte"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="b">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="b"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this byte b)
		{
			return b != 0;
		}
		/// <summary>
		/// Converts an <see cref="sbyte"/> into a <see cref="bool"/>.
		/// </summary>
		/// <param name="b">Integer value to convert</param>
		/// <returns><c>false</c> if <c><paramref name="b"/> == 0</c>, <c>true</c> otherwise.</returns>
		public static bool ToBool(this sbyte b)
		{
			return b != 0;
		}
		#endregion integer to bool

		/// <summary>
		/// Converts a <see cref="bool"/> into a <see cref="byte"/>.
		/// </summary>
		/// <param name="b">Boolean to convert</param>
		/// <returns>1 if <c>true</c>, 0 if <c>false</c></returns>
		public static byte ToByte(this bool b)
		{
			return (byte)b.ToInt();
		}
		/// <summary>
		/// Converts a <see cref="bool"/> into an <see cref="int"/>.
		/// </summary>
		/// <param name="b">Boolean to convert</param>
		/// <returns>1 if <c>true</c>, 0 if <c>false</c></returns>
		public static int ToInt(this bool b)
		{
			return b ? 1 : 0;
		}

		/// <summary>
		/// Converts a <see cref="bool?"/> into an <see cref="int"/>.
		/// </summary>
		/// <param name="b">Nullable boolean to convert</param>
		/// <returns>1 if <c>true</c>, -1 if <c>false</c>, 0 if <c>null</c></returns>
		public static int ToInt(this bool? b)
		{
			if (b == null)
			{
				return 0;
			}

			return b.Value ? 1 : -1;
		}

		/// <summary>
		/// Converts a <see cref="bool?"/> into a <see cref="byte"/>.
		/// </summary>
		/// <param name="b">Nullable boolean to convert</param>
		/// <returns>1 if <c>true</c>, -1 if <c>false</c>, 0 if <c>null</c></returns>
		public static sbyte ToSByte(this bool? b)
		{
			return (sbyte)b.ToInt();
		}

		/// <summary>
		/// Parses <see cref="bool"/> by "looser" definitions; allows for "y"/"n", or "1"/"0", etc.
		/// </summary>
		/// <param name="input">Input string to parse</param>
		/// <returns>Resulting boolean</returns>
		public static bool ParseLoose(string input)
		{
			string str = input.ToLower().RemoveWhitespace();
			if (str == "yes" || str == "y" || str == "true" || str == "1")
			{
				return true;
			}
			else if (str == "no" || str == "n" || str == "false" || str == "0" || str == "-1")
			{
				return false;
			}

			throw new FormatException("Invalid bool: " + input);
		}

		/// <summary>
		/// Tries to parse a bool by "looser" definitions
		/// </summary>
		/// <param name="input">Input string to parse</param>
		/// <param name="result">Resulting boolean (<c>false</c> if parsing failed)</param>
		/// <returns><c>true</c> if the parse succeeded, <c>false</c> if not.</returns>
		public static bool TryParseLoose(string input, out bool result)
		{
			try
			{
				result = ParseLoose(input);
				return true;
			}
			catch (FormatException)
			{
				result = false;
				return false;
			}
		}

		/// <summary>
		/// Gets the next random <see cref="bool"/> from a <see cref="Random"/>.
		/// </summary>
		/// <param name="rand"><see cref="Random"/> instance to extract value from</param>
		/// <returns><c>true</c> or <c>false</c>, randomly</returns>
		public static bool NextBool(this Random rand)
		{
			return rand.Next(2) == 0;
		}
	}
}
