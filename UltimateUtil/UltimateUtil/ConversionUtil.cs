using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	/// <summary>
	/// Contains methods for converting between various <see cref="IConvertible"/> types
	/// </summary>
	public static class ConversionUtil
	{
		/// <summary>
		/// Converts to a given type using <see cref="Convert.ChangeType(object, Type)"/>
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="obj">Object to convert</param>
		/// <returns><paramref name="obj"/> converted to type <typeparamref name="T"/></returns>
		public static T To<T>(this IConvertible obj)
		{
			return (T)Convert.ChangeType(obj, typeof(T));
		}

		/// <summary>
		/// Attempts to convert to a given type using <see cref="Convert.ChangeType(object, Type)"/>,
		/// and returning whether the conversion was successful
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="obj">Object to convert</param>
		/// <param name="result">Output variable to store the converted value</param>
		/// <returns><c>true</c> if conversion was successful, <c>false</c> if not</returns>
		public static bool TryTo<T>(this IConvertible obj, out T result)
		{
			try
			{
				result = obj.To<T>();
				return true;
			}
			catch (FormatException)
			{
				result = default(T);
				return false;
			}
			catch (InvalidCastException)
			{
				result = default(T);
				return false;
			}
		}

		/// <summary>
		/// Attempts to convert to a given type, or returns a value if conversion
		/// was unsuccessful
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="obj">Object to convert</param>
		/// <param name="val">Value to default to if conversion fails</param>
		/// <returns>The converted value, or <paramref name="val"/> if unsuccessful</returns>
		public static T ToOrValue<T>(this IConvertible obj, T val)
		{
			T res;
			if (!obj.TryTo(out res))
			{
				res = val;
			}

			return res;
		}
		/// <summary>
		/// Attempts to convert to a given type, or returns the default value if
		/// conversion was unsuccessful
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="obj">Object to convert</param>
		/// <returns>The converted value, or <c>default(<typeparamref name="T"/>)</c> if unsuccessful</returns>
		public static T ToOrDefault<T>(this IConvertible obj)
		{
			return obj.ToOrValue(default(T));
		}

		/// <summary>
		/// Attempts to parse a <see cref="string"/> using <see cref="Enum.Parse(Type, string, bool)"/> or
		/// <see cref="Convert.ChangeType(object, Type, IFormatProvider)"/> with a given
		/// <see cref="IFormatProvider"/>, defaulting to a value if parsing fails
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="original"><see cref="string"/> to parse</param>
		/// <param name="provider">Format provider for parsing</param>
		/// <param name="defaultValue">The value to default to if conversion fails</param>
		/// <returns>The converted value, or <paramref name="defaultValue"/> if parsing fails</returns>
		public static T Parse<T>(this string original, IFormatProvider provider, T defaultValue)
		{
			T result;
			Type type = typeof(T);

			if (original.IsNullOrEmpty())
				result = defaultValue;
			else
			{
				// need to get the underlying type if T is Nullable<>.

				if (type.IsNullableType())
				{
					type = Nullable.GetUnderlyingType(type);
				}

				try
				{
					// ChangeType doesn't work properly on Enums
					if (type.IsEnum)
					{
						result = (T)Enum.Parse(type, original, true);
					}
					else
					{
						result = (T)Convert.ChangeType(original, type, provider);
					}
				}
				catch   // HACK: what can we do to minimize or avoid raising exceptions as part of 
				{       // normal operation? custom string parsing (regex?) for well-known types? 
						// it would be best to know if you can convert to the desired type before you attempt to do so.
					result = defaultValue;
				}
			}

			return result;
		}
		/// <summary>
		/// Attempts to parse a <see cref="string"/> using <see cref="Enum.Parse(Type, string, bool)"/> or
		/// <see cref="Convert.ChangeType(object, Type)"/> with the current culture, defaulting
		/// to a value if parsing fails
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="original"><see cref="string"/> to parse</param>
		/// <param name="defaultValue">The value to default to if conversion fails</param>
		/// <returns>The converted value, or <paramref name="defaultValue"/> if parsing fails</returns>
		public static T Parse<T>(this string original, T defaultValue)
		{
			return original.Parse(CultureInfo.CurrentCulture, defaultValue);
		}
		/// <summary>
		/// Attempts to parse a <see cref="string"/> using <see cref="Enum.Parse(Type, string, bool)"/> or
		/// <see cref="Convert.ChangeType(object, Type)"/> with the current culture, defaulting to the
		/// type's default value if parsing fails
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="original"><see cref="string"/> to parse</param>
		/// <returns>The converted value, or <c>default(<typeparamref name="T"/>)</c> if parsing fails</returns>
		public static T Parse<T>(this string original)
		{
			return original.Parse(default(T));
		}
	}
}
