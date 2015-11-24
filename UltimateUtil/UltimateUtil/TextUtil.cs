using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Various utilities that involve <see cref="string"/> values.
	/// </summary>
	public static class TextUtil
	{
		/// <summary>
		/// Removes all whitespace from a string
		/// </summary>
		/// <param name="str">Affected string</param>
		/// <returns><paramref name="str"/> without whitespace</returns>
		public static string RemoveWhitespace(this string str)
		{
			return str.RemoveChars(' ', '\t', '\n', '\r', '\f');
		}

		/// <summary>
		/// Removes all instances of the specified characters
		/// </summary>
		/// <param name="str">Affected string</param>
		/// <param name="removed">Characters to be removed</param>
		/// <returns><paramref name="str"/> without the specified characters.</returns>
		public static string RemoveChars(this string str, params char[] removed)
		{
			string result = str;
			foreach (char c in removed)
			{
				result = result.Replace(c.ToString(), "");
			}
			return result;
		}

		/// <summary>
		/// Returns if a character is a letter or not
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>
		/// <c>true</c> if <paramref name="c"/> is a letter, 
		/// <c>false</c> otherwise
		/// </returns>
		public static bool IsAlphabetic(this char c)
		{
			return c.ToLower() != c.ToUpper();
		}

		/// <summary>
		/// Returns if a character is a number or not
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>
		/// <c>true</c> if <paramref name="c"/> is a number,
		/// <c>false</c> otherwise
		/// </returns>
		public static bool IsNumeric(this char c)
		{
			return c >= '0' && c <= '9';
		}

		/// <summary>
		/// Returns if a character is a letter or number
		/// </summary>
		/// <param name="c">Character to test</param>
		/// <returns>
		/// <c>true</c> if <paramref name="c"/> is a letter or number, 
		/// <c>false</c> otherwise
		/// </returns>
		public static bool IsAlphaNumeric(this char c)
		{
			return c.IsAlphabetic() || c.IsNumeric();
		}

		/// <summary>
		/// Makes the character lowercase
		/// </summary>
		/// <param name="c">Character to affect</param>
		/// <returns>A lowercase <paramref name="c"/></returns>
		public static char ToLower(this char c)
		{
			return c.ToString().ToLower()[0];
		}

		/// <summary>
		/// Capitalizes the character
		/// </summary>
		/// <param name="c">Character to capitalize</param>
		/// <returns>A capital <paramref name="c"/></returns>
		public static char ToUpper(this char c)
		{
			return c.ToString().ToUpper()[0];
		}

		/// <summary>
		/// Gets a substring by removing the amount of characters from the
		/// start equal to the length of a given string.
		/// </summary>
		/// <param name="str">String to cut from</param>
		/// <param name="removeStart">String whose length is used to generate a substring</param>
		/// <returns><c><paramref name="str"/>.Substring(<paramref name="removeStart"/>.Length)</c></returns>
		public static string Substring(this string str, string removeStart)
		{
			return str.Substring(removeStart.Length);
		}

		/// <summary>
		/// Splits the string into parts, using format strings to denote the locations
		/// between the splitters
		/// </summary>
		/// <param name="input">String to be split</param>
		/// <param name="format">Format string to specify how <paramref name="input"/> is split</param>
		/// <param name="temporary">
		/// Temporary character to use in the format string. Set it to be a character that is not
		/// used anywhere in the format string. Defaults to newline (<c>'\n'</c>).
		/// </param>
		/// <returns>An array of <see cref="string"/>s split according to <paramref name="format"/></returns>
		/// <exception cref="FormatException">
		/// Thrown if a format piece does not contain a valid integer.
		/// </exception>
		/// <example>
		/// <c>"partOne:partTwo=partThree".FormatSplit("{0}:{1}={2}")</c>
		/// Returns <c>new string[] { "partOne", "partTwo", "partThree" }</c>
		/// </example>
		public static string[] FormatSplit(this string input, string format, char temporary = '\n')
		{
			const string FORMAT_PIECE = @"{\d+}";

			MatchCollection matches = Regex.Matches(format, FORMAT_PIECE);
			if (matches.Count == 0)
			{
				return new string[] { input };
			}

			List<int> order = new List<int>();
			foreach (Match m in matches)
			{
				string numStr = m.Value.TrimEnd('}').TrimStart('{');
				int val;
				if (!int.TryParse(numStr, out val))
				{
					throw new FormatException("Invalid number within " + m.Value);
				}
				order.Add(val);
			}

			string splittersCombined = Regex.Replace(format, FORMAT_PIECE, temporary.ToString());
			List<string> splitters = splittersCombined.Split(temporary).ToList();
			splitters.RemoveAll((s) => s.RemoveWhitespace() == "");

			List<string> pieces = new List<string>();
			int currentStart = 0;
			foreach (string spl in splitters)
			{
				int end = input.IndexOf(spl, currentStart);
				int pieceLength = end - currentStart;
				string piece = input.Substring(currentStart, pieceLength);
				currentStart = end + spl.Length;

				pieces.Add(piece);
			}
			string lastPiece = input.Substring(currentStart);
			pieces.Add(lastPiece);

			string[] res = new string[order.Max() + 1];
			for (int i = 0; i < order.Count; i++)
			{
				int place = order[i];
				res[place] = pieces[i];
			}

			return res;
		}

		/// <summary>
		/// Repeats a specified string a number of times
		/// </summary>
		/// <param name="repeated">String to repeat</param>
		/// <param name="count">Number of times to repeat</param>
		/// <returns>Repeated string</returns>
		public static string Repeat(this string repeated, int count)
		{
			string res = "";
			for (int i = 0; i < count; i++)
			{
				res += repeated;
			}
			return res;
		}

		/// <summary>
		/// Runs <see cref="string.Format(string, object[])"/> through an extension method.
		/// </summary>
		/// <param name="formatted">String to format</param>
		/// <param name="formatArgs">Arguments of format</param>
		/// <returns>Post-formatting string</returns>
		public static string Fmt(this string formatted, params object[] formatArgs)
		{
			return string.Format(formatted, formatArgs);
		}

		/// <summary>
		/// Compares two strings, ignoring case.
		/// </summary>
		/// <param name="str">First string</param>
		/// <param name="other">Second string</param>
		/// <returns>Whether the two strings are equal, ignoring case</returns>
		public static bool EqualsIgnoreCase(this string str, string other)
		{
			return str.Equals(other, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Tests whether a string starts with another string, ignoring case
		/// </summary>
		/// <param name="str">String to search</param>
		/// <param name="other">String to test for</param>
		/// <returns>
		/// <c>true</c> if <paramref name="str"/> starts with <paramref name="other"/>,
		/// <c>false</c> if not, ignoring case
		/// </returns>
		public static bool StartsWithIgnoreCase(this string str, string other)
		{
			return str.StartsWith(other, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Tests whether a string ends with another string, ignoring case
		/// </summary>
		/// <param name="str">String to search</param>
		/// <param name="other">String to test for</param>
		/// <returns>
		/// <c>true</c> if <paramref name="str"/> ends with <paramref name="other"/>,
		/// <c>false</c> if not, ignoring case
		/// </returns>
		public static bool EndsWithIgnoreCase(this string str, string other)
		{
			return str.EndsWith(other, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Tests whether a string contains another string, ignoring case
		/// </summary>
		/// <param name="str">String to search</param>
		/// <param name="other">String to test for</param>
		/// <returns>
		/// <c>true</c> if <paramref name="str"/> contains <paramref name="other"/>,
		/// <c>false</c> if not, ignoring case
		/// </returns>
		public static bool ContainsIgnoreCase(this string str, string other)
		{
			return str.ToLower().Contains(other.ToLower());
		}

		/// <summary>
		/// Tests whether a string is <c>null</c> or empty, an extension method
		/// version of <see cref="string.IsNullOrEmpty(string)"/>
		/// </summary>
		/// <param name="str">String to check</param>
		/// <returns>
		/// <c>true</c> if <paramref name="str"/> is <c>null</c> or 
		/// <see cref="string.Empty"/>, <c>false</c> if not
		/// </returns>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Tests whether a string is <c>null</c>, empty, or whitespace, an extension method
		/// version of <see cref="string.IsNullOrWhiteSpace(string)"/>
		/// </summary>
		/// <param name="str">String to check</param>
		/// <returns>
		/// <c>true</c> if <paramref name="str"/> is <c>null</c>, <see cref="string.Empty"/>, 
		/// or all whitespace, <c>false</c> if not
		/// </returns>
		public static bool IsNullOrWhitespace(this string str)
		{
			return string.IsNullOrWhiteSpace(str);
		}

		/// <summary>
		/// Reverses a string to go back-to-front
		/// </summary>
		/// <param name="str">String to reverse</param>
		/// <returns><paramref name="str"/>, only backwards</returns>
		public static string Reverse(this string str)
		{
			char[] array = str.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}

		/// <summary>
		/// Throws an exception if the string is <c>null</c> or empty
		/// </summary>
		/// <param name="str">String to test</param>
		/// <param name="varname">
		/// Variable name to supply the exception. <c>null</c> calls the default constructor
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if <paramref name="str"/> is <c>null</c> or empty
		/// </exception>
		public static void ThrowIfNullOrEmpty(this string str, string varname = null)
		{
			if (str.IsNullOrEmpty())
			{
				if (varname == null)
				{
					throw new ArgumentNullException();
				}
				else
				{
					throw new ArgumentNullException(varname);
				}
			}
		}

		/// <summary>
		/// Shortens a string to a maximum length, adding a "continued" suffix to indicate it
		/// has been shortened
		/// </summary>
		/// <param name="str">String to shorten</param>
		/// <param name="maxLength">Maximum characters in string before shortening</param>
		/// <param name="suffix">Suffix to append to the string if shortened.</param>
		/// <returns>
		/// <paramref name="str"/> shortened to <paramref name="maxLength"/> characters, 
		/// with <paramref name="suffix"/> appended if <paramref name="str"/> was shortened
		/// </returns>
		public static string Shorten(this string str, int maxLength, string suffix = "...")
		{
			if (str.IsNullOrEmpty() || str.Length <= maxLength)
			{
				return str;
			}

			return str.Substring(0, maxLength) + suffix;
		}
	}
}
