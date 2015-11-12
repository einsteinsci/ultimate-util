using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUtil
{
	public static class ConversionUtil
	{
		public static T To<T>(this IConvertible obj)
		{
			return (T)Convert.ChangeType(obj, typeof(T));
		}

		public static bool TryTo<T>(this IConvertible obj, out T result)
		{
			try
			{
				result = obj.To<T>();
				return true;
			}
			catch
			{
				result = default(T);
				return false;
			}
		}

		public static T ToOrValue<T>(this IConvertible obj, T val)
		{
			T res;
			if (!obj.TryTo(out res))
			{
				res = val;
			}

			return res;
		}
		public static T ToOrDefault<T>(this IConvertible obj)
		{
			return obj.ToOrValue(default(T));
		}

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
				catch // HACK: what can we do to minimize or avoid raising exceptions as part of normal operation? custom string parsing (regex?) for well-known types? it would be best to know if you can convert to the desired type before you attempt to do so.
				{
					result = defaultValue;
				}
			}

			return result;
		}
		public static T Parse<T>(this string original, T defaultValue)
		{
			return original.Parse(CultureInfo.CurrentCulture, defaultValue);
		}
		public static T Parse<T>(this string original)
		{
			return original.Parse(default(T));
		}
	}
}
