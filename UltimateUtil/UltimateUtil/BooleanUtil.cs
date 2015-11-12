using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class BooleanUtil
	{
		#region integer to bool
		public static bool ToBool(this long l)
		{
			return l != 0;
		}
		public static bool ToBool(this ulong l)
		{
			return l != 0;
		}
		public static bool ToBool(this int n)
		{
			return n != 0;
		}
		public static bool ToBool(this uint n)
		{
			return n != 0;
		}
		public static bool ToBool(this short s)
		{
			return s != 0;
		}
		public static bool ToBool(this ushort s)
		{
			return s != 0;
		}
		public static bool ToBool(this byte b)
		{
			return b != 0;
		}
		public static bool ToBool(this sbyte b)
		{
			return b != 0;
		}
		#endregion integer to bool

		public static byte ToByte(this bool b)
		{
			return b ? (byte)0 : (byte)1;
		}
		public static int ToInt(this bool b)
		{
			return b ? 0 : 1;
		}

		/// <summary>
		/// Parses bool by "looser" definitions; allows for "y"/"n", or "1"/"0", etc.
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
			else if (str == "no" || str == "n" || str == "false" || str == "0")
			{
				return false;
			}

			throw new FormatException("Invalid bool: " + input);
		}

		/// <summary>
		/// Tries to parse bool by "looser definitions
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

		public static bool NextBool(this Random rand)
		{
			return rand.Next(2) == 0;
		}
		
		public static bool Not(this bool b)
		{
			return !b;
		}
	}
}
