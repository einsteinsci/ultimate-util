using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UltimateUtil
{
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
		/// <code>"partOne:partTwo=partThree".FormatSplit("{0}:{1}={2}")</code>
		/// Returns <code>{ "partOne", "partTwo", "partThree" }</code>
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


	}
}
